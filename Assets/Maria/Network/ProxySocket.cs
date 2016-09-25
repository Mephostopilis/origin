using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Maria.Network
{


    public enum ProxySocketError
    {
        None,
        SocketShutdown,
        RecviveBufferNotEnough,
        RecviveTimeout,
    }

    public enum ProxySocketType
    {
        Line,
        Header,
    }

    public class ProxySocket
    {
        private PackageSocket sock = new PackageSocket();
        private Queue<byte[]> q = new Queue<byte[]>();

        public ProxySocket()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnConnect(bool connected)
        {
        }

        void OnRecvive(byte[] data, int start, int length)
        {
        }

        void OnDisconnect(SocketError socketError, PackageSocketError packageSocketError)
        {
        }

        public void Send(byte[] buffer)
        {
            sock.Send(buffer, 0, buffer.Length);
        }

        public void SetProxySocketType(ProxySocketType type)
        {
            switch (type)
            {
                case ProxySocketType.Line:
                    sock.SetPackageSocketType(PackageSocketType.Line);
                    break;
                case ProxySocketType.Header:
                    sock.SetPackageSocketType(PackageSocketType.Header);
                    break;
            }
        }
    }

}