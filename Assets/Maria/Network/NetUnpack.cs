using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria.Network {
    public class NetUnpack {
        public static int Unpacklh(byte[] buffer, int offset, out short res) {
            try {
                res = 0;
                res |= (short)(buffer[offset] << (0 * 8));
                res |= (short)(buffer[offset + 1] << (1 * 8));
                return (offset + 1);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
            } finally {
                res = 0;
            }
            return offset;
        }

        public static int UnpacklH(byte[] buffer, int offset, out ushort res) {
            res = 0;
            res |= (ushort)(buffer[offset] << (0 * 8));
            res |= (ushort)(buffer[offset + 1] << (1 * 8));
            return (offset + 1);
        }

        public static int Unpackli(byte[] buffer, int offset, out int res) {
            try {
                res = 0;
                res |= buffer[offset] << (0 * 8);
                res |= buffer[offset + 1] << (1 * 8);
                res |= buffer[offset + 2] << (2 * 8);
                res |= buffer[offset + 3] << (3 * 8);
                return offset + 3;
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.Log(ex.Message);
                throw;
            }
        }

        public static int UnpacklI(byte[] buffer, int offset, out uint res) {
            try {
                res = 0;
                res |= (uint)(buffer[offset] << (0 * 8));
                res |= (uint)(buffer[offset + 1] << (1 * 8));
                res |= (uint)(buffer[offset + 2] << (2 * 8));
                res |= (uint)(buffer[offset + 3] << (3 * 8));
                return (offset + 3);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
            } finally {
                res = 0;
            }
            return offset;
        }

        public static int Unpackbi(byte[] buffer, int offset, out int res) {
            try {
                res = 0;
                res |= buffer[offset] << (3 * 8);
                res |= buffer[offset + 1] << (2 * 8);
                res |= buffer[offset + 2] << (1 * 8);
                res |= buffer[offset + 3] << (0 * 8);
                return (offset + 3);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
            } finally {
                res = 0;
            }
            return offset;
        }

        public static int UnpackbI(byte[] buffer, int offset, out uint res) {
            try {
                res = 0;
                res |= (uint)buffer[offset] << (3 * 8);
                res |= (uint)buffer[offset + 1] << (2 * 8);
                res |= (uint)buffer[offset + 2] << (1 * 8);
                res |= (uint)buffer[offset + 3] << (0 * 8);
                return (offset + 3);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
            } finally {
                res = 0;
            }
            return offset;
        }

        public static int Unpackll(byte[] buffer, int offset, out long res) {
            try {
                res = 0;
                for (int i = 0; i < 8; i++) {
                    res |= ((long)buffer[offset + i]) << (i * 8);
                }
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
            } finally {
                res = 0;
            }
            return offset;
        }

        public static int UnpacklL(byte[] buffer, int offset, out ulong res) {
            try {
                res = 0;
                int i = 0;
                for (; i < 8; i++) {
                    res |= ((ulong)buffer[offset + i]) << (i * 8);
                }
                return (offset + i);
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);

            } finally { res = 0; }
            return offset;
        }

        public static int Unpacklf(byte[] buffer, int offset, out float res) {
            res = BitConverter.ToSingle(buffer, offset);
            return (offset + 3);
        }

        public static int Unpackld(byte[] buffer, int offset, out double res) {
            long t;
            int xoffset = Unpackll(buffer, offset, out t);
            if (xoffset == offset && t == 0) {
                res = 0;
                return offset;
            }
            res = BitConverter.Int64BitsToDouble(t);
            return xoffset;
        }
    }
}
