using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using ButtplugApp.Android.Services;
using ButtplugApp.Models;
using System;

namespace ButtplugApp.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            MessagingCenter.Subscribe<ServerCommandMessage>(this, nameof(ServerCommandMessage), OnWebSocketServerMessage);
        }

        private void OnWebSocketServerMessage(ServerCommandMessage message)
        {
            switch (message.Command)
            {
                case ServerCommand.Start:
                    StartService(new Intent(this, typeof(WebSocketService)));
                    break;
                case ServerCommand.Stop:
                    StopService(new Intent(this, typeof(WebSocketService)));
                    break;
            }
        }

        protected override void OnDestroy()
        {
            MessagingCenter.Unsubscribe<ServerCommandMessage>(this, nameof(ServerCommandMessage));

            base.OnDestroy();
        }
    }
}

