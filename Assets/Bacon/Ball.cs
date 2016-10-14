using Maria.Network;
using UnityEngine;

namespace Bacon
{
    class Ball
    {
        protected GameObject _ball = null;
        protected GameObject _arrow = null;
        protected float _radis = 0;
        protected float _length = 0.0f;
        protected float _width = 0.0f;
        protected float _height = 0.0f;
        protected Vector3 _dir = new Vector3(0, 0, 0);
        protected Vector3 _pos = Vector3.zero;
        protected float _vel = 0.0f;
        protected AABB _aabb = null;
        protected uint _uid = 0;
        protected uint _session = 0;

        public Ball(GameObject o, float radis, float length, float width, float height)
        {
            Debug.Assert(_ball != o);
            _ball = o;
            _arrow = _ball.transform.GetChild(0).gameObject;
            _radis = radis;
            _length = length;
            _width = width;
            _height = height;
            _dir = Vector3.right;
            _vel = 0.0f;
            CalArrowPosition();
            CalArrowDirection();
            Vector3 position = _ball.transform.localPosition;
            Vector3 min = position + new Vector3(-_length / 2, -_width / 2, -_height / 2);
            Vector3 max = position + new Vector3(_length / 2, _width / 2, _height / 2);
            _aabb = new AABB(min, max);

        }

        public uint Uid { get { return _uid; } set { _uid = value; } }

        public uint Session { get { return _session; } set { _session = value; } }

        public Vector3 Dir { get { return _dir; } set { _dir = value; } }

        public float Vel { get { return _vel; } set { _vel = value; } }

        public void MoveBy(Vector2 v)
        {
            Vector3 shift = new Vector3(v.x, v.y, 0);
            MoveBy(shift);
        }

        public void MoveBy(Vector3 v)
        {
            _pos += v;
            if (_ball != null)
            {
                _ball.transform.Translate(v);
            }
        }

        public void MoveTo(Vector2 v)
        {
            var position = _ball.transform.localPosition;
            Vector3 shift = new Vector3(v.x, v.y, position.z);
            MoveTo(shift);
        }

        public void MoveTo(Vector3 v)
        {
            _pos = v;
            if (_ball != null)
            {
                _ball.transform.localPosition = v;
            }
        }

        protected void CalArrowPosition()
        {
            Vector3 position = _ball.transform.localPosition;
            Vector3 arr = _dir * _radis + position;
            _arrow.transform.position = arr;
        }

        protected void CalArrowDirection()
        {
            //_direction. Vector3.up;
        }

        public byte[] PackPosition()
        {
            byte[] res = new byte[12];
            NetPack.Packlf(res, 0, _pos.x);
            NetPack.Packlf(res, 4, _pos.y);
            NetPack.Packlf(res, 8, _pos.z);
            return res;
        }

        public byte[] PackDirection()
        {
            Vector3 direction = _dir;
            byte[] res = new byte[12];
            NetPack.Packlf(res, 0, direction.x);
            NetPack.Packlf(res, 4, direction.y);
            NetPack.Packlf(res, 8, direction.z);
            return res;
        }

        public void SyncPosition(byte[] buffer, int start, int len)
        {
            Debug.Assert(buffer.Length >= (start + 12));
            float x = NetUnpack.Unpacklf(buffer, start);
            float y = NetUnpack.Unpacklf(buffer, start + 4);
            float z = NetUnpack.Unpacklf(buffer, start + 8);
            _ball.transform.localPosition = new Vector3(x, y, z);
        }

        public void SyncDirection(byte[] buffer, int start, int len)
        {
            Debug.Assert(buffer.Length >= (start + 12));
            float x = NetUnpack.Unpacklf(buffer, start);
            float y = NetUnpack.Unpacklf(buffer, start + 4);
            float z = NetUnpack.Unpacklf(buffer, start + 8);
            _dir.Set(x, y, z);
        }

        public void Leave()
        {
            GameObject.DestroyImmediate(_ball);
        }
    }
}
