using Buttplug.Server.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using Buttplug.Core;

namespace Buttplug.Server.XamarinBluetoothManager
{
    public class XamarinBluetoothManager : BluetoothSubtypeManager
    {
        public XamarinBluetoothManager(IButtplugLogManager logManager) 
            : base(logManager)
        {

        }

        public override bool IsScanning()
        {
            throw new NotImplementedException();
        }

        public override void StartScanning()
        {
            throw new NotImplementedException();
        }

        public override void StopScanning()
        {
            throw new NotImplementedException();
        }
    }
}
