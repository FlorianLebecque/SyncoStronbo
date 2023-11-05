using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo.Devices.Socket {

    [DevicesTypeManagerAttriburte("TCPSocketType")]
    internal class SocketDeviceServer : IDevicesTypeManager {

        private List<SocketDeviceClient> clients;

        private TcpListener listener;

        private Thread th;

        //event
        public event EventHandler<IDevice> OnDeviceConnected;
        public event EventHandler<IDevice> OnDeviceDisconnected;

        public SocketDeviceServer() {
            clients = new();

            var ipEndPoint = new IPEndPoint(IPAddress.Any, 13000);
            listener =  new(ipEndPoint);

            listener.Start();

            th = new Thread(ConnectHandler); 
            
            th.Start();
        }

        private async void ConnectHandler() {

            while (true) {
                TcpClient handler = await listener.AcceptTcpClientAsync();

                if(handler != null && handler.Connected) {

                    var client = new SocketDeviceClient(handler);

                    clients.Add(client);

                    OnDeviceConnected.Invoke(this, client);
                }
                    
                    //clear disconnected client of list
                foreach(SocketDeviceClient client in clients) {
                    if (!client.IsConnected()) {
                        OnDeviceDisconnected.Invoke(this, client);
                        clients.Remove(client);
                        client.Disconnect();
                    }
                }
            }

        }

    }
}
