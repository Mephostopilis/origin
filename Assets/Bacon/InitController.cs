using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon
{
    class InitController : Controller
    {
        private float _handshakecd = 5f;

        public InitController(Context ctx) : base(ctx)
        {
        }

        internal override void Update(float delta)
        {
            base.Update(delta);
            Handshake(delta);
        }

        public void Handshake(float delta)
        {
            if (_authtcp)
            {
                if (_handshakecd > 0)
                {
                    _handshakecd -= delta;
                    if (_handshakecd <= 0)
                    {
                        _handshakecd = 5f;
                        _ctx.SendReq<C2sProtocol.handshake>("handshake", null);
                    }
                }
            }
        }
    }
}
