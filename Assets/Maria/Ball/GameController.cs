using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Ball
{
    class GameController : Controller
    { 
        public GameController(Context ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            base.Enter();
            LoadScene("game");
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
