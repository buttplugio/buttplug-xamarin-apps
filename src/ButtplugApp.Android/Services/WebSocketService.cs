using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Com.Koushikdutta.Async.Http;
using Com.Koushikdutta.Async.Http.Server;
using System.Collections.Generic;
using System;
using System.Linq;
using static Com.Koushikdutta.Async.Http.Server.AsyncHttpServer;
using Com.Koushikdutta.Async.Callback;
using Java.Lang;
using Java.Util.Concurrent;
using Android.Support.V4.App;
using ButtplugApp.Android.Receivers;
using Com.Koushikdutta.Async;
using Buttplug.Core;
using ButtplugError = Buttplug.Core.Messages.Error;
using Buttplug.Server;

namespace ButtplugApp.Android.Services
{
    [Service]
    public class WebSocketService : Service, IRunnable
    {
        internal AsyncHttpServer _server;
        internal readonly Dictionary<string, MyWebSocketConnection> _sockets;
        internal readonly IScheduledExecutorService _timer;
        private readonly IButtplugServerFactory _serverFactory;

        public WebSocketService()
        {
            _timer = Executors.NewSingleThreadScheduledExecutor();
            _sockets = new Dictionary<string, MyWebSocketConnection>();
            _serverFactory = null; // serverFactory;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _sockets.Clear();

            _server = new AsyncHttpServer();
            _server.Websocket("/", new MyWebSocketServer(this, _serverFactory));
            _server.Listen(Resources.GetInteger(Resource.Integer.server_port));

            //RaiseStartedEvent();
            CreateForegroundNotification();

            _timer.ScheduleAtFixedRate(this, 3000, 3000, TimeUnit.Milliseconds);
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
                .SetTicker(GetString(Resource.String.app_name))
                .SetContentTitle(GetString(Resource.String.notification_server_running))
                .AddAction(Resource.Drawable.ic_media_stop_light, GetString(Resource.String.notification_stop), pendingReceiver);

            StartForeground(Resource.Id.websocket_notification, notification.Build());
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            _timer.ShutdownNow();
            //EventBus.getDefault().removeAllStickyEvents();
            //EventBus.getDefault().postSticky(new ServerStoppedEvent());
            _server.Stop();
            AsyncServer.Default.Stop(); // no, really, I mean stop

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
            var remoteId = request.ToString();

            // Only allow new connections from the same client
            if (_webSocketService._sockets.Count > 0 && !_webSocketService._sockets.ContainsKey(remoteId))
            {
                webSocket.Send(new ButtplugJsonMessageParser().Serialize(new ButtplugError(
                        "WebSocketServer already in use!", ButtplugError.ErrorClass.ERROR_INIT, ButtplugConsts.SystemMsgId)));
                webSocket.Close();
            }

            var connection = new MyWebSocketConnection(_webSocketService, webSocket, _serverFactory.GetServer());

            if (_webSocketService._sockets.ContainsKey(remoteId))
            {
                // Close the old connection 
                _webSocketService._sockets[remoteId].WebSocket.Close();
                _webSocketService._sockets[remoteId] = connection;
            }
            else
            {
                _webSocketService._sockets.Add(remoteId, connection);
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
                var remoteId = _webSocketService._sockets.First(c => c.Value.WebSocket == WebSocket).Key;
                _webSocketService._sockets.Remove(remoteId);
            }
        }

        public void OnStringAvailable(string s)
        {
            var respMsgs = _buttplugServer.SendMessage(s).Result;
            var respMsg = _buttplugServer.Serialize(respMsgs);

            WebSocket.Send(respMsg);

            if (respMsgs.Any(m => m is ButtplugError && (m as ButtplugError).ErrorCode == ButtplugError.ErrorClass.ERROR_PING && WebSocket != null && WebSocket.IsOpen))
                WebSocket.Close();
        }
    }
}