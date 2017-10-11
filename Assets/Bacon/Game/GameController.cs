using Bacon.Event;
using Bacon.Helper;
using Bacon.Service;
using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Bacon.Game.Systems;

namespace Bacon.Game {

    [XLua.Hotfix]
    [XLua.LuaCallCSharp]
    partial class GameController : Controller {

        //private UIRootActor _ui = null;
        private byte[] _syncmsg1 = null;

        private NetFrameQueue _queue = new NetFrameQueue();
        private Entitas.Systems _systems;
        private IndexSystem _indexsystem;
        private MapSystem _mapsystem;
        private JoinSystem _joinsystem;
        private MyPlayerSystem _myplayersystem;
        private Lua.ILuaGameController _luaBinding;


        public GameController(Context ctx) : base(ctx) {
            _systems = new Entitas.Systems();
            _indexsystem = new IndexSystem(Contexts.sharedInstance.game);
            _mapsystem = new MapSystem(Contexts.sharedInstance.game);
            _joinsystem = new JoinSystem(Contexts.sharedInstance.game);
            _myplayersystem = new MyPlayerSystem(Contexts.sharedInstance.game);
            _systems.Add(_indexsystem)
                .Add(_mapsystem)
                .Add(_joinsystem)
                .Add(_myplayersystem);


            // 4, protocol
            _syncmsg1 = new byte[4];
            NetPack.Packli(_syncmsg1, 0, 1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MAP, SetupMap);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            //EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_VIEW, SetupCamera);
            //_ctx.EventDispatcher.AddCmdEventListener(listener3);

        }

        public override void OnEnter() {
            base.OnEnter();
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            service.SMActor.LoadScene("World");

            _systems.Initialize();

            // udp sync
            Modules.BattleScene m = _ctx.U.GetModule<Modules.BattleScene>();
            long session = m.Session;
            string host = m.UdpHost;
            int port = (int)m.UdpPort;
            _ctx.UdpAuth(session, host, port);

        }

        public override void Update(float delta) {
            base.Update(delta);
        }

        public override void OnCreateLua() {
            _luaBinding = Maria.Lua.LuaPool.Instance.Cache<Lua.ILuaPool>(null).CreateGameController();
        }

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

        public void SetupMap(EventCmd e) {
            GameObject map = e.Orgin;
            _ctx.SendReq<C2sProtocol.setupmap>(C2sProtocol.setupmap.Tag, null);
        }

        //public void SetupScene(EventCmd e) {
        //    GameObject word = e.Orgin;
        //    _scene = new Scene(_ctx, this, word);
        //}



        //public void OnMoveStart() {
        //    _moveflag = true;
        //}

        //public void OnMove(Vector2 d) {
        //    if (_moveflag) {
        //        UnityEngine.Debug.Log(string.Format("move: x {0}, y {1}", d.x, d.y));
        //        _moveflag = false;

        //        ////Vector2 dir = d.normalized;
        //        //Vector2 dir = d;
        //        //float speed = 10f;
        //        //Vector2 shift = dir * speed;
        //        //_myball.MoveBy(shift);
        //    }
        //}

        //public void OnMoveSpeed(Vector2 s) {
        //    if (_moveflag) {
        //        UnityEngine.Debug.Log(string.Format("speed: x {0}, y {1}", s.x, s.y));
        //    }
        //}


        #region response
        // 游戏协议
        // 主要是同步场景中已经加入的其他玩家
        public void Join(SprotoTypeBase responseObj) {
            C2sSprotoType.join.response obj = responseObj as C2sSprotoType.join.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
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

        #endregion

        #region requeset
        public SprotoTypeBase OnJoin(SprotoTypeBase requestObj) {
            S2cSprotoType.join.request obj = requestObj as S2cSprotoType.join.request;
            if (obj != null) {
                if (obj.uid == _ctx.U.Uid) {
                    _myplayersystem.Join(obj.index);
                    _queue.InitK((int)obj.k);
                }
                _joinsystem.Join((int)obj.index);
            }
            S2cSprotoType.join.response responseObj = new S2cSprotoType.join.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }


        #endregion

        // [ protocol : 4
        //   k : 4
        //   len : 4
        //   index : 4
        //   len : 4
        //   opcode : 4
        //   value : 
        //   ...
        //   index : 4
        private void ExecFrame() {
            int k;
            NetFrame frame;
            if (_queue.Dequeue(out k, out frame) == 1) {
                try {
                    int offset = 0;
                    int index = 0, len = 0;
                    offset = NetUnpack.Unpackli(frame.buffer, offset, out index);
                    offset = NetUnpack.Unpackli(frame.buffer, offset, out len);
                    OpCodeParse(index, frame.buffer, offset, len);
                } catch (Exception ex) {
                    k = _queue.BackK();
                    int len = 8;
                    byte[] msg = new byte[len];
                    int offset = 0;
                    offset = NetPack.Packli(msg, offset, (int)ProtocolType.PT_FETCHK);
                    offset = NetPack.Packli(msg, offset, k);
                    _ctx.SendUdp(msg, 0, len);
                }
            } else {
                int len = 8;
                byte[] msg = new byte[len];
                int offset = 0;
                offset = NetPack.Packli(msg, offset, (int)ProtocolType.PT_FETCHK);
                offset = NetPack.Packli(msg, offset, k);
                _ctx.SendUdp(msg, 0, len);
            }
        }

        public override void OnUdpRecv(byte[] data, int start, int len) {
            base.OnUdpRecv(data, start, len);
            int protocol;
            int offset = NetUnpack.Unpackli(data, 0, out protocol);
            if (protocol == ProtocolType.PT_DATA) {
                for (int i = 0; i < 3; i++) {
                    int k, sz;
                    offset = NetUnpack.Unpackli(data, offset, out k);
                    offset = NetUnpack.Unpackli(data, offset, out sz);
                    byte[] buffer = new byte[sz];
                    Array.Copy(data, offset, buffer, 0, sz);
                    NetFrame frame = new NetFrame();
                    frame.k = k;
                    frame.buffer = buffer;
                    _queue.Enqueue(frame);
                }
                ExecFrame();
            }
        }
    }
}
