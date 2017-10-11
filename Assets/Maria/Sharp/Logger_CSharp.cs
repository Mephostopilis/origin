using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Sharp {
    class Logger_CSharp {
        public const string DLL = "mariac";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr log_create(IntPtr sc, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject log);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void log_retain(IntPtr logger);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void log_release(IntPtr logger);

    }
}
