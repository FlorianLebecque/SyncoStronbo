using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo.Devices.Socket {
    internal class SocketDeviceClient : IDevice {

        private TcpClient tcpClient;

        public SocketDeviceClient(TcpClient tcpClient_) {
            tcpClient = tcpClient_;

        }

        public bool IsConnected() {
            return tcpClient.Connected;
        }

        public void Disconnect() {
            tcpClient.Close();
        }

        public async void TurnOff() {
            await using NetworkStream stream = tcpClient.GetStream();
            await stream.WriteAsync(Encoding.UTF8.GetBytes("off"));
        }

        public async void TurnOn() {
            await using NetworkStream stream = tcpClient.GetStream();
            await stream.WriteAsync(Encoding.UTF8.GetBytes("on"));
        }
    }
}


