using Maria;
using Maria.Network;
using Sproto;
using UnityEngine;
using System;

namespace Bacon {
    class InitController : Controller {
        private float _handshakecd = 5f;
        private long _last = 0;
        private int _lag;
        private SMActor _smactor = null;
        private TimeSync _ts = new TimeSync();

        public InitController(Context ctx) : base(ctx) {
            _smactor = new SMActor(_ctx, this);
        }

        public override void Update(float delta) {
            base.Update(delta);
            SendHandshake(delta);
        }

        public SMActor SMActor { get { return _smactor; } }

        public int Ping { get { return _lag; } }

        public object DataTime { get; private set; }

        public void SendHandshake(float delta) {
            if (_authtcp) {
                if (_handshakecd > 0) {
                    _handshakecd -= delta;
                    if (_handshakecd <= 0) {
                        _handshakecd = 5f;
                        if (_authtcp) {
                            _ctx.SendReq<C2sProtocol.handshake>(C2sProtocol.handshake.Tag, null);
                        }
                        _last = _ts.GetTimeMs();
                    }
                }
            }
        }

        public void Handshake(SprotoTypeBase responseObj) {
            C2sSprotoType.handshake.response o = responseObj as C2sSprotoType.handshake.response;
            Debug.Log(string.Format("handshake {0}", o.errorcode));
            int lag = (int)(_ts.GetTimeMs() - _last); // ms
            _lag = lag;
        }
    }
}
