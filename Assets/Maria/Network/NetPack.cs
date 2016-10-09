using System;
using UnityEngine;

namespace Maria.Network
{
    class NetPack
    {
        public static void Packlh(byte[] buffer, int start, short n)
        {
            int len = 2;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void PacklH(byte[] buffer, int start, ushort n)
        {
            int len = 2;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void Packll(byte[] buffer, int start, long n)
        {
            int len = 8;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void PacklL(byte[] buffer, int start, ulong n)
        {
            int len = 8;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void Packli(byte[] buffer, int start, int n)
        {
            int len = 4;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void PacklI(byte[] buffer, int start, uint n)
        {
            int len = 4;
            Debug.Assert(start + len <= buffer.Length);
            for (int i = 0; i < len; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * (len - i)) & 0xff);
            }
        }

        public static void Packlf(byte[] buffer, int start, float n)
        {
            byte[] res = BitConverter.GetBytes(n);
            Array.Copy(res, 0, buffer, start, res.Length);
        }

        public static void Packld(byte[] buffer, int start, double n)
        {
            byte[] res = BitConverter.GetBytes(n);
            Array.Copy(res, 0, buffer, start, res.Length);
        }

        public static void PackbI(byte[] buffer, int start, uint n)
        {
            // 左移右移就是进位与降维，跟物理存储无关，因为左移右移是在寄存器里九三
            int t = 1;
            int tt = t >> 1;    // 0
            int ttt = t << 1;   // 2
            Debug.Assert(start + 4 <= buffer.Length);
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(n >> (8 * i) & 0xff);
            }
        }
    }
}
