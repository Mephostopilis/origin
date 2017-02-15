using Maria;
using Maria.Network;
using Sproto;
using UnityEngine;
using System;

namespace Bacon {
    class InitService : Service {

        public static readonly string Name = "init";

        private bool _authed = false;
        private float _handshakecd = 5f;
        private long _last = 0;
        private int _lag;
        private SMActor _smactor = null;
        private TimeSync _ts = null;

        public InitService(Context ctx) : base(ctx) {
            _ts = ctx.TiSync;
            _smactor = new SMActor(ctx, this);

            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnDisconnected, OnDiconnected, null);
            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnAuthed, OnAuthed, null);
        }

        public override void Update(float delta) {
            base.Update(delta);
            SendHandshake(delta);
        }

        public SMActor SMActor { get { return _smactor; } }

        public int Ping { get { return _lag; } }

        public object DataTime { get; private set; }

        public void SendHandshake(float delta) {
            if (!_authed) {
                return;
            }
            if (_handshakecd > 0) {
                _handshakecd -= delta;
                if (_handshakecd <= 0) {
                    _handshakecd = 5f;
                    _ctx.SendReq<C2sProtocol.handshake>(C2sProtocol.handshake.Tag, null);
                    _last = _ts.GetTimeMs();
                }
            }
        }

        public void Handshake(SprotoTypeBase responseObj) {
            C2sSprotoType.handshake.response o = responseObj as C2sSprotoType.handshake.response;
            Debug.Log(string.Format("handshake {0}", o.errorcode));
            int lag = (int)(_ts.GetTimeMs() - _last); // ms
            _lag = lag;
        }

        private void OnAuthed(EventCustom e) {
            _authed = true;
        }

        private void OnDiconnected(EventCustom e) {
            _authed = false;
            _ctx.GateAuth();
        }
    }
}
