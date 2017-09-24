using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Maria.HexMap {
    public class HexMap_CSharp {

        public const string DLL = "play";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 hexmap_create_from_plist(IntPtr src, Int32 len);

       
    }
}
