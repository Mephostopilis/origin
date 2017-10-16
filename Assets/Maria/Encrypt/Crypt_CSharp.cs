using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Encrypt
{
    public class Crypt_CSharp
    {
        public const string DLL = "mariac";

        /// <summary>
        /// string str = Marshal.PtrToStringAnsi(intPtr)
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr randomkey();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr desencode(IntPtr key, IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr desdecode(IntPtr key, IntPtr encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hashkey(IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "tohex")]
        public static extern IntPtr hexencode(IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "fromhex")]
        public static extern IntPtr hexdecode(IntPtr encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hmac64(IntPtr key1, IntPtr key2);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hmac_hash(IntPtr key, IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dhsecret(IntPtr key, IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dhexchange(IntPtr key);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "b64encode")]
        public static extern IntPtr base64encode(IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "b64decode")]
        public static extern IntPtr base64decode(IntPtr encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sha1(IntPtr src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hmac_sha1(IntPtr src);

    }
}