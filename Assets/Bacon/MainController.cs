using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    class MainController : Controller {
        public MainController(Context ctx) : base(ctx) {
            _name = "main";
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MATCH, OnMatch);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public override void Update(float delta) {
            base.Update(delta);
        }

        public override void Enter() {
            base.Enter();
            InitService service = (InitService)_ctx.QueryService(InitService.Name);
            SMActor actor = service.SMActor;
            actor.LoadScene(_name);
        }

        public void OnMatch(EventCmd e) {
        }
    }
}
