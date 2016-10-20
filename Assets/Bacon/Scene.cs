using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    public class Scene
    {
        private AppContext _ctx = null;
        private GameObject _world = null;

        private Dictionary<uint, Ball> _uballs = new Dictionary<uint, Ball>();
        private Dictionary<uint, Ball> _sessionBalls = new Dictionary<uint, Ball>();
        private List<Ball> _balls = new List<Ball>();

        private View _view = null;
        private Map _map = null;

        public Scene(Context ctx, GameObject go)
        {
            Debug.Assert(ctx != null);
            
            _ctx = ctx as AppContext;
            _world = go;

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

        internal void Update(float delta) {
            //Ball myball = _sessionBalls[_mysession];
            //Debug.Assert(myball != null);
            //foreach (var item in _balls) {
            //    if (myball.AABB.intersects(item.AABB)) {

            //    }
            //}
        }

        public Map CreateMap(GameObject go) {
            _map = new Map(this, go);
            return _map;
        }

        public View View { get { return _view; } set { _view = value; } }

        public Map Map { get { return _map; } set { _map = value; } }

        public View SetupView(GameObject go) {
            _view = new View(this, go);
            var com = go.GetComponent<ViewBehaviour>();
            com.SetupView(_view);
            return _view;
        }

        public Map SetupMap(GameObject go) {
            _map = new Map(this, go);
            var com = go.GetComponent<MapBehaviour>();
            com.SetupMap(_map);
            return _map;
        }

        public Ball SetupBall(uint uid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel, GameObject o)
        {
            Ball ball = null;
            if (session == (uint)_ctx.Session) {
                ball = new MyBall(this, o, radis, length, width, height);
            } else {
                ball = new Ball(this, o, radis, length, width, height);
            }
            ball.MoveTo(position);
            ball.Dir = dir;
            ball.Vel = vel;
            ball.Uid = uid;
            ball.Session = session;
            _uballs[uid] = ball;
            _sessionBalls[session] = ball;
            _balls.Add(ball);

            o.transform.SetParent(_world.transform);
            var com = o.GetComponent<BallBehaviour>();
            com.SetupBall(ball);
            return ball;
        }

        public void UpdateBall(uint session, Vector3 pos, Vector3 dir)
        {
            try
            {
                var ball = _sessionBalls[session];
                ball.MoveTo(pos);
                ball.Dir = dir;

                // 检测碰撞

            }
            catch (KeyNotFoundException ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public void Leave(uint session)
        {
            var ball = _sessionBalls[session];
            _uballs[ball.Uid] = null;
            _balls.Remove(ball);
            ball.Leave();
            _sessionBalls[session] = null;
        }
    }
}
