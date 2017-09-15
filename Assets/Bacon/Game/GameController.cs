using Bacon.Helper;
using Bacon.Service;
using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon.Game {
    class GameController : Controller {

        //private UIRootActor _ui = null;

        private float _synccd1 = 0.03f;
        private byte[] _syncmsg1 = null;

        private long _mysession = 0;
        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();

        private Map _map = null;
        private View _view = null;
        private Scene _scene = null;

        private bool _moveflag = false;
        private int _lastK = 0;

        public GameController(Context ctx) : base(ctx) {

            //_ui = new UIRootActor(_ctx, this);

            // 4, protocol
            _syncmsg1 = new byte[4];
            NetPack.Packli(_syncmsg1, 0, 1);

            //EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_SCENE, SetupScene);
            //_ctx.EventDispatcher.AddCmdEventListener(listener1);

            //EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MAP, SetupMap);
            //_ctx.EventDispatcher.AddCmdEventListener(listener2);

            //EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_VIEW, SetupCamera);
            //_ctx.EventDispatcher.AddCmdEventListener(listener3);
        }

        //public override void Update(float delta) {
        //    base.Update(delta);
        //    if (_scene != null) {
        //        _scene.Update(delta);
        //    }
        //    Sync1(delta);
        //}

        //public override void OnEnter() {
        //    base.OnEnter();
        //    InitService service = (InitService)_ctx.QueryService("init");
        //    if (service != null) {
        //        SMActor actor = service.SMActor;
        //        actor.LoadScene("game");
        //    }
        //}


        //public void Sync1(float delta) {
        //    if (_authudp) {
        //        if (_synccd1 > 0) {
        //            _synccd1 -= delta;
        //            if (_synccd1 <= 0) {
        //                _synccd1 = 0.03f;
        //                int[] e = _ctx.TiSync.GlobalTime();
        //                UnityEngine.Debug.Log(string.Format("globaltime: {0}", e[0]));

        //                _ctx.SendUdp(_syncmsg1);
        //            }
        //        }
        //    }
        //}

        //// 初始游戏场景
        //private Ball SetupBall(long ballid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel) {
        //    Ball ball = _scene.SetupBall(ballid, session, radis, length, width, height, position, dir, vel);
        //    return ball;
        //}

        //public void SetupCamera(EventCmd e) {
        //    // 无论_camera == null,新场景启动都要重置
        //    GameObject go = e.Orgin;
        //    UnityEngine.Debug.Assert(_scene != null);
        //    _view = _scene.SetupView(go);
        //}

        //public void SetupMap(EventCmd e) {
        //    GameObject map = e.Orgin;
        //    UnityEngine.Debug.Assert(_scene != null);
        //    _map = _scene.SetupMap(map);
        //}

        //public void SetupScene(EventCmd e) {
        //    GameObject word = e.Orgin;
        //    _scene = new Scene(_ctx, this, word);
        //}

        public override void OnEnter() {
            base.OnEnter();
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            service.SMActor.LoadScene("world");
        }

        public void OnMoveStart() {
            _moveflag = true;
        }

        public void OnMove(Vector2 d) {
            if (_moveflag) {
                UnityEngine.Debug.Log(string.Format("move: x {0}, y {1}", d.x, d.y));
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
                UnityEngine.Debug.Log(string.Format("speed: x {0}, y {1}", s.x, s.y));
            }
        }

       

        // 游戏协议
        // 主要是同步场景中已经加入的其他玩家
        public void Join(SprotoTypeBase responseObj) {
            //if (responseObj != null) {
            //    C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;
            //    _mysession = o.session;
            //    //string host = o.host;
            //    //int port = (int)o.port;

            //    if (_playes.ContainsKey(_mysession)) {
            //    } else {
            //        Player player = new Player((uint)_mysession);
            //        _playes[_mysession] = player;
            //    }

            //    foreach (var item in o.players) {
            //        Player player = null;
            //        if (_playes.ContainsKey(item.session)) {
            //            player = _playes[item.session];
            //        } else {
            //            player = new Player((uint)item.session);
            //            _playes[item.session] = player;
            //        }
            //        if (player != null) {
            //            foreach (var b in item.balls) {
            //                uint session = (uint)b.session;
            //                long ballid = b.ballid;
            //                float radis = b.radis;
            //                float length = b.length;
            //                float width = b.width;
            //                float height = b.height;
            //                float pos_x = (float)BitConverter.Int64BitsToDouble(b.px);
            //                float pos_y = (float)BitConverter.Int64BitsToDouble(b.py);
            //                float pos_z = (float)BitConverter.Int64BitsToDouble(b.pz);
            //                Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
            //                float dir_x = (float)BitConverter.Int64BitsToDouble(b.dx);
            //                float dir_y = (float)BitConverter.Int64BitsToDouble(b.dy);
            //                float dir_z = (float)BitConverter.Int64BitsToDouble(b.dz);
            //                Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
            //                float vel = (float)BitConverter.Int64BitsToDouble(b.vel);
            //                var ball = SetupBall(ballid, session, radis, length, width, height, pos, dir, vel);
            //                player.Add(ball);
            //            }
            //        }
            //    }
            //}
        }

        public SprotoTypeBase OnJoin(SprotoTypeBase requestObj) {
            //S2cSprotoType.join.request obj = requestObj as S2cSprotoType.join.request;
            //if (obj != null) {
            //    if (_playes.ContainsKey(obj.session)) {
            //    } else {
            //        Player player = new Player((uint)obj.session);
            //        _playes[obj.session] = player;
            //    }
            //}
            //S2cSprotoType.join.response responseObj = new S2cSprotoType.join.response();
            //responseObj.errorcode = Errorcode.SUCCESS;
            //return responseObj;
            return null;
        }

        public void Born(SprotoTypeBase responseObj) {
            //C2sSprotoType.born.response obj = responseObj as C2sSprotoType.born.response;
            //if (obj != null && obj.errorcode == Errorcode.SUCCESS) {
            //    var item = obj.b;
            //    uint session = (uint)item.session;
            //    long ballid = item.ballid;
            //    float radis = item.radis;
            //    float length = item.length;
            //    float width = item.width;
            //    float height = item.height;
            //    float pos_x = (float)BitConverter.Int64BitsToDouble(item.px);
            //    float pos_y = (float)BitConverter.Int64BitsToDouble(item.py);
            //    float pos_z = (float)BitConverter.Int64BitsToDouble(item.pz);
            //    Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
            //    float dir_x = (float)BitConverter.Int64BitsToDouble(item.dx);
            //    float dir_y = (float)BitConverter.Int64BitsToDouble(item.dy);
            //    float dir_z = (float)BitConverter.Int64BitsToDouble(item.dz);
            //    Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
            //    float vel = (float)BitConverter.Int64BitsToDouble(item.vel);
            //    var ball = SetupBall(ballid, session, radis, length, width, height, pos, dir, vel);
            //    var player = _playes[session];
            //    player.Add(ball);
            //    if (session == _mysession) {
            //        _view.MoveTo(new Vector2(pos_x, pos_z));
            //    }
            //}
        }

        public SprotoTypeBase OnBorn(SprotoTypeBase requestObj) {
            //S2cSprotoType.born.request obj = requestObj as S2cSprotoType.born.request;
            //if (obj != null) {
            //    foreach (var b in obj.bs) {
            //        uint session = (uint)b.session;
            //        long ballid = b.ballid;
            //        float radis = b.radis;
            //        float length = b.length;
            //        float width = b.width;
            //        float height = b.height;
            //        float pos_x = (float)BitConverter.Int64BitsToDouble(b.px);
            //        float pos_y = (float)BitConverter.Int64BitsToDouble(b.py);
            //        float pos_z = (float)BitConverter.Int64BitsToDouble(b.pz);
            //        Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
            //        float dir_x = (float)BitConverter.Int64BitsToDouble(b.dx);
            //        float dir_y = (float)BitConverter.Int64BitsToDouble(b.dy);
            //        float dir_z = (float)BitConverter.Int64BitsToDouble(b.dz);
            //        Vector3 dir = new Vector3(dir_x, dir_y, dir_z);
            //        float vel = (float)BitConverter.Int64BitsToDouble(b.vel);
            //        var ball = SetupBall(ballid, session, radis, length, width, height, pos, dir, vel);
            //        var player = _playes[session];
            //        player.Add(ball);
            //    }

            //    S2cSprotoType.born.response responseObj = new S2cSprotoType.born.response();
            //    responseObj.errorcode = Errorcode.SUCCESS;
            //    return responseObj;
            //} else {
            //    throw new System.Exception("request obj is null");
            //}
            return null;
        }

        public void Leave(SprotoTypeBase responseObj) {
            //C2sSprotoType.leave.response obj = responseObj as C2sSprotoType.leave.response;
            //if (obj != null) {
            //    if (obj.errorcode == Errorcode.SUCCESS) {
            //        var player = _playes[_mysession];
            //        var balls = player.GetBallids();
            //        foreach (var item in balls) {
            //            _scene.Leave(item);
            //        }
            //        player.Clear();
            //    }
            //}
        }

        public SprotoTypeBase OnLeave(SprotoTypeBase requestObj) {
            //S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            //if (obj != null) {
            //    for (int i = 0; i < obj.ballid.Count; i++) {
            //        _scene.Leave(obj.ballid[i]);
            //    }
            //    S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            //    responseObj.errorcode = Errorcode.SUCCESS;
            //    return responseObj;
            //} else {
            //    throw new System.Exception("obj is null");
            //}
            return null;
        }

        public void OpCode(SprotoTypeBase responseObj) {

        }

        public SprotoTypeBase OnDie(SprotoTypeBase requestObj) {
            //S2cSprotoType.die.request obj = requestObj as S2cSprotoType.die.request;
            //try {
            //    var ball =_scene.Leave(obj.ballid);
            //    Player player = _playes[obj.session];
            //    player.Remove(ball);

            //    S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            //    responseObj.errorcode = Errorcode.SUCCESS;
            //    return responseObj;
            //} catch (KeyNotFoundException ex) {
            //    Debug.LogError(ex.Message);
            //    throw;
            //}
            return null;
        }

        public override void OnUdpRecv(PackageSocketUdp.R r) {
            base.OnUdpRecv(r);
            //int protocol = NetUnpack.Unpackli(r.Data, 0);
            //if (protocol == 1) {
            //    _ctx.TiSync.Sync((int)r.Localtime, (int)r.Globaltime);
            //} else if (protocol == 2) {
            //    Debug.Log(string.Format("{0}, {1}", r.Session, protocol));
            //    int k = NetUnpack.Unpackli(r.Data, 4);
            //    if (_lastK == 0) {
            //        _lastK = k;
            //    } else if (k > _lastK) {
            //        _lastK = k;
            //        if (r.Data.Length > 4) {
            //            int ball_sz = NetUnpack.Unpackli(r.Data, 8);
            //            for (int i = 0; i < ball_sz; i++) {
            //                long ballid = NetUnpack.Unpackll(r.Data, 12 + (i * 32) + 0);
            //                float px = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 8);
            //                float py = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 12);
            //                float pz = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 16);
            //                float dx = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 20);
            //                float dy = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 24);
            //                float dz = NetUnpack.Unpacklf(r.Data, 12 + (i * 32) + 28);
            //                _scene.UpdateBall(ballid, new Vector3(px, py, pz), new Vector3(dx, dy, dz));
            //            }
            //            var player = _playes[_mysession];
            //            var pivot = player.GetPivot();
            //            _view.MoveTo(new Vector2(pivot.x, pivot.z));
            //        }
            //    } else {
            //    }
            //}
        }
    }
}
