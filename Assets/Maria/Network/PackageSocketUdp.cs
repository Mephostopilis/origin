using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Maria.Network
{
    class PackageSocketUdp
    {
        private Socket _so = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private byte[] _buffer = new byte[1024];
        private int _head = 0;
        private int _tail = 0;
        private int _cap = 1024;
        private byte[] _secret;
        private uint _session;

        public PackageSocketUdp(byte[] secret, uint session)
        {
            _secret = secret;
            _session = session;
        }

        public void Connect(string host, int port)
        {
            _so.Connect(host, port);
        }

        public void Sync(ulong now)
        {
            long nowBD = IPAddress.HostToNetworkOrder((long)now);
            long d = 16;
            long dBD = IPAddress.HostToNetworkOrder(d);
            long sessionBD = IPAddress.HostToNetworkOrder(_session);
            byte[] data = new byte[24];
            Array.Copy(BitConverter.GetBytes(nowBD), 0, data, 0, 8);
            Array.Copy(BitConverter.GetBytes(dBD), 0, data, 8, 8);
            Array.Copy(BitConverter.GetBytes(sessionBD), 0, data, 16, 8);
            _so.Send(data);
        }

        public void Send(byte[] data)
        {
            _so.Send(data);
        }

        public void Recv()
        {
            if (!_so.Poll(0, SelectMode.SelectRead))
            {
                return;
            }
            //_so.Receive()
        }

    }
}
