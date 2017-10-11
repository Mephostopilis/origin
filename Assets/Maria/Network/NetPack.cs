using System;
using UnityEngine;

namespace Maria.Network {
    public class NetPack {
        public static int Packlh(byte[] buffer, int start, short n) {
            try {
                int len = 2;
                int i = 0;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int PacklH(byte[] buffer, int start, ushort n) {
            try {
                int len = 2;
                int i = 0;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int Packli(byte[] buffer, int start, int n) {
            try {
                int len = 4;
                int i = 0;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int PacklI(byte[] buffer, int start, uint n) {
            try {
                int i = 0, len = 4;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int Packbi(byte[] buffer, int start, int n) {
            try {
                int i = 0, len = 4;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)(n >> (8 * (3 - i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int PackbI(byte[] buffer, int start, uint n) {
            try {
                // 左移右移就是进位与降维，跟物理存储无关，因为左移右移是在寄存器里九三
                int i = 0, len = 4;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)(n >> (8 * (3 - i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int Packll(byte[] buffer, int start, long n) {
            try {
                int i = 0, len = 8;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int PacklL(byte[] buffer, int start, ulong n) {
            try {
                int i = 0, len = 8;
                for (; i < len; i++) {
                    buffer[start + i] = (byte)((n >> (8 * i)) & 0xff);
                }
                return (start + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogError(ex.Message);
            }
            return start;
        }

        public static int Packlf(byte[] buffer, int start, float n) {
            byte[] res = BitConverter.GetBytes(n);
            UnityEngine.Debug.Assert(BitConverter.ToSingle(res, 0) == n);
            Array.Copy(res, 0, buffer, start, res.Length);
            return (start + res.Length);
        }

        public static int Packld(byte[] buffer, int start, double n) {
            UnityEngine.Debug.Assert(buffer.Length >= (start + 8));
            long nn = BitConverter.DoubleToInt64Bits(n);
            Packll(buffer, start, nn);
            return (start + 8);
        }
    }
}
