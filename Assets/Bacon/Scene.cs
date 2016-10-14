using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class Scene
    {
        private AppContext _ctx = null;
        private Dictionary<uint, Ball> _uballs = new Dictionary<uint, Ball>();
        private Dictionary<uint, Ball> _sessionBalls = new Dictionary<uint, Ball>();
        private List<Ball> _balls = new List<Ball>();

        private View _view = null;
        private Map _map = null;

        public Scene(Context ctx, View view, Map map)
        {
            Debug.Assert(ctx != null);
            Debug.Assert(view != null);
            Debug.Assert(map != null);
            _ctx = ctx as AppContext;
            _view = view;
            _map = map;

            AABB maabb = _map.AABB;
            AABB vaabb = _view.AABB;
            Vector2 offset = new Vector2();
            if (maabb.containAABB2D(vaabb, out offset))
            {
            }
            else
            {
                Vector3 t = new Vector3(offset.x, offset.y, 0);
                _view.Translate(t);
            }
        }

        public Ball SetupBall(uint uid, uint session, float radis, float length, float width, float height, Vector3 position, Vector3 dir, float vel, GameObject o)
        {
            Ball ball = new Ball(o, radis, length, width, height);
            ball.MoveTo(position);
            ball.Dir = dir;
            ball.Vel = vel;
            ball.Uid = uid;
            ball.Session = session;
            _uballs[uid] = ball;
            _sessionBalls[session] = ball;
            _balls.Add(ball);
            return ball;
        }

        public void Update(uint session, Vector3 pos, Vector3 dir)
        {
            try
            {
                var ball = _sessionBalls[session];
                ball.MoveTo(pos);
                ball.Dir = dir;
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
