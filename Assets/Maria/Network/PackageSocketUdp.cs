using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using Maria.Encrypt;
using Maria.Rudp;
using System.Text;

namespace Maria.Network {
    public class PackageSocketUdp : DisposeObject {

        public class R {
            public uint Globaltime { get; set; }
            public uint Localtime { get; set; }
            public uint Eventtime { get; set; }
            public uint Session { get; set; }
            public byte[] Data { get; set; }
        }

        public delegate void RecvCB(R r);
        public delegate void SyncCB();

        private Socket _so = null;
        private string _host = String.Empty;
        private int _port = 0;
        private IPEndPoint _remoteEP = null;

        private byte[] _buffer = new byte[3072];

        private Context _ctx;
        private byte[] _secret;
        private uint _session;
        private TimeSync _timeSync;

        private bool _connected = false;
        private RecvCB _recvCb = null;
        private SyncCB _syncCb = null;
        private Rudp.Rudp _u = null;
        private int _last = 0;
        private int _delta = 25; // 0.025s


        public PackageSocketUdp(Context ctx, byte[] secret, uint session) {
            _ctx = ctx;
            _timeSync = _ctx.TiSync;

            _so = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _secret = secret;
            _session = session;
            
            _u = new Rudp.Rudp(_ctx.SharpC, 1, 5);
            _u.OnRecv = RRecv;
            _u.OnSend = RSend;

            int now = _timeSync.LocalTime();
            _last = now;
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _u.Dispose();

            }
            // 清理非托管资源

            _disposed = true;
        }

        public RecvCB OnRecv { get { return _recvCb; } set { _recvCb = value; } }
        public SyncCB OnSync { get { return _syncCb; } set { _syncCb = value; } }

        public void Connect(string host, int port) {
            _host = host;
            _port = port;
            IPAddress addr = IPAddress.Parse(_host);
            _remoteEP = new IPEndPoint(addr, _port);
            _connected = false;
        }

        public void Sync() {
            int now = _timeSync.LocalTime();

            byte[] buffer = new byte[12];
            NetPack.PacklI(buffer, 0, (uint)now);
            NetPack.PacklI(buffer, 4, 0xffffffff);
            NetPack.PacklI(buffer, 8, (uint)_session);
            byte[] head = Crypt.hmac_hash(_secret, buffer);
            byte[] data = new byte[8 + buffer.Length];
            Array.Copy(head, data, 8);
            Array.Copy(buffer, 0, data, 8, buffer.Length);

            _u.Send(data, 0, data.Length);
        }

        public void Send(byte[] data) {
            if (_connected) {
                // 8 + 12 + 4 + data
                //byte[] crypt_head = new byte[8];
                byte[] head = new byte[12];
                byte[] buffer = new byte[8 + 12 + data.Length];

                int local = _timeSync.LocalTime();
                int[] global = _timeSync.GlobalTime();
                NetPack.PacklI(head, 0, (uint)local);
                NetPack.PacklI(head, 4, (uint)global[0]);
                NetPack.PacklI(head, 8, (uint)_session);
                byte[] crypt_head = Crypt.hmac_hash(_secret, head);

                Array.Copy(crypt_head, buffer, 8);
                Array.Copy(head, 0, buffer, 8, 12);
                Array.Copy(data, 0, buffer, 20, data.Length);

                UnityEngine.Debug.Log(string.Format("localtime:{0}, eventtime:{1}, session:{2}", local, global[0], _session));
                _u.Send(data, 0, data.Length);
            } else {
                UnityEngine.Debug.Assert(false);
            }
        }

        public void Update() {
            int sz = 0;
            if (_so.Poll(0, SelectMode.SelectRead)) {
                EndPoint ep = _remoteEP as EndPoint;
                sz = _so.ReceiveFrom(_buffer, 3072, SocketFlags.None, ref ep);
                UnityEngine.Debug.Log(string.Format("size {0}", sz));
            }

            int tick = 0;
            int now = _timeSync.LocalTime();
            if (now - _last >= _delta) {
                _last += _delta;
                tick = 1;   
            }

            if (sz > 0) {
                _u.Update(_buffer, 0, sz, tick);
            } else {
                if (tick > 0) {
                    _u.Update(null, 0, 0, tick);
                }
            }
        }

        private void RSend(byte[] buffer, int start, int len) {
            if (len == 1) {
                if (buffer[0] == 0) {
                }
            } else if (len > 1) {
                _so.SendTo(buffer, start, len, SocketFlags.None, _remoteEP);
            }
        }

        private void RRecv(byte[] buffer, int start, int len) {
            if (len < 0) {
                return;
            }
            int remaining = len;
            int head = 0;
            do {
                UnityEngine.Debug.Assert(len >= 16);
                uint globaltime = NetUnpack.UnpacklI(buffer, head);
                uint localtime = NetUnpack.UnpacklI(buffer, head + 4);
                uint eventtime = NetUnpack.UnpacklI(buffer, head + 8);
                uint session = NetUnpack.UnpacklI(buffer, head + 12);
                head += 16;
                remaining -= 16;
                UnityEngine.Debug.Log(string.Format("localtime:{0}, eventtime:{1}, session:{2}", localtime, eventtime, session));
                if (eventtime == 0xffffffff) {
                    if (session == _session) {
                        _timeSync.Sync((int)localtime, (int)globaltime);
                        if (_syncCb != null) {
                            _connected = true;
                            _syncCb();
                        }
                        UnityEngine.Debug.Assert(remaining == 0);
                        return;
                    }
                } else {
                    if (session == _session) {
                        //_timeSync.Sync((int)localtime, (int)globaltime);
                    }
                    if (remaining > 0) {
                        R r = new R();
                        r.Globaltime = globaltime;
                        r.Localtime = localtime;
                        r.Eventtime = eventtime;
                        r.Session = session;
                        byte[] data = new byte[remaining];
                        Array.Copy(_buffer, head, data, 0, remaining);
                        r.Data = data;
                        if (_recvCb != null) {
                            _recvCb(r);
                        }
                        head += remaining;
                        remaining -= remaining;
                    }
                }
            } while (remaining > 0);
            UnityEngine.Debug.Assert(remaining == 0);
        }
    }
}
