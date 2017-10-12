using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Sharp {
    public class Logger : DisposeObject {
        private SharpC _sharpc = null;
        private SharpC.CSObject _log;
        private IntPtr _ptr = IntPtr.Zero;

        public Logger(SharpC sc) {
            _sharpc = sc;
            _log = _sharpc.CacheFunc(Log);
            _ptr = Logger_CSharp.log_create(sc.CPtr, _log);
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
            }
            // 清理非托管资源

            Logger_CSharp.log_release(_ptr);

            _sharpc.ReleaseFunc(_log);
            _disposed = true;
        }

        public static int Log(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = SharpC.maxArgs)] SharpC.CSObject[] argv) {

            UnityEngine.Debug.Assert(argv[1].type == SharpC.CSType.INT32);
            UnityEngine.Debug.Assert(argv[2].type == SharpC.CSType.STRING);

            string msg = Marshal.PtrToStringAnsi(argv[2].ptr);

            if (argv[1].v32 == 1) {
                UnityEngine.Debug.Log(msg);
            } else if (argv[1].v32 == 2) {
                UnityEngine.Debug.LogWarning(msg);
            } else if (argv[1].v32 == 3) {
                UnityEngine.Debug.LogError(msg);
            }

            return 0;
        }
    }

}
