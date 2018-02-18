using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Buttplug.Server;

[assembly: Xamarin.Forms.Dependency(typeof(Intiface.Android.Services.ButtplugServerFactory))]
namespace Intiface.Android.Services
{
    public class ButtplugServerFactory : IButtplugServerFactory
    {
        private DeviceManager _deviceManager;

        Settings Settings => (App.Current as App).Settings;

        public ButtplugServer GetServer()
        {
            ButtplugServer server;

            if (_deviceManager != null)
                return (server = new ButtplugServer(Settings.ServerName, (uint)Settings.WebSocketPing, _deviceManager));

            server = new ButtplugServer(Settings.ServerName, (uint)Settings.WebSocketPing);
            _deviceManager = server.DeviceManager;

            //server.AddDeviceSubtypeManager(aLogger => new UWPBluetoothManager(aLogger));

            return server;
        }
    }
}