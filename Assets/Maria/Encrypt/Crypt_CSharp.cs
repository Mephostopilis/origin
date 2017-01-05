using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Encrypt
{
    public class Crypt_CSharp
    {

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
        [DllImport("crypt", EntryPoint = "randomkey", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE randomkey();

        [DllImport("crypt", EntryPoint = "desencode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE desencode(PACKAGE key, PACKAGE src);

        [DllImport("crypt", EntryPoint = "desdecode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE desdecode(PACKAGE key, PACKAGE encrypted);

        [DllImport("crypt", EntryPoint = "hashkey", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hashkey(PACKAGE src);

        [DllImport("crypt", EntryPoint = "tohex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hexencode(PACKAGE src);

        [DllImport("crypt", EntryPoint = "fromhex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hexdecode(PACKAGE encrypted);

        [DllImport("crypt", EntryPoint = "hmac64", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hmac64(PACKAGE key1, PACKAGE key2);

        [DllImport("crypt", EntryPoint = "hmac_hash", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hmac_hash(PACKAGE key, PACKAGE src);

        [DllImport("crypt", EntryPoint = "dhsecret", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE dhsecret(PACKAGE key, PACKAGE src);

        [DllImport("crypt", EntryPoint = "dhexchange", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE dhexchange(PACKAGE key);

        [DllImport("crypt", EntryPoint = "b64encode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE base64encode(PACKAGE src);

        [DllImport("crypt", EntryPoint = "b64decode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE base64decode(PACKAGE encrypted);

        [DllImport("crypt", EntryPoint = "sha1", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE sha1(PACKAGE src);

        [DllImport("crypt", EntryPoint = "hmac_sha1", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern PACKAGE hmac_sha1(PACKAGE src);

        [DllImport("crypt", EntryPoint = "pfree", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void pfree(PACKAGE src);
    }
}