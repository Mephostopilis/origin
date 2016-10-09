using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Network
{
    class NetUnpack
    {
        public static short Unpacklh(byte[] buffer, int offset)
        {
            short res = 0;
            int len = 2;
            for (int i = 0; i < len; i++)
            {
                res |= (short)(buffer[offset + i] & 0xff << ((len- i) * 8));
            }
            return res;
        }

        public static int Unpackli(byte[] buffer, int offset)
        {
            int res = 0;
            int len = 4;
            for (int i = 0; i < len; i++)
            {
                res |= (buffer[offset + i] & 0xff << ((len - i) * 8));
            }
            return res;
        }

        public static float Unpacklf(byte[] buffer, int offset)
        {
            float res = BitConverter.ToSingle(buffer, offset);
            return res;
        }

        public static double Unpackld(byte[] buffer, int offset)
        {
            double res = BitConverter.ToDouble(buffer, offset);
            return res;
        }
    }
}
