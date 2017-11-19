using Android.Content;
using Android.Widget;
using ButtplugApp.Android.Services;
using Xamarin.Forms;

using ButtplugApp.Models;

namespace ButtplugApp.Android.Receivers
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