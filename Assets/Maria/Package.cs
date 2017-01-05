using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Maria {
    public class Package { 
        [StructLayout(LayoutKind.Sequential)]
        public struct package {
            public IntPtr buffer;
            public int size;
            public int cap;
        }

        public static void init(package self, int cap) {
            self.buffer = Marshal.AllocHGlobal(cap);
            self.size = 0;
            self.cap = cap;
        }

        public static void exit(package self) {
            Marshal.FreeHGlobal(self.buffer);
        }

        public static void clear(package self) {
            self.size = 0;
        }

        public static int write(package self, byte[] buffer) {
            if (buffer.Length > (self.cap - self.size)) {
                return 0;
            }
            Debug.Assert(self.size == 0);
            Marshal.Copy(buffer, 0, self.buffer, buffer.Length);
            self.size = buffer.Length;
            return 1;
        }

        public static int write(package self, byte[] buffer, int start, int len) {
            if (len > (self.cap - self.size)) {
                return 0;
            }
            Debug.Assert(self.size == 0);
            Marshal.Copy(buffer, start, self.buffer, len);
            self.size = buffer.Length;
            return 1;
        }
    }
}
