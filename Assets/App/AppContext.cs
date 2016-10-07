using Maria.Network;

namespace Maria.Ball
{
    public class AppContext : Context
    {
        private float _cd = 2;
        private float _v = 2;

        public AppContext(global::App app)
            : base(app)
        {
            _hash["game"] = new GameController(this);

            TiSync = new TimeSync();
            Config = new AppConfig();
        }

        public TimeSync TiSync { get; set; }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (_auth)
            {
                if (_cd > 0)
                {
                    _cd -= delta;
                    if (_cd <= 0)
                    {
                        SendReq<C2sProtocol.handshake>("handshake", null);
                    }
                }
            }
        }

        public Config Config { get; set; }
    }
}
