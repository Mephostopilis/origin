using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace Maria.Rudp {
    public class Rudp : DisposeObject {
        public delegate void Callback(byte[] buffer, int start, int len);
        private IntPtr _u;
        private IntPtr _buffer;
        private byte[] _sendBuffer;
        private byte[] _recvBuffer;

        public Rudp(int send_delay, int expired_time) {
            _u = Rudp_CSharp.aux_new(send_delay, expired_time, RSend, RRecv);

            _buffer = Marshal.AllocHGlobal(3072);

            _sendBuffer = new byte[3072];
            _recvBuffer = new byte[3072];
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
            }
            // 清理非托管资源
            Marshal.FreeHGlobal(_buffer);
            Rudp_CSharp.aux_delete(_u);
            _disposed = true;
        }

        public Callback OnSend { get; set; }
        public Callback OnRecv { get; set; }

        public void Send(byte[] buf) {
            Debug.Assert(buf.Length != 0);
            IntPtr buffer = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, buffer, buf.Length);
            Rudp_CSharp.aux_send(_u, buffer, buf.Length);
            Marshal.FreeHGlobal(buffer);
        }

        public void Update(byte[] buf, int start, int len, int tick) {
            Marshal.Copy(buf, start, _buffer, len);
            Rudp_CSharp.aux_update(_u, _buffer, len, tick);
        }

        [MonoPInvokeCallback(typeof(Rudp_CSharp.Callback))]
        public void RSend(IntPtr buffer, int len) {
            Marshal.Copy(buffer, _sendBuffer, 0, len);
            if (OnSend != null) {
                OnSend(_sendBuffer, 0, len);
            }
        }

        [MonoPInvokeCallback(typeof(Rudp_CSharp.Callback))]
        public void RRecv(IntPtr buffer, int len) {
            Marshal.Copy(buffer, _recvBuffer, 0, len);
            if (OnRecv != null) {
                OnRecv(_recvBuffer, 0, len);
            }
        }
    }
}
