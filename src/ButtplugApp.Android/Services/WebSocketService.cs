using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Com.Koushikdutta.Async.Http;
using Com.Koushikdutta.Async.Http.Server;
using System.Collections.Generic;
using System;
using static Com.Koushikdutta.Async.Http.Server.AsyncHttpServer;
using Com.Koushikdutta.Async.Callback;
using Java.Lang;
using Com.Koushikdutta.Async.Http.Socketio;

namespace ButtplugApp.Android.Services
{
    [Service]
    public class WebSocketService : Service
    {
        internal AsyncHttpServer _server;
        internal Dictionary<IWebSocket, MyWebSocketConnection> _sockets;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _server = new AsyncHttpServer();

            _sockets = new Dictionary<IWebSocket, MyWebSocketConnection>();

            _server.Get("/", new MyTestServer(this));

            // Not working for some reason, giving a 404...
            _server.Websocket("/live", new MyWebSocketServer(this));

            //// ..Sometime later, broadcast!
            //foreach (var webSocket in _sockets.Keys)
            //    webSocket.Send("Fireball!");

            // listen on port 5000
            _server.Listen(5000);
            // browsing http://localhost:5000 will return Hello!!!
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
    }

    internal class MyTestServer : Java.Lang.Object, IHttpServerRequestCallback
    {
        private WebSocketService webSocketService;

        public MyTestServer(WebSocketService webSocketService)
        {
            this.webSocketService = webSocketService;
        }

        public void OnRequest(IAsyncHttpServerRequest request, IAsyncHttpServerResponse response)
        {
            response.Send("Hello!!!");
        }
    }

    internal class MyWebSocketServer : Java.Lang.Object, IWebSocketRequestCallback
    {
        private WebSocketService webSocketService;

        public MyWebSocketServer(WebSocketService webSocketService)
        {
            this.webSocketService = webSocketService;
        }

        public void OnConnected(IWebSocket webSocket, IAsyncHttpServerRequest request)
        {
            var connection = new MyWebSocketConnection(webSocketService, webSocket);

            webSocketService._sockets.Add(webSocket, connection);
        }

    }

    internal class MyWebSocketConnection : Java.Lang.Object, IWebSocketStringCallback, ICompletedCallback
    {
        private readonly WebSocketService _webSocketService;
        private IWebSocket _webSocket;

        public MyWebSocketConnection(WebSocketService webSocketService, IWebSocket webSocket)
        {
            _webSocketService = webSocketService;
            _webSocket = webSocket;

            //Use this to clean up any references to your websocket
            _webSocket.ClosedCallback = this;

            _webSocket.StringCallback = this;
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
                _webSocketService._sockets.Remove(_webSocket);
            }
        }

        public void OnStringAvailable(string s)
        {
            if ("Hello Server".Equals(s))
                _webSocket.Send("Welcome Client!");
        }
    }
}