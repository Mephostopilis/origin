using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class GameController : Controller {
        
        private UIRootActor _ui = null;

        private float _synccd1 = 0.03f;
        private byte[] _syncmsg1 = null;

        private Player _player = null;
        private Map _map = null;
        private View _view = null;
        private Scene _scene = null;

        private bool _moveflag = false;

        public GameController(Context ctx) : base(ctx) {

            _ui = new UIRootActor(_ctx, this);

            // 4 + 12:pos + 12:dir
            _syncmsg1 = new byte[28];
            NetPack.Packli(_syncmsg1, 0, 1);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_SCENE, SetupScene);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MAP, SetupMap);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_VIEW, SetupCamera);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_PRESSDOWN, OnPressDown);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_PRESSUP, OnPressUp);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_PRESSLEFT, OnPressLeft);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_PRESSRIGHT, OnPressRight);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);

        }

        internal override void Update(float delta) {
            base.Update(delta);
            if (_scene != null) {
                _scene.Update(delta);
            }
            Sync1(delta);
        }

        public void Sync1(float delta) {
            if (_authudp) {
                if (_synccd1 > 0) {
                    _synccd1 -= delta;
                    if (_synccd1 <= 0) {
                        _synccd1 = 0.03f;
                        int[] e = _ctx.TiSync.GlobalTime();
                        Debug.Log(string.Format("globaltime: {0}", e[0]));

                        //byte[] buf = _player.PackBall();
                        byte[] buf = new byte[4];
                        NetPack.Packli(buf, 0, 1);
                        _ctx.SendUdp(buf);
                    }
                }
            }
        }

        public override void Enter() {
            base.Enter();
            SMActor actor = ((AppContext)_ctx).SMActor;
            actor.LoadScene("game");
        }

        public override void Exit() {
            base.Exit();
        }

        public override void AuthGateCB(int code) {
            base.AuthGateCB(code);
        }

        public override void OnDisconnect() {
            base.OnDisconnect();
        }

        public override void AuthUdpCb(bool ok) {
            base.AuthUdpCb(ok);
        }

        // 初始游戏场景
        private Ball SetupBall(long ballid, uint uid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel) {
            Ball ball = _scene.SetupBall(ballid, uid, session, radis, length, width, height, position, dir, vel);
            return ball;
        }

        public void SetupCamera(EventCmd e) {
            // 无论_camera == null,新场景启动都要重置
            GameObject go = e.Orgin;
            Debug.Assert(_scene != null);
            _view = _scene.SetupView(go);
        }

        public void SetupMap(EventCmd e) {
            GameObject map = e.Orgin;
            Debug.Assert(_scene != null);
            _map = _scene.SetupMap(map);
        }

        public void SetupScene(EventCmd e) {
            GameObject word = e.Orgin;
            _scene = new Scene(_ctx, this, word);
        }

        public void OnMoveStart() {
            _moveflag = true;
        }

        public void OnMove(Vector2 d) {
            if (_moveflag) {
                Debug.Log(string.Format("move: x {0}, y {1}", d.x, d.y));
                _moveflag = false;

                ////Vector2 dir = d.normalized;
                //Vector2 dir = d;
                //float speed = 10f;
                //Vector2 shift = dir * speed;
                //_myball.MoveBy(shift);
            }
        }

        public void OnMoveSpeed(Vector2 s) {
            if (_moveflag) {
                Debug.Log(string.Format("speed: x {0}, y {1}", s.x, s.y));
            }
        }

        public void OnPressUp(EventCmd e) {
            //Vector3 dir = new Vector3(0, 0, 1);
            //_player.ChangeDir(dir);
            try {
                C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
                obj.code = (long)(OpCodes.OPCODE_PRESSUP);
                _ctx.SendReq<C2sProtocol.opcode>("opcode", obj);
            } catch (KeyNotFoundException ex) {
                Debug.LogError(ex.Message);
            }
        }

        public void OnPressRight(EventCmd e) {
            //Vector3 dir = new Vector3(1, 0, 0);
            //_player.ChangeDir(dir);
            try {
                C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
                obj.code = OpCodes.OPCODE_PRESSRIGHT;
                _ctx.SendReq<C2sProtocol.opcode>("opcode", obj);
            } catch (KeyNotFoundException ex) {
                Debug.LogError(ex.Message);
            }
        }

        public void OnPressDown(EventCmd e) {
            //Vector3 dir = new Vector3(0, -1, 0);
            //_player.ChangeDir(dir);
            try {
                C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
                obj.code = OpCodes.OPCODE_PRESSDOWN;
                _ctx.SendReq<C2sProtocol.opcode>("opcode", obj);
            } catch (KeyNotFoundException ex) {
                Debug.LogError(ex.Message);
            }
        }

        public void OnPressLeft(EventCmd e) {
            //Vector3 dir = new Vector3(-1, 0, 0);
            //_player.ChangeDir(dir);
            try {
                C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
                obj.code = OpCodes.OPCODE_PRESSLEFT;
                _ctx.SendReq<C2sProtocol.opcode>("opcode", obj);
            } catch (KeyNotFoundException ex) {
                Debug.LogError(ex.Message);
            }
        }

        // 游戏协议
        // 主要是同步场景中已经加入的其他玩家
        public void Join(SprotoTypeBase responseObj) {
            if (responseObj != null) {
                C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;
                string host = o.host;
                int port = (int)o.port;
                _ctx.AuthUdpCb(o.session, host, port);

                if (_player == null) {
                    _player = new Player();
                }
                _player.Session = (uint)o.session;

                var all = o.all;
                foreach (var item in all) {
                    uint uid = (uint)item.uid;
                    uint session = (uint)item.session;
                    long ballid = item.ballid;
                    float radis = item.radis;
                    float length = item.length;
                    float width = item.width;
                    float height = item.height;
                    float pos_x = (float)BitConverter.Int64BitsToDouble(item.px);
                    float pos_y = (float)BitConverter.Int64BitsToDouble(item.py);
                    float pos_z = (float)BitConverter.Int64BitsToDouble(item.pz);
                    Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
                    float dir_x = (float)BitConverter.Int64BitsToDouble(item.dx);
                    float dir_y = (float)BitConverter.Int64BitsToDouble(item.dy);
                    float dir_z = (float)BitConverter.Int64BitsToDouble(item.dz);
                    Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
                    float vel = (float)BitConverter.Int64BitsToDouble(item.vel);
                    if (session == (uint)_ctx.Session) {
                        var ball = SetupBall(ballid, uid, session, radis, length, width, height, pos, dir, vel);
                        _player.Add(ball as MyBall);
                    } else {
                        SetupBall(ballid, uid, session, radis, length, width, height, pos, dir, vel);
                    }
                }
            }
        }

        public void Born(SprotoTypeBase responseObj) {
            C2sSprotoType.born.response obj = responseObj as C2sSprotoType.born.response;
            if (obj != null && obj.errorcode == Errorcode.SUCCESS) {
                var item = obj.b;
                uint uid = (uint)item.uid;
                uint session = (uint)item.session;
                long ballid = item.ballid;
                float radis = item.radis;
                float length = item.length;
                float width = item.width;
                float height = item.height;
                float pos_x = (float)BitConverter.Int64BitsToDouble(item.px);
                float pos_y = (float)BitConverter.Int64BitsToDouble(item.py);
                float pos_z = (float)BitConverter.Int64BitsToDouble(item.pz);
                Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
                float dir_x = (float)BitConverter.Int64BitsToDouble(item.dx);
                float dir_y = (float)BitConverter.Int64BitsToDouble(item.dy);
                float dir_z = (float)BitConverter.Int64BitsToDouble(item.dz);
                Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
                float vel = (float)BitConverter.Int64BitsToDouble(item.vel);
                if (session == (uint)_player.Session) {
                    var myball = SetupBall(ballid, uid, session, radis, length, width, height, pos, dir, vel);
                    _player.Add(myball as MyBall);
                    _view.MoveTo(new Vector2(pos_x, pos_z));
                    //Vector3 pivot = _player.GetPivot();
                    //if (_view != null) {
                    //    _view.InitView(pivot);
                    //}
                } else {
                    Debug.Assert(false);
                    SetupBall(ballid, uid, session, radis, length, width, height, pos, dir, vel);
                }
            }
        }

        public SprotoTypeBase OnBorn(SprotoTypeBase requestObj) {
            S2cSprotoType.born.request obj = requestObj as S2cSprotoType.born.request;
            if (obj != null) {
                S2cSprotoType.ball b = obj.b;
                uint uid = (uint)b.uid;
                uint session = (uint)b.session;
                long ballid = b.ballid;
                float radis = b.radis;
                float length = b.length;
                float width = b.width;
                float height = b.height;
                float pos_x = (float)BitConverter.Int64BitsToDouble(b.px);
                float pos_y = (float)BitConverter.Int64BitsToDouble(b.py);
                float pos_z = (float)BitConverter.Int64BitsToDouble(b.pz);
                Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
                float dir_x = (float)BitConverter.Int64BitsToDouble(b.dx);
                float dir_y = (float)BitConverter.Int64BitsToDouble(b.dy);
                float dir_z = (float)BitConverter.Int64BitsToDouble(b.dz);
                Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
                float vel = (float)BitConverter.Int64BitsToDouble(b.vel);
                SetupBall(ballid, uid, session, radis, length, width, height, pos, dir, vel);

                S2cSprotoType.born.response responseObj = new S2cSprotoType.born.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } else {
                throw new System.Exception("request obj is null");
            }
        }

        public void Leave(SprotoTypeBase responseObj) {
        }

        public SprotoTypeBase OnLeave(SprotoTypeBase requestObj) {
            S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            if (obj != null) {
                for (int i = 0; i < obj.ballid.Count; i++) {
                    _scene.Leave(obj.ballid[i]);
                }
                if (obj.session == _player.Session) {
                }

                S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } else {
                throw new System.Exception("obj is null");
            }
        }

        public void OpCode(SprotoTypeBase responseObj) {

        }

        public override void OnRecviveUdp(PackageSocketUdp.R r) {
            base.OnRecviveUdp(r);
            if (r.Protocol == 1) {
                Debug.Log(string.Format("{0}, {1}", r.Session, r.Protocol));
                Debug.Assert(_player != null);

                if (_player.Session != r.Session) {
                    int sz = NetUnpack.Unpackli(r.Data, 0);
                    for (int i = 0; i < sz; i++) {
                        long ballid = NetUnpack.Unpackll(r.Data, 4 + (i * 32) + 0);
                        float px = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 8);
                        float py = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 12);
                        float pz = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 16);
                        float dx = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 20);
                        float dy = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 24);
                        float dz = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 28);
                        _scene.UpdateBall(ballid, new Vector3(px, py, pz), new Vector3(dx, dy, dz));
                    }
                    //uint ok = NetUnpack.UnpacklI(r.Data, 32);

                } else {
                    int sz = NetUnpack.Unpackli(r.Data, 0);
                    for (int i = 0; i < sz; i++) {
                        long ballid = NetUnpack.Unpackll(r.Data, 4 + (i * 32) + 0);
                        float px = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 8);
                        float py = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 12);
                        float pz = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 16);
                        float dx = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 20);
                        float dy = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 24);
                        float dz = NetUnpack.Unpacklf(r.Data, 4 + (i * 32) + 28);
                        _scene.UpdateBall(ballid, new Vector3(px, py, pz), new Vector3(dx, dy, dz));
                    }
                    Vector3 pivot = _player.GetPivot();
                    _view.MoveTo(new Vector2(pivot.x, pivot.z));
                }
            }
        }
    }
}
