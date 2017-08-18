using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria.Network {
    public class NetUnpack {
        public static short Unpacklh(byte[] buffer, int offset) {
            short res = 0;
            res |= (short)(buffer[offset] << (0 * 8));
            res |= (short)(buffer[offset + 1] << (1 * 8));
            return res;
        }

        public static ushort UnpacklH(byte[] buffer, int offset) {
            ushort res = 0;
            res |= (ushort)(buffer[offset] << (0 * 8));
            res |= (ushort)(buffer[offset + 1] << (1 * 8));
            return res;
        }

        public static int Unpackli(byte[] buffer, int offset) {
            try {
                int res = 0;
                res |= buffer[offset] << (0 * 8);
                res |= buffer[offset + 1] << (1 * 8);
                res |= buffer[offset + 2] << (2 * 8);
                res |= buffer[offset + 3] << (3 * 8);
                return res;
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.Log(ex.Message);
                throw;
            }
        }

        public static uint UnpacklI(byte[] buffer, int offset) {
            try {
                uint res = 0;
                res |= (uint)(buffer[offset] << (0 * 8));
                res |= (uint)(buffer[offset + 1] << (1 * 8));
                res |= (uint)(buffer[offset + 2] << (2 * 8));
                res |= (uint)(buffer[offset + 3] << (3 * 8));
                return res;
            } catch (IndexOutOfRangeException ex) {
                UnityEngine.Debug.LogException(ex);
                throw;
            }
        }

        public static int Unpackbi(byte[] buffer, int offset) {
            int res = 0;
            res |= buffer[offset] << (3 * 8);
            res |= buffer[offset + 1] << (2 * 8);
            res |= buffer[offset + 2] << (1 * 8);
            res |= buffer[offset + 3] << (0 * 8);
            return res;
        }

        public static uint UnpackbI(byte[] buffer, int offset) {
            uint res = 0;
            res |= (uint)buffer[offset] << (3 * 8);
            res |= (uint)buffer[offset + 1] << (2 * 8);
            res |= (uint)buffer[offset + 2] << (1 * 8);
            res |= (uint)buffer[offset + 3] << (0 * 8);
            return res;
        }

        public static long Unpackll(byte[] buffer, int offset) {
            UnityEngine.Debug.Assert(buffer.Length >= offset + 8);
            ulong res = 0;
            for (int i = 0; i < 8; i++) {
                res |= ((ulong)buffer[offset + i]) << (i * 8);
            }
            return (long)res;
        }

        public static float Unpacklf(byte[] buffer, int offset) {
            float res = BitConverter.ToSingle(buffer, offset);
            return res;
        }

        public static double Unpackld(byte[] buffer, int offset) {
            UnityEngine.Debug.Assert(buffer.Length >= offset + 8);
            long nn = Unpackll(buffer, offset);
            return BitConverter.Int64BitsToDouble(nn);
        }
    }
}
