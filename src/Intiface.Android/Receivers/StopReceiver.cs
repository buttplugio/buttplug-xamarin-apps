﻿using Android.Content;
using Android.Widget;
using Intiface.Android.Services;
using Xamarin.Forms;
using Intiface.Models;

namespace Intiface.Android.Receivers
{
    [BroadcastReceiver]
    public class StopReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            MessagingCenter.Send(new ServerCommandMessage { Command = ServerCommand.Stop }, nameof(ServerCommandMessage));
        }
    }
}