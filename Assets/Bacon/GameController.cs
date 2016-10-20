using Maria;
using Maria.Encrypt;
using Maria.Network;
using Sproto;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bacon
{
    class GameController : Controller
    {
        private float _synccd1 = 1;
        private byte[] _syncmsg1 = null;

        private Ball _myball = null;
        private Map _map = null;
        private View _view = null;
        private Scene _scene = null;

        private bool _moveflag = false;

        public GameController(Context ctx) : base(ctx)
        {
            // 4 + 12:pos + 12:dir
            _syncmsg1 = new byte[28];
            NetPack.Packli(_syncmsg1, 0, 1);
        }

        internal override void Update(float delta)
        {
            base.Update(delta);
            if (_scene != null) {
                _scene.Update(delta);
            }
            Sync1(delta);
        }

        public void Sync1(float delta)
        {
            if (_authudp && (_myball != null))
            {
                if (_synccd1 > 0)
                {
                    _synccd1 -= delta;
                    if (_synccd1 <= 0)
                    {
                        _synccd1 = 1.0f;
                        int[] e = _ctx.TiSync.GlobalTime();
                        Debug.Log(string.Format("globaltime: {0}", e[0]));
                        byte[] pos = _myball.PackPos();
                        byte[] dir = _myball.PackDir();
                        Debug.Assert(pos.Length == 12);
                        Array.Copy(pos, 0, _syncmsg1, 4, pos.Length);
                        Debug.Assert(dir.Length == 12);
                        Array.Copy(dir, 0, _syncmsg1, 16, dir.Length);
                        _ctx.SendUdp(_syncmsg1);
                    }
                }
            }
        }

        public override void Enter()
        {
            base.Enter();
            LoadScene("game");
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

        public override void AuthGateCB(int code)
        {
            base.AuthGateCB(code);
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
        }

        public override void AuthUdpCb(bool ok)
        {
            base.AuthUdpCb(ok);
        }

        // 初始游戏场景
        private Ball SetupBall(uint uid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel)
        {
            string path = "Prefabs/Ball";
            UnityEngine.Object o = Resources.Load(path, typeof(GameObject));
            GameObject go = UnityEngine.Object.Instantiate(o) as GameObject;

            Ball ball = _scene.SetupBall(uid, session, radis, length, width, height, position, dir, vel, go);
            return ball;
        }

        public void SetupCamera(GameObject go)
        {
            // 无论_camera == null,新场景启动都要重置
            Debug.Assert(_scene != null);
            _view = _scene.SetupView(go);
        }

        public void SetupMap(GameObject map)
        {
            Debug.Assert(_scene != null);
            _map = _scene.SetupMap(map);
        }

        public void SetupScene(GameObject word)
        {
            _scene = new Scene(_ctx, word);
        }

        public void OnMoveStart()
        {
            _moveflag = true;
        }

        public void OnMove(Vector2 d)
        {
            if (_moveflag)
            {
                Debug.Log(string.Format("{0},{1}", d.x, d.y));

                _moveflag = false;

                //Vector2 dir = d.normalized;
                Vector2 dir = d;
                float speed = 10f;
                Vector2 shift = dir * speed;
                _myball.MoveBy(shift);
            }
        }

        // 游戏协议
        // 主要是同步场景中已经加入的其他玩家
        public void Join(SprotoTypeBase responseObj)
        {
            if (responseObj != null)
            {
                C2sSprotoType.join.response o = responseObj as C2sSprotoType.join.response;
                string host = o.host;
                int port = (int)o.port;
                _ctx.AuthUdpCb(o.session, host, port);

                var all = o.all;
                foreach (var item in all)
                {
                    uint uid = (uint)item.uid;
                    uint session = (uint)item.session;
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
                    if (session == (uint)_ctx.Session)
                    {
                        _myball = SetupBall(uid, session, radis, length, width, height, pos, dir, vel);
                    }
                    else
                    {
                        SetupBall(uid, session, radis, length, width, height, pos, dir, vel);
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
                    _myball = SetupBall(uid, session, radis, length, width, height, pos, dir, vel);
                    _myball.MoveTo(new Vector2(10, 10));
                } else {
                    Debug.Assert(false);
                    SetupBall(uid, session, radis, length, width, height, pos, dir, vel);
                }
            }
        }

        public SprotoTypeBase OnBorn(SprotoTypeBase requestObj)
        {
            S2cSprotoType.born.request obj = requestObj as S2cSprotoType.born.request;
            if (obj != null)
            {
                S2cSprotoType.ball b = obj.b;
                uint uid = (uint)b.uid;
                uint session = (uint)b.session;
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
                SetupBall(uid, session, radis, length, width, height, pos, dir, vel);

                S2cSprotoType.born.response responseObj = new S2cSprotoType.born.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            }
            else
            {
                throw new System.Exception("request obj is null");
            }
        }

        public SprotoTypeBase OnLeave(SprotoTypeBase requestObj)
        {
            S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            if (obj != null)
            {
                if (_myball != null && _myball.Session == (uint)obj.session)
                {
                    _myball = null;
                }

                _scene.Leave((uint)obj.session);
                S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            }
            else
            {
                throw new System.Exception("obj is null");
            }
        }

        public override void OnRecviveUdp(PackageSocketUdp.R r)
        {
            base.OnRecviveUdp(r);
            uint protocol = NetUnpack.UnpacklI(r.Data, 0);
            if (protocol == 1)
            {
                if (_myball != null)
                {
                    if (_myball.Session != r.Session)
                    {
                        float px = NetUnpack.Unpacklf(r.Data, 0);
                        float py = NetUnpack.Unpacklf(r.Data, 4);
                        float pz = NetUnpack.Unpacklf(r.Data, 8);
                        float dx = NetUnpack.Unpacklf(r.Data, 12);
                        float dy = NetUnpack.Unpacklf(r.Data, 16);
                        float dz = NetUnpack.Unpacklf(r.Data, 20);
                        uint ok = NetUnpack.UnpacklI(r.Data, 24);
                        _scene.UpdateBall((uint)r.Session, new Vector3(px, py, pz), new Vector3(dx, dy, dz));
                    }
                }
                else
                {
                    float px = NetUnpack.Unpacklf(r.Data, 0);
                    float py = NetUnpack.Unpacklf(r.Data, 4);
                    float pz = NetUnpack.Unpacklf(r.Data, 8);
                    float dx = NetUnpack.Unpacklf(r.Data, 12);
                    float dy = NetUnpack.Unpacklf(r.Data, 16);
                    float dz = NetUnpack.Unpacklf(r.Data, 20);
                    uint ok = NetUnpack.UnpacklI(r.Data, 24);
                    _scene.UpdateBall((uint)r.Session, new Vector3(px, py, pz), new Vector3(dx, dy, dz));
                }
            }
        }
    }
}
