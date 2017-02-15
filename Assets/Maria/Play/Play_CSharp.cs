using System;
using System.Runtime.InteropServices;

namespace Maria.Play {
    class Play_CSharp
    {
        private const string DLL = "sharpc";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr play_alloc([In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject ex, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject cb);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_free(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_start(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_close(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_kill(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_update(IntPtr self, SharpC.CSObject delta);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int  play_join(IntPtr self, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject uid, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject sid, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject session);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void play_leave(IntPtr self, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject uid, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject sid, [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject session);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int play_fetch(IntPtr self, [In, Out, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject ptr, [In, Out, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject len);

    }
}
