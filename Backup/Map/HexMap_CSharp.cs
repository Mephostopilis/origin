using Maria.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Map {
    class HexMap_CSharp {
        public const string DLL = "mariac";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hexmapaux_create(IntPtr sc, 
            int o, 
            float oradis, 
            int shape, 
            int width,
            int height,
            [In, MarshalAs(UnmanagedType.Struct)]SharpC.CSObject foreach_cb);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void hexmapaux_release(IntPtr self);

    }
}
