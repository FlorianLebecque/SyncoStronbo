using SyncoStronbo.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo {
    internal interface IDevicesTypeManager {
        public event EventHandler<IDevice> OnDeviceConnected;
        public event EventHandler<IDevice> OnDeviceDisconnected;
    }
}
