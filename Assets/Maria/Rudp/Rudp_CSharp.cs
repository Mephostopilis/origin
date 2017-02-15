using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Rudp {
    class Rudp_CSharp {

        public const string DLL = "sharpc";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr rudpaux_alloc(int send_delay, int expired_time, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject ex, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject send, [In, MarshalAs(UnmanagedType.Struct)] SharpC.CSObject recv);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rudpaux_free(IntPtr U);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rudpaux_send(IntPtr U, IntPtr buffer, int sz);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rudpaux_update(IntPtr U, IntPtr buffer, int sz, int tick);
    }
}
