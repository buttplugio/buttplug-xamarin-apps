using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using ButtplugApp.Android.Services;

namespace ButtplugApp.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //var intent = new Intent(this, typeof(WebSocketService));
            //StartService(intent);
        }
    }
}

