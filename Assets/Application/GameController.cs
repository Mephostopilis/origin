using Maria;
using System.Text;
using UnityEngine;

namespace Application
{
    class GameController : Controller
    {
        private float _v = 1;
        private float _cd = 1;
        private bool _udpflag = false;

        public GameController(Context ctx) : base(ctx)
        {
        }

        internal override void Update(float delta)
        {
            base.Update(delta);
            if (_udpflag)
            {
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
        }

        public override void Enter()
        {
            base.Enter();
            LoadScene("game");
            _ctx.AuthUdp(AuthUdpCb);
        }

        public void AuthUdpCb(int code)
        {
            if (code == 200)
            {
                _udpflag = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Run()
        {
            base.Run();
            _ctx.Push("game");
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
            //_ctx.AuthGate(AuthGateCb);
        }

        public void AuthGateCb(int code)
        {
            if (code == 200)
            {

            }
        }
    }
}
