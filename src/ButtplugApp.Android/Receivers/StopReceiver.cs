using Android.Content;
using Android.Widget;
using ButtplugApp.Android.Services;

namespace ButtplugApp.Android.Receivers
{
    [BroadcastReceiver]
    public class StopReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
            context.StopService(new Intent(context, typeof(WebSocketService)));
        }
    }
}