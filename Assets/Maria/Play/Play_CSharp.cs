using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Maria.Play
{
    class Play_CSharp
    {
        [DllImport("play", EntryPoint = "play_alloc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr play_alloc();

        [DllImport("play", EntryPoint = "play_free", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void play_free(IntPtr self);

        [DllImport("play", EntryPoint = "play_update", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void play_update(IntPtr self);

        [DllImport("play", EntryPoint = "play_join", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern int  play_join(IntPtr self, IntPtr ud);

        [DllImport("play", EntryPoint = "play_leave", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern void play_leave(IntPtr self, int id);
    }
}
