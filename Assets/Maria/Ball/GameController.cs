using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria.Ball
{
    class GameController : Controller
    {
        private float _v = 1;
        private float _cd = 1;

        public GameController(Context ctx) : base(ctx)
        {
        }

        internal override void Update(float delta)
        {
            base.Update(delta);
            if (_cd > 0)
            {
                _cd -= delta;
                if (_cd <= 0)
                {
                    AppContext ctx = _ctx as AppContext;
                    int[] e = ctx.TiSync.GlobalTime();
                    Debug.Log(string.Format("globaltime: {0}", e[1]));
                    string c = "hello word.";
                    byte[] data = Encoding.ASCII.GetBytes(c);
                    _ctx.SendUdp(data);
                    _cd = _v;
                }
            }

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
