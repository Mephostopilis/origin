using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace Maria.Rudp {
    public class Rudp : DisposeObject {
        public delegate void Callback(byte[] buffer, int start, int len);

        private SharpC _sharpc = null;
        private IntPtr _u;
        private IntPtr _buffer;
        private static byte[] _sendBuffer;
        private static byte[] _recvBuffer;

        public Rudp(SharpC sharpc, int send_delay, int expired_time) {
            _sharpc = sharpc;

            SharpC.CSObject cso0 = sharpc.CacheObj(this);
            SharpC.CSObject cso1 = sharpc.CacheFunc(RSend);
            SharpC.CSObject cso2 = sharpc.CacheFunc(RRecv);

            _u = Rudp_CSharp.rudpaux_alloc(send_delay, expired_time, cso0, cso1, cso2);

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
            Rudp_CSharp.rudpaux_free(_u);
            _disposed = true;
        }

        public Callback OnSend { get; set; }
        public Callback OnRecv { get; set; }

        public void Send(byte[] buf, int start, int len) {
            UnityEngine.Debug.Assert(len > 0);

            IntPtr buffer = Marshal.AllocHGlobal(len);
            Marshal.Copy(buf, 0, buffer, len);
            Rudp_CSharp.rudpaux_send(_u, buffer, len);
            Marshal.FreeHGlobal(buffer);
        }

        public void Update(byte[] buf, int start, int len, int tick) {
            if (buf == null || len == 0) {
                Rudp_CSharp.rudpaux_update(_u, IntPtr.Zero, 0, tick);
            } else {
                Marshal.Copy(buf, start, _buffer, len);
                Rudp_CSharp.rudpaux_update(_u, _buffer, len, tick);
            }
        }

        public static int RSend(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] SharpC.CSObject[] argv, int args, int res) {
            UnityEngine.Debug.Assert(args >= 3);
            UnityEngine.Debug.Assert(argv[1].type == SharpC.CSType.SHARPOBJECT);
            Rudp u = (Rudp)SharpC.cache.Get(argv[1].v32);
            IntPtr buffer = argv[2].ptr;
            int len = argv[3].v32;
            if (u.OnSend != null) {
                Marshal.Copy(buffer, _sendBuffer, 0, len);
                u.OnSend(_sendBuffer, 0, len);
            }
            return 0;
        }

        public static int RRecv(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] SharpC.CSObject[] argv, int args, int res) {
            UnityEngine.Debug.Assert(args >= 3);
            UnityEngine.Debug.Assert(argv[1].type == SharpC.CSType.SHARPOBJECT);
            Rudp u = (Rudp)SharpC.cache.Get(argv[1].v32);
            IntPtr buffer = argv[2].ptr;
            int len = argv[3].v32;
            if (u.OnRecv != null) {
                Marshal.Copy(buffer, _recvBuffer, 0, len);
                u.OnRecv(_recvBuffer, 0, len);
            }
            return 0;
        }

    }
}
