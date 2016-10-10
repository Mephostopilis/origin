using Maria;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bacon
{
    class GameController : Controller
    {

        private float _synccd = 1;
        private bool _udpflag = false;
        private MyBall _myball = null;
        private Map _map = null;
        private Camera _camera = null;
        private Rect _cameraRt = null;
        private bool _moveflag = false;

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
            //_ctx.AuthUdp(AuthUdpCb);
            
        }

        public override void ActiveSceneChanged(Scene from, Scene to)
        {
            base.ActiveSceneChanged(from, to);
        }

        public override void SceneLoaded(Scene scene, LoadSceneMode sm)
        {
            base.SceneLoaded(scene, sm);
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

        public void SetupCamera(Camera camera)
        {
            Debug.Assert(camera != null);
            _camera = camera;

            Quaternion q = Quaternion.AngleAxis(90, new Vector3(1.0f, 0.0f, 0.0f));
            _camera.transform.localRotation = q;

            float aspect = camera.aspect;
            float fov = camera.fieldOfView;
            float near = camera.nearClipPlane;
            float depth = camera.transform.localPosition.y;
            depth = 20;
            float yMax = Mathf.Tan(fov / 2.0f / 180 * Mathf.PI) * depth;
            float yMin = -yMax;
            float z = depth;
            float xMax = aspect * yMax;
            float xMin = -xMax;

           
            var childTrans = camera.transform.GetChild(0).transform;

            Transform min = Object.Instantiate(camera.transform);
            Transform max = Object.Instantiate(camera.transform);

            max.Translate(new Vector3(xMax, yMax, z));
            min.Translate(new Vector3(xMin, yMin, z));

            Vector3 max_position = max.position;
            Vector3 min_position = min.position;

            Object.DestroyImmediate(min.gameObject);
            Object.DestroyImmediate(max.gameObject);
            

            //Matrix4x4 m = 
            //Vector3 min1 = camera.transform.worldToLocalMatrix * new Vector3(xMin, yMin, z);
            //Vector3 max1 = camera.transform.worldToLocalMatrix * new Vector3(xMax, yMax, z);

            //Vector3 min = camera.transform.parent.worldToLocalMatrix * min1;
            //Vector3 max = camera.transform.parent.worldToLocalMatrix * max1;

            //Vector3 by = new Vector3(-min.x, 0, -min.z);
            //min += by;
            //max += by;

            //_camera.transform.Translate(camera.transform.localPosition + by);

            //_cameraRt = new Rect(min, max);
        }

        public void SetupMap(GameObject map)
        {
            Debug.Assert(_map == null);
            if (_map == null)
            {
                _map = new Map(map);
            }
        }

        public void SetupMyBall(GameObject ball)
        {
            Debug.Assert(_myball == null);
            if (_myball == null)
            {
                _myball = new MyBall(ball, 1);
                _myball.MoveTo(new Vector2(10, 10));
            }
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
    }
}
