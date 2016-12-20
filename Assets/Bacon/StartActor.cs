using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class StartActor : Actor {

        public StartActor(Context ctx, Controller controller) : base(ctx, controller) {
            //EventListenerCmd listener1 = new EventListenerCmd(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, SetupStartRoot);
            //_ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void SetupStartRoot(EventCmd e) {
            _go = e.Orgin;
            _ctx.Countdown("startcontroller", 2, CountdownCb);

            //byte[] buffer1 = new byte[4] { 1, 2, 3, 4 };
            //byte[] buffer2 = new byte[4] { 5, 6, 7, 8 };

            //Rudp.Rudp S = new Rudp.Rudp(1, 5);
            //S.OnRecv = OnRecv;

            //Rudp.Rudp C = new Rudp.Rudp(1, 5);
            //C.Send(buffer1);
            //C.Send(buffer2);

            //List<byte[]> res = C.Update(null, 0, 0, 1);
            //foreach (var item in res) {
            //    S.Update(item, 0, 0, 1);
            //    S.Recv();
            //}

        }

        private void CountdownCb() {
            _ctx.Push("login");
        }

        private void OnRecv(byte[] buffer, int start, int len) {
            for (int i = 0; i < len; i++) {
                Debug.Log(string.Format("{0}", buffer[i]));
            }
        }
    }
}
