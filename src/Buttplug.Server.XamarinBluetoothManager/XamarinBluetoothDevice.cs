using Buttplug.Server.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using Buttplug.Core;
using System.Threading.Tasks;

namespace Buttplug.Server.XamarinBluetoothManager
{
    internal class XamarinBluetoothDevice : IBluetoothDeviceInterface
    {
        public string Name => throw new NotImplementedException();

        public event EventHandler DeviceRemoved;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public ulong GetAddress()
        {
            throw new NotImplementedException();
        }

        public Task<ButtplugMessage> WriteValue(uint aMsgId, Guid aCharacteristicIndex, byte[] aValue, bool aWriteWithResponse = false)
        {
            throw new NotImplementedException();
        }
    }
}
