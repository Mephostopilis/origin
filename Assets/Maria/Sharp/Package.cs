using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Maria.Sharp {
    public class Package {

        public const string DLL = "mariac";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr package_alloc(IntPtr src, int size);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int package_size(IntPtr self);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void package_memcpy(IntPtr self, IntPtr dst, int len);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void package_free(IntPtr self);

        public static IntPtr package_packarray(byte[] buffer) {
            IntPtr ptr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            IntPtr package = package_alloc(ptr, buffer.Length);
            Marshal.FreeHGlobal(ptr);
            return package;
        }

        public static byte[] package_unpackarray(IntPtr ptr) {
            int size = package_size(ptr);
            IntPtr h = Marshal.AllocHGlobal(size);
            byte[] buffer = new byte[size];
            package_memcpy(ptr, h, size);
            Marshal.Copy(h, buffer, 0, size);
            Marshal.FreeHGlobal(h);
            return buffer;
        }
    }
}
