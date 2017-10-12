using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Maria.Sharp {
    public class SharpC : DisposeObject {

        public enum CSType {
            NIL = 0,
            INT32 = 1,
            INT64 = 2,
            REAL = 3,
            BOOLEAN = 4,
            STRING = 5,
            INTPTR = 6,
            SHARPOBJECT = 7,
            SHARPFUNCTION = 8,
            SHARPSTRING = 9,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CSObject {
            //public WeakReference obj { get; set; }

            public CSType type;
            public IntPtr ptr;
            public Int32 v32; // len or key or d
            public Int64 v64;
            public Double f;
        }

        public delegate int pfunc(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = maxArgs)] CSObject[] argv);

        public static SharpObject cache = new SharpObject();
        public const int maxArgs = 8;
        public const string DLL = "mariac";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sharpc_create(pfunc func);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_release(IntPtr self);

        private IntPtr _sharpc = IntPtr.Zero;
        private Logger _logger = null;

        public SharpC() {
            try {
                _sharpc = sharpc_create(SharpC.CallCSharp);
                _logger = new Logger(this);
            } catch (DllNotFoundException ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _logger.Dispose();
            }
            // 清理非托管资源
            sharpc_release(_sharpc);

            _disposed = true;
        }

        [MonoPInvokeCallback(typeof(pfunc))]
        public static int CallCSharp(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = maxArgs)] CSObject[] argv) {
            UnityEngine.Debug.Assert(argc > 0);
            CSObject o = argv[0];
            if (o.type == CSType.SHARPFUNCTION) {
                pfunc f = (pfunc)cache.Get(o.v32);
                return f(argc, argv);
            }
            return 0;
        }

        public CSObject CacheFunc(pfunc func) {
            CSObject o = new CSObject();
            o.type = CSType.SHARPFUNCTION;
            o.v32 = cache.AddKey(func);
            return o;
        }

        public void ReleaseFunc(CSObject o) {
            UnityEngine.Debug.Assert(o.type == CSType.SHARPFUNCTION);
            cache.Remove(o.v32);
        }

        public CSObject CacheObj(object obj) {
            CSObject o = new CSObject();
            o.type = CSType.SHARPOBJECT;
            o.v32 = cache.AddKey(obj);
            return o;
        }

        public void ReleaseObj(CSObject o) {
            UnityEngine.Debug.Assert(o.type == CSType.SHARPOBJECT);
            cache.Remove(o.v32);
        }

        public IntPtr CPtr { get { return _sharpc; } }

    }
}
