using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    class UIRootActor : Maria.Actor {

        private int _ping = 0;

        public UIRootActor(Context ctx, Controller controller) : base(ctx, controller) {

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_UIROOT, SetupUIRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_ONBORN, OnBorn);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_ONJOIN, OnJoin);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);
        }

        public override void Update(float delta) {
            base.Update(delta);
            Ping();
        }

        public void SetupUIRoot(EventCmd e) {
            _go = e.Orgin;
        }

        public void OnBorn(EventCmd e) {
            //_ctx.SendReq<C2sProtocol.born>(C2sProtocol.born.Tag, null);
        }

        public void OnJoin(EventCmd e) {
            //_ctx.UdpAuth(null);
        }

        public void Ping() {
            //_ping = _init.Ping;
            _ctx.EnqueueRenderQueue(RenderPing);
        }

        public void RenderPing() {
            if (_go) {
                var com = _go.GetComponent<UIRootBehaviour>();
                com.ShowPing(_ping);
            }
        }
    }
}
