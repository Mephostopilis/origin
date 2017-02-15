using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Maria {
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

        public delegate int pfunc(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = maxArgs)] CSObject[] argv, int args, int res);

        public static SharpObject cache = new SharpObject();
        public const int maxArgs = 8;
        public const string DLL = "sharpc";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sharpc_alloc(pfunc func);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_free(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_log(IntPtr self, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] CSObject[] xx);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void test();

        private IntPtr _sharpc = IntPtr.Zero;

        public SharpC() {
            try {
                _sharpc = sharpc_alloc(SharpC.CallCSharp);
                CacheLog();
            } catch (DllNotFoundException ex) {
                Debug.LogException(ex);
            }
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
            }
            // 清理非托管资源
            sharpc_free(_sharpc);

            _disposed = true;
        }

        [MonoPInvokeCallback(typeof(pfunc))]
        public static int CallCSharp(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = maxArgs)] CSObject[] argv, int args, int res) {
            UnityEngine.Debug.Assert(argc > 0);
            CSObject o = argv[0];
            if (o.type == CSType.SHARPFUNCTION) {
                pfunc f = (pfunc)cache.Get(o.v32);
                return f(argc, argv, args, res);
            }
            return 0;
        }

        public CSObject CacheFunc(pfunc func) {
            CSObject o = new CSObject();
            o.type = CSType.SHARPFUNCTION;
            o.v32 = cache.AddKey(func);
            return o;
        }

        public CSObject CacheObj(object obj) {
            CSObject o = new CSObject();
            o.type = CSType.SHARPOBJECT;
            o.v32 = cache.AddKey(obj);
            return o;
        }

        public static int Log(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] SharpC.CSObject[] argv, int args, int res) {
            Debug.Assert(args + res + 1 <= 8);

            Debug.Assert(argv[1].type == CSType.INT32);
            Debug.Assert(argv[2].type == CSType.STRING);

            string msg = Marshal.PtrToStringAnsi(argv[2].ptr);

            if (argv[1].v32 == 1) {
                Debug.Log(msg);
            } else if (argv[1].v32 == 2) {
                Debug.LogWarning(msg);
            } else if (argv[1].v32 == 3) {
                Debug.LogError(msg);
            }
            
            return 0;
        }

        protected void CacheLog() {
            CSObject[] args = new CSObject[2];
            CSObject cso = CacheFunc(Log);
            args[0] = cso;
            args[1] = new CSObject();
            args[1].type = CSType.NIL;

            sharpc_log(_sharpc, args);
        }

    }
}
