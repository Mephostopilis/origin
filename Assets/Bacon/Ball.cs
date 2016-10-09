using UnityEngine;

namespace Bacon
{
    class Ball
    {
        protected GameObject _ball = null;
        protected GameObject _arrow = null;
        protected float _radis = 0;
        protected Vector3 _direction = new Vector3(0, 0, 0);
        
        public Ball(GameObject o, float radis)
        {
            Debug.Assert(o != null);
            _ball = o;
            _arrow = _ball.transform.GetChild(0).gameObject;
            _radis = radis;
            _direction = Vector3.right;
            CalArrowPosition();
            CalArrowDirection();
        }

        public void MoveBy(Vector2 v)
        {
            Vector3 shift = new Vector3(v.x, v.y, 0);
            Vector3 origin = _ball.transform.localPosition;
            Vector3 dst = origin + shift;
            _ball.transform.Translate(dst);
        }

        public void MoveBy(Vector3 v)
        {
            Vector3 origin = _ball.transform.localPosition;
            Vector3 dst = origin + v;
            _ball.transform.Translate(dst);
        }

        public void MoveTo(Vector2 v)
        {
            Vector3 shift = new Vector3(v.x, v.y, 0);
            _ball.transform.Translate(shift);
        }

        public void MoveTo(Vector3 v)
        {
            _ball.transform.Translate(v);
        }

        protected void CalArrowPosition()
        {
            Vector3 position = _ball.transform.localPosition;
            Vector3 arr = _direction * _radis + position;
            _arrow.transform.position = arr;
        }

        protected void CalArrowDirection()
        {
            //_direction. Vector3.up;
        }

    }
}
