using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {
        private Matrix4x4 _mat = Matrix4x4.identity;

        private View _view = null;
        private Map _map = null;
        private Dictionary<long, Ball> _ballidBalls = new Dictionary<long, Ball>();

        public Scene(Context ctx, Controller controller, GameObject go) : base(ctx, controller, go) {

            _view = null;
            _map = null;

            //AABB maabb = _map.AABB;
            //AABB vaabb = _view.AABB;
            //Vector2 offset = new Vector2();
            //if (maabb.containAABB2D(vaabb, out offset))
            //{
            //}
            //else
            //{
            //    Vector3 t = new Vector3(offset.x, offset.y, 0);
            //    _view.Translate(t);
            //}
        }

        public override void Update(float delta) {
            base.Update(delta);

            //Ball myball = _sessionBalls[_mysession];
            //Debug.Assert(myball != null);
            //foreach (var item in _balls) {
            //    if (myball.AABB.intersects(item.AABB)) {

            //    }
            //}
        }

        public View View { get { return _view; } set { _view = value; } }

        public Map Map { get { return _map; } set { _map = value; } }

        public View SetupView(GameObject go) {
            _view = new View(_ctx, _controller, go, this);
            return _view;
        }

        public Map SetupMap(GameObject go) {
            _map = new Map(_ctx, _controller, go, this);
            return _map;
        }

        public Ball SetupBall(long ballid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel) {
            Ball ball = new Ball(_ctx, _controller, this, radis, length, width, height);
            ball.MoveTo(position);
            ball.Dir = dir;
            ball.Vel = vel;
            ball.Session = session;
            ball.Id = ballid;
            _ballidBalls[ballid] = ball;
            return ball;
        }

        public void UpdateBall(long ballid, Vector3 pos, Vector3 dir) {
            try {
                if (_ballidBalls.ContainsKey(ballid)) {
                    Debug.Log(string.Format("update ball {0}", ballid));
                    var ball = _ballidBalls[ballid];
                    ball.MoveTo(pos);
                    ball.Dir = dir;
                }
                // 检测碰撞
            } catch (KeyNotFoundException ex) {
                Debug.Log(ex.Message);
            }
        }

        public Ball Leave(long ballid) {
            if (_ballidBalls.ContainsKey(ballid)) {
                var ball = _ballidBalls[ballid];
                ball.Leave();
                _ballidBalls.Remove(ballid);
                return ball;
            } else {
                Debug.LogError(string.Format("contains key no %l", ballid));
                return null;
            }
        }
    }
}
