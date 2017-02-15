using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Encrypt
{
    public class Crypt_CSharp
    {
        public const string DLL = "crypt";

        [StructLayout(LayoutKind.Sequential)]
        public struct PACKAGE
        {
            public IntPtr src;
            public Int32 len;
        }

        /// <summary>
        /// string str = Marshal.PtrToStringAnsi(intPtr)
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE randomkey();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE desencode(PACKAGE key, PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE desdecode(PACKAGE key, PACKAGE encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE hashkey(PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "tohex")]
        public static extern PACKAGE hexencode(PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "fromhex")]
        public static extern PACKAGE hexdecode(PACKAGE encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE hmac64(PACKAGE key1, PACKAGE key2);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE hmac_hash(PACKAGE key, PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE dhsecret(PACKAGE key, PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE dhexchange(PACKAGE key);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "b64encode")]
        public static extern PACKAGE base64encode(PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "b64decode")]
        public static extern PACKAGE base64decode(PACKAGE encrypted);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE sha1(PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern PACKAGE hmac_sha1(PACKAGE src);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pfree(PACKAGE src);
    }
}