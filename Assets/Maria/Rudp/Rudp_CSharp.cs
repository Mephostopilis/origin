using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Maria.Rudp {
    class Rudp_CSharp {

        public delegate void Callback(IntPtr buffer, int len);

        [DllImport("rudp", EntryPoint = "aux_new", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr aux_new(int send_delay, int expired_time, Callback send, Callback recv);

        [DllImport("rudp", EntryPoint = "aux_delete", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void aux_delete(IntPtr U);

        [DllImport("rudp", EntryPoint = "aux_send", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void aux_send(IntPtr U, IntPtr buffer, int sz);

        [DllImport("rudp", EntryPoint = "aux_update", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void aux_update(IntPtr U, IntPtr buffer, int sz, int tick);
    }
}
