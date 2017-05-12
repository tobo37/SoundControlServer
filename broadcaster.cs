using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoundControllServer
{
    class broadcaster
    {
        IPEndPoint broadcastAddress;
        UdpClient udpClient;
        public broadcaster()
        {
            this.broadcastAddress = new IPEndPoint(IPAddress.Any, 1234);
            this.udpClient = new UdpClient();
            this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.udpClient.ExclusiveAddressUse = false; // only if you want to send/receive on same machine.
            this.udpClient.Client.Bind(this.broadcastAddress);
            this.udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            //
        }
    }
}
