using Maria;
using Maria.Network;
using Sproto;
using UnityEngine;
using System;
using XLua;
using Bacon.Helper;
using Bacon.Event;

namespace Bacon.Service {

    [LuaCallCSharp]
    class InitService : Maria.Service {

        public static readonly string Name = "init";

        private bool _authed = false;
        private float _handshakecd = 5f;
        private long _last = 0;
        private SMActor _smactor = null;
        private TimeSync _ts = null;

        public InitService(Context ctx) : base(ctx) {
            _ts = ctx.TiSync;
            _smactor = new SMActor(ctx, this);

            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnGateAuthed, OnEventAuthed, null);
            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnGateDisconnected, OnEventDiconnected, null);
            _ctx.EventDispatcher.AddCustomEventListener(MyEventCustom.LOGOUT, OnEventLogout, null);
        }

        public override void Update(float delta) {
            base.Update(delta);
            SendHandshake(delta);
        }

        public SMActor SMActor { get { return _smactor; } }

        public object DataTime { get; private set; }
    
        public void SendHandshake(float delta) {
            
            if (!_authed) {
                return;
            }

            if (_handshakecd > 0) {
                _handshakecd -= delta;
                if (_handshakecd <= 0) {
                    _handshakecd = 5f;
                    _last = _ts.GetTimeMs();
                    _ctx.SendReq<C2sProtocol.handshake>(C2sProtocol.handshake.Tag, null);
                }
            }
        }

        public void OnRspHandshake(SprotoTypeBase responseObj) {
            C2sSprotoType.handshake.response o = responseObj as C2sSprotoType.handshake.response;

            long lag = _ts.GetTimeMs() - _last; // ms
            UnityEngine.Debug.LogFormat("handshake code {0}, tts: {1} ms", o.errorcode, lag);
        }

        private void OnEventAuthed(EventCustom e) {
            _authed = true;
        }

        private void OnEventDiconnected(EventCustom e) {
            _authed = false;
            if (_ctx.Logined) {
                _ctx.GateAuth();
            }
        }

        private void OnEventLogout(EventCustom e) {
            _authed = false;
        }

        //public SprotoTypeBase OnReqRadio(SprotoTypeBase requestObj) {
        //    S2cSprotoType.radio.request obj = requestObj as S2cSprotoType.radio.request;
          

        //    S2cSprotoType.radio.response responseObj = new S2cSprotoType.radio.response();
        //    responseObj.errorcode = Errorcode.SUCCESS;
        //    return responseObj;
        //}

    }
}
