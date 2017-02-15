using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Maria.Play {
    public class Play : DisposeObject {

        public delegate void FetchCB(IntPtr ptr, int len);

        private SharpC _sharpc = null;
        private IntPtr _play = IntPtr.Zero;

        public Play(SharpC sharpc ) {
            _sharpc = sharpc;
            SharpC.CSObject[] args = new SharpC.CSObject[2];
            args[0] = _sharpc.CacheObj(this);
            args[1] = _sharpc.CacheFunc(fetch);
            _play = Play_CSharp.play_alloc(args[0], args[1]);
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) return;
            if (disposing) {
                // release managed
            }
            // release unmanaged
            Play_CSharp.play_free(_play);
            _disposed = true;
        }

        public FetchCB OnFetch { get; set; }

        public void start() {
            Play_CSharp.play_start(_play);
        }

        
        public void close(IntPtr self) {
            Play_CSharp.play_close(_play);
        }

        public void kill(IntPtr self) {
            Play_CSharp.play_kill(_play);
        }

        public void update(float delta) {
            SharpC.CSObject cso = new SharpC.CSObject();
            cso.type = SharpC.CSType.REAL;
            cso.f = delta;
            Play_CSharp.play_update(_play, cso);
        }

        public int join(int uid, int sid, int session) {
            //GCHandle handle = GCHandle.Alloc(ud);
            //IntPtr ptr = GCHandle.ToIntPtr(handle);

            //SharpC.CSObject[] args = new SharpC.CSObject[3];
            //args[0].type = SharpC.CSType.INT32;
            //args[0].v32 = uid;

            //args[1].type = SharpC.CSType.INT32;
            //args[1].v32 = sid;

            //args[2].type = SharpC.CSType.INT32;
            //args[2].v32 = session;

            //return Play_CSharp.play_join(_play, args[0], args[1], args[2]);

            SharpC.CSObject x1 = new SharpC.CSObject();
            x1.type = SharpC.CSType.INT32;
            x1.v32 = uid;

            SharpC.CSObject x2 = new SharpC.CSObject();
            x2.type = SharpC.CSType.INT32;
            x2.v32 = sid;

            SharpC.CSObject x3 = new SharpC.CSObject();
            x3.type = SharpC.CSType.INT32;
            x3.v32 = session;

            return Play_CSharp.play_join(_play, x1, x2, x3);
        }

        public void leave(int uid, int sid, int session) {
            SharpC.CSObject[] args = new SharpC.CSObject[3];
            args[0].type = SharpC.CSType.INT32;
            args[0].v32 = uid;

            args[1].type = SharpC.CSType.INT32;
            args[1].v32 = sid;

            args[2].type = SharpC.CSType.INT32;
            args[2].v32 = session;

            Play_CSharp.play_leave(_play, args[0], args[1], args[2]);
        }

        public int xfetch(ref IntPtr ptr, ref int len) {

            SharpC.CSObject[] args = new SharpC.CSObject[2];
            args[0].type = SharpC.CSType.INTPTR;
            args[0].ptr = ptr;

            args[1].type = SharpC.CSType.INT32;
            args[1].v32 = len;

            return Play_CSharp.play_fetch(_play, args[0], args[1]);
        }

        public static int fetch(int argc, [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] SharpC.CSObject[] argv, int args, int res) {
            UnityEngine.Debug.Assert(argc >= 3);
            UnityEngine.Debug.Assert(argv[1].type == SharpC.CSType.SHARPOBJECT);

            Play p =  SharpC.cache.Get(argv[1].v32) as Play;
            if (p != null) {
                IntPtr ptr = argv[2].ptr;
                int len = argv[3].v32;

                if (p.OnFetch != null) {
                    p.OnFetch(ptr, len);
                }
            }

            return 0;
        }
    }
}
