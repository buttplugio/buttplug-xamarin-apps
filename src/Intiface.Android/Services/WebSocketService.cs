using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;
using Java.Util.Concurrent;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

using Com.Koushikdutta.Async.Http;
using Com.Koushikdutta.Async.Http.Server;
using static Com.Koushikdutta.Async.Http.Server.AsyncHttpServer;
using Com.Koushikdutta.Async.Callback;
using Com.Koushikdutta.Async;
using Xamarin.Forms;

using Buttplug.Core;
using ButtplugError = Buttplug.Core.Messages.Error;

using Buttplug.Server;
using Android.Widget;
using Intiface.Android.Receivers;
using Intiface.Models;
using Plugin.Settings;

namespace Intiface.Android.Services
{
    [Service]
    public class WebSocketService : Service, IRunnable
    {
        internal AsyncHttpServer Server;
        internal readonly Dictionary<string, MyWebSocketConnection> Sockets;
        internal readonly IScheduledExecutorService Timer;
        internal readonly Settings Settings;
        private readonly IButtplugServerFactory _serverFactory;
        private readonly App _app;

        public WebSocketService()
        {
            _app = Xamarin.Forms.Application.Current as App;

            Timer = Executors.NewSingleThreadScheduledExecutor();
            Sockets = new Dictionary<string, MyWebSocketConnection>();
            _serverFactory = DependencyService.Get<IButtplugServerFactory>();

            Settings = new Settings(CrossSettings.Current);
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Sockets.Clear();

            Server = new AsyncHttpServer();
            Server.Websocket("/", new MyWebSocketServer(this, _serverFactory));
            if (Settings.EnableTLS)
            {
                //Server.ListenSecure(Settings.WebSocketPort, );
            }
            else
            {
                Server.Listen(Settings.WebSocketPort);
            }

            //RaiseStartedEvent();
            CreateForegroundNotification();

            Timer.ScheduleAtFixedRate(this, 3000, 3000, TimeUnit.Milliseconds);

            MessagingCenter.Subscribe<ServerCommandMessage>(this, nameof(ServerCommandMessage), OnServerMessage);

            Toast.MakeText(this, Properties.Resource.NotificationServerStarted, ToastLength.Short).Show();
        }

        private void CreateForegroundNotification()
        {

            var activity = new Intent(this, typeof(MainActivity));
            var pendingActivity = PendingIntent.GetActivity(this, 0, activity, 0);

            var receiver = new Intent(this, typeof(StopReceiver));
            var pendingReceiver = PendingIntent.GetBroadcast(this, 0, receiver, 0);

            var notification = new NotificationCompat.Builder(this)
                .SetAutoCancel(true)
                .SetContentIntent(pendingActivity)
                .SetSmallIcon(Resource.Drawable.ic_logo)
                .SetTicker(Properties.Resource.ApplicationName)
                .SetContentTitle(Properties.Resource.NotificationServerRunning)
                .AddAction(Resource.Drawable.ic_media_stop_light, Properties.Resource.ActionServerStop, pendingReceiver);

            StartForeground(Resource.Id.websocket_notification, notification.Build());
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            Timer.ShutdownNow();
            //EventBus.getDefault().removeAllStickyEvents();
            //EventBus.getDefault().postSticky(new ServerStoppedEvent());
            Server.Stop();
            AsyncServer.Default.Stop(); // no, really, I mean stop

            MessagingCenter.Unsubscribe<ServerCommandMessage>(this, nameof(ServerCommandMessage));

            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotSupportedException();
        }

        public void Run()
        {
            // throw new NotImplementedException();
        }

        private void OnServerMessage(ServerCommandMessage message)
        {
            if (message.Command == ServerCommand.Stop)
                this.StopSelf();
        }
    }

    internal class MyWebSocketServer : Java.Lang.Object, IWebSocketRequestCallback
    {
        private readonly WebSocketService _webSocketService;
        private readonly IButtplugServerFactory _serverFactory;

        public MyWebSocketServer(WebSocketService webSocketService, IButtplugServerFactory serverFactory)
        {
            _webSocketService = webSocketService;
            _serverFactory = serverFactory;
        }

        public void OnConnected(IWebSocket webSocket, IAsyncHttpServerRequest request)
        {
            var socket = request.Socket as AsyncNetworkSocket;
            if(socket == null)
            {
                webSocket.Send(new ButtplugJsonMessageParser().Serialize(new ButtplugError(
                        $"Unsupported AsyncSocket type ({request.GetType().FullName})", ButtplugError.ErrorClass.ERROR_INIT, ButtplugConsts.SystemMsgId)));
                webSocket.Close();
                return;
            }

            var remoteId = $"{socket.RemoteAddress.Address.HostAddress}:{socket.RemoteAddress.Port}";

            // Only allow new connections from the same client
            if (_webSocketService.Sockets.Count > 0 && !_webSocketService.Sockets.ContainsKey(remoteId))
            {
                webSocket.Send(new ButtplugJsonMessageParser().Serialize(new ButtplugError(
                        "WebSocketServer already in use!", ButtplugError.ErrorClass.ERROR_INIT, ButtplugConsts.SystemMsgId)));
                webSocket.Close();
                return;
            }

            var connection = new MyWebSocketConnection(_webSocketService, webSocket, _serverFactory.GetServer());

            if (_webSocketService.Sockets.ContainsKey(remoteId))
            {
                // Close the old connection 
                _webSocketService.Sockets[remoteId].WebSocket.Close();
                _webSocketService.Sockets[remoteId] = connection;
            }
            else
            {
                _webSocketService.Sockets.Add(remoteId, connection);
            }
            //ConnectionAccepted?.Invoke(this, new ConnectionEventArgs(remoteId));
        }
    }

    internal class MyWebSocketConnection : Java.Lang.Object, IWebSocketStringCallback, ICompletedCallback
    {
        private readonly WebSocketService _webSocketService;
        public readonly IWebSocket WebSocket;
        private readonly ButtplugServer _buttplugServer;

        public MyWebSocketConnection(WebSocketService webSocketService, IWebSocket webSocket, ButtplugServer buttplugServer)
        {
            _webSocketService = webSocketService;
            WebSocket = webSocket;
            _buttplugServer = buttplugServer;

            //Use this to clean up any references to your websocket
            WebSocket.ClosedCallback = this;

            WebSocket.StringCallback = this;
        }

        public void OnCompleted(Java.Lang.Exception ex)
        {
            try
            {
                //if (ex != null)
                    //("WebSocket", "Error");
            }
            finally
            {
                var remoteId = _webSocketService.Sockets.First(c => c.Value.WebSocket == WebSocket).Key;
                _webSocketService.Sockets.Remove(remoteId);
            }
        }

        public void OnStringAvailable(string s)
        {
            var respMsgs = _buttplugServer.SendMessage(s).GetAwaiter().GetResult();
            var respMsg = _buttplugServer.Serialize(respMsgs);

            WebSocket.Send(respMsg);

            if (respMsgs.Any(m => m is ButtplugError && (m as ButtplugError).ErrorCode == ButtplugError.ErrorClass.ERROR_PING && WebSocket != null && WebSocket.IsOpen))
                WebSocket.Close();
        }
    }
}