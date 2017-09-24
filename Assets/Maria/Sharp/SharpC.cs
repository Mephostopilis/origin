﻿using AOT;
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

        public delegate int pfunc(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = maxArgs)] CSObject[] argv, int args, int res);

        public static SharpObject cache = new SharpObject();
        public const int maxArgs = 8;
        public const string DLL = "play";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sharpc_create(pfunc func);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_retain(pfunc func);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_release(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void sharpc_callc(IntPtr self, int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] CSObject[] argv, int res);

        private IntPtr _sharpc = IntPtr.Zero;

        public SharpC() {
            try {
                _sharpc = sharpc_create(SharpC.CallCSharp);
                CacheLog();
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
            }
            // 清理非托管资源
            sharpc_release(_sharpc);

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
            UnityEngine.Debug.Assert(args + res + 1 <= 8);

            UnityEngine.Debug.Assert(argv[1].type == CSType.INT32);
            UnityEngine.Debug.Assert(argv[2].type == CSType.STRING);

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

        protected void CacheLog() {
            CSObject[] args = new CSObject[2];
            CSObject cso = CacheFunc(Log);
            args[0] = cso;
            args[1] = new CSObject();
            args[1].type = CSType.NIL;

            //sharpc_log(_sharpc, args);
        }

    }
}
