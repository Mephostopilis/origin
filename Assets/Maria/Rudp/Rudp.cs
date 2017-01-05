using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Maria.Rudp {
    public class Rudp : DisposeObject {
        public delegate void OnRecvHandler(byte[] buffer, int start, int len);
        private IntPtr _u;
        private OnRecvHandler _recv;
        private IntPtr _recvBuffer;
        private int _recvBuffersz;
        private byte[] _buffer;

        public Rudp(int send_delay, int expired_time) {
            _u = Rudp_CSharp.aux_new(send_delay, expired_time);
            _recv = null;
            _recvBuffer = Marshal.AllocHGlobal(10240);
            _recvBuffersz = 10240;
            _buffer = new byte[_recvBuffersz];
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
            }
            // 清理非托管资源
            Marshal.FreeHGlobal(_recvBuffer);
            Rudp_CSharp.aux_delete(_u);
            _disposed = true;
        }
        
        public OnRecvHandler OnRecv { get { return _recv; } set { _recv = value; } }

        public int Recv() {
            while (true) {
                int res = Rudp_CSharp.aux_recv(_u, _recvBuffer, _recvBuffersz);
                if (res == 0) {
                    return 0;
                } else if (res > 0) {
                    Marshal.Copy(_recvBuffer, _buffer, 0, res);
                    if (_recv != null) {
                        _recv(_buffer, 0, res);
                    }
                } else if (res == -1) {
                    return -1;
                }
            }
        }

        public void Send(byte[] buf) {
            Debug.Assert(buf.Length != 0);
            IntPtr buffer = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, buffer, buf.Length);
            Rudp_CSharp.aux_send(_u, buffer, buf.Length);
            Marshal.FreeHGlobal(buffer);
        }

        public List<byte[]> Update(byte[] buf, int start, int len, int tick) {
            List<byte[]> result = new List<byte[]>();
            IntPtr buffer = IntPtr.Zero;
            int sz = 0;
            if (buf != null && buf.Length > 0) {
                buffer = Marshal.AllocHGlobal(buf.Length);
                Marshal.Copy(buf, 0, buffer, buf.Length);
                sz = buf.Length;
            }
            IntPtr res = Rudp_CSharp.aux_update(_u, buffer, sz, tick);
            while (res != IntPtr.Zero) {
                Rudp_CSharp.rudp_package pack = (Rudp_CSharp.rudp_package)Marshal.PtrToStructure(res, typeof(Rudp_CSharp.rudp_package));
                byte[] d = new byte[pack.sz];
                Marshal.Copy(pack.buffer, d, 0, pack.sz);
                result.Add(d);
                res = pack.next;
            }
            return result;
        }
    }
}
