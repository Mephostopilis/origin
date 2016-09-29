using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using Maria.Encrypt;

namespace Maria.Network
{
    class PackageSocketUdp
    {
        public class R
        {
            public int Eventtime { get; set; }
            public long Session { get; set; }
            public byte[] Data { get; set; }
        }

        public delegate void RecviveCB(R r);

        private Socket _so = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private string _host = String.Empty;
        private int _port = 0;
        private IPEndPoint _ipEndPt = null;
        private byte[] _buffer = new byte[1024];
        private int _head = 0;
        private int _tail = 0;
        private int _cap = 1024;
        private byte[] _secret;
        private long _session;
        private TimeSync _timeSync;

        public PackageSocketUdp(byte[] secret, long session, TimeSync ts)
        {
            _secret = secret;
            _session = session;
            _timeSync = ts;
        }

        public RecviveCB OnRecviveUdp { get; set; }

        public void Change(byte[] secret, long session)
        {
            _secret = secret;
            _session = session;
        }

        public void Connect(string host, int port)
        {
            _host = host;
            _host = "192.168.199.239";
            _port = port;
            IPAddress ipadd = IPAddress.Parse(_host);
            _ipEndPt = new IPEndPoint(ipadd, _port);
            //_so.Connect(host, port);
        }

        public void Sync()
        {
            int now = _timeSync.LocalTime();
            byte[] buffer = new byte[12];
            Pack(buffer, 0, (uint)now);
            Pack(buffer, 4, 0xffffffff);
            Pack(buffer, 8, (uint)_session);
            byte[] head = Crypt.hmac_hash(_secret, buffer);
            byte[] data = new byte[8 + buffer.Length];
            Array.Copy(head, data, 8);
            Array.Copy(buffer, 0, data, 8, buffer.Length);
            _so.SendTo(data, _ipEndPt);
        }

        public void Send(byte[] data)
        {
            int now = _timeSync.LocalTime();
            byte[] buffer = new byte[12 + data.Length];
            Pack(buffer, 0, (uint)now);
            Pack(buffer, 4, 0xffffffff);
            Pack(buffer, 8, (uint)_session);
            Array.Copy(data, 0, buffer, 12, data.Length);
            byte[] head = Crypt.hmac_hash(_secret, buffer);
            byte[] send = new byte[8 + buffer.Length];
            Array.Copy(head, send, 8);
            Array.Copy(buffer, 0, send, 8, buffer.Length);
            _so.SendTo(send, _ipEndPt);
        }

        public void Update()
        {
            if (!_so.Poll(0, SelectMode.SelectRead))
            {
            }
            else
            {
                while (true)
                {
                    if (_cap - _tail < 100)
                    {
                        Array.Copy(_buffer, 0, _buffer, _head, _tail - _head);
                        _head = 0;
                        _tail = _tail = _tail - _head;
                    }
                    EndPoint ep = _ipEndPt as EndPoint;
                    int sz = _so.ReceiveFrom(_buffer, _tail, _cap - _tail, SocketFlags.None, ref ep);
                    _tail += sz;
                    if (_cap - _tail <= 100)
                    {
                        Array.Copy(_buffer, 0, _buffer, _head, _tail - _head);
                        _head = 0;
                        _tail = _tail = _tail - _head;
                    }
                    int globaltime = UnpackInt(_buffer, _head);
                    int localtime = UnpackInt(_buffer, _head + 4);
                    int eventtime = UnpackInt(_buffer, _head + 8);
                    int session = UnpackInt(_buffer, _head + 12);
                    if (session == _session)
                    {
                        _timeSync.Sync(localtime, globaltime);
                    }

                    int datalen = _tail - _head - 16;
                    if (datalen > 0)
                    {
                        R r = new R();
                        r.Eventtime = eventtime;
                        r.Session = session;

                        byte[] buffer = new byte[datalen];
                        Array.Copy(buffer, 0, _buffer, _head + 16, datalen);
                        r.Data = buffer;
                        OnRecviveUdp(r);
                        _head += datalen;
                        break;
                    }
                    else
                    {
                        _head = _head + 16;
                        break;
                    }
                }
            }
        }

        private void Pack(byte[] buffer, int start, uint n)
        {
            Debug.Assert(start + 4 <= buffer.Length);
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(n >> i & 0xff);
            }
        }

        private int UnpackInt(byte[] buffer, int offset)
        {
            int res = 0;
            res |= buffer[offset] & 0xff;
            res |= buffer[offset + 1] & 0xff << 1;
            res |= buffer[offset + 2] & 0xff << 2;
            res |= buffer[offset + 3] & 0xff << 3;
            return res;
        }
    }
}
