using System;
using UnityEngine;

namespace Maria.Network {
   public class NetPack {
        public static void Packlh(byte[] buffer, int start, short n) {
            try {
                int len = 2;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void PacklH(byte[] buffer, int start, ushort n) {
            try {
                int len = 2;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void Packli(byte[] buffer, int start, int n) {
            try {
                int len = 4;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void PacklI(byte[] buffer, int start, uint n) {
            try {
                int len = 4;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void Packbi(byte[] buffer, int start, int n) {
            try {
                UnityEngine.Debug.Assert(start + 4 <= buffer.Length);
                for (int i = 0; i < 4; i++) {
                    buffer[start + i] = (byte)(n >> (8 * (3 - i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void PackbI(byte[] buffer, int start, uint n) {
            try {
                // 左移右移就是进位与降维，跟物理存储无关，因为左移右移是在寄存器里九三
                UnityEngine.Debug.Assert(start + 4 <= buffer.Length);
                for (int i = 0; i < 4; i++) {
                    buffer[start + i] = (byte)(n >> (8 * (3 - i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void Packll(byte[] buffer, int start, long n) {
            try {
                int len = 8;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void PacklL(byte[] buffer, int start, ulong n) {
            try {
                int len = 8;
                UnityEngine.Debug.Assert(start + len <= buffer.Length);
                for (int i = 0; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        public static void Packlf(byte[] buffer, int start, float n) {
            byte[] res = BitConverter.GetBytes(n);
            UnityEngine.Debug.Assert(BitConverter.ToSingle(res, 0) == n);
            Array.Copy(res, 0, buffer, start, res.Length);
        }

        public static void Packld(byte[] buffer, int start, double n) {
            UnityEngine.Debug.Assert(buffer.Length >= (start + 8));
            long nn = BitConverter.DoubleToInt64Bits(n);
            Packll(buffer, start, nn);
        }
    }
}
