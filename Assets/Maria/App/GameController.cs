using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Maria.App
{
    class GameController : Controller
    {
        private GameObject _scene = null;

        public GameController(Context ctx) : base(ctx)
        {
        }

        // 进入房间
        public void Enter()
        {
            _scene = GameObject.Find("Root");
        }


        public void Exit()
        {

        }

        public void Shullle()
        {

        }

        public override void Run()
        {
            base.Run();
            _ctx.Push("game");
        }
    }
}
