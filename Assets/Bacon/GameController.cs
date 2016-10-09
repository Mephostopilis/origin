using Maria;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class GameController : Controller
    {

        private float _synccd = 1;
        private bool _udpflag = false;
        private MyBall _myball = null;
        private Map _map = new Map();

        public GameController(Context ctx) : base(ctx)
        {
        }

        internal override void Update(float delta)
        {
            base.Update(delta);
            Sync(delta);
        }

        public void Sync(float delta)
        {
            if (_udpflag)
            {
                if (_synccd > 0)
                {
                    _synccd -= delta;
                    if (_synccd <= 0)
                    {
                        _synccd = 1f;
                        AppContext ctx = _ctx as AppContext;
                        int[] e = ctx.TiSync.GlobalTime();
                        Debug.Log(string.Format("globaltime: {0}", e[1]));
                        string c = "hello word.";
                        byte[] data = Encoding.ASCII.GetBytes(c);
                        _ctx.SendUdp(data);
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
                // 创建一个球
                //SendReq<C2sProtocol.born>("born", null);
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

        public void Born(float radis, Vector3 position, Vector3 direction)
        {
            _myball = _map.Born(radis, position, direction);
        }
    }
}
