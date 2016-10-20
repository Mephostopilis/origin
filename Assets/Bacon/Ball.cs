using Maria.Network;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Ball {

        protected Scene _scene = null;

        protected uint _uid = 0;
        protected uint _session = 0;

        protected GameObject _ball = null;
        protected GameObject _toy = null;
        protected GameObject _arrow = null;

        protected float _radis = 0;
        protected float _length = 0.0f;
        protected float _width = 0.0f;
        protected float _height = 0.0f;

        protected Vector3 _pos = Vector3.zero;
        protected Vector3 _dir = new Vector3(0, 0, 0);
        protected float _vel = 0.0f;

        protected AABB _aabb = null;
        

        public Ball(Scene scene, GameObject o, float radis, float length, float width, float height) {
            Debug.Assert(_ball != o);

            _scene = scene;

            _uid = 0;
            _session = 0;

            _ball = o;
            try {
                _toy = _ball.transform.FindChild("Toy").gameObject;
                _arrow = _ball.transform.FindChild("Arrow").gameObject;
                CalArrowPosition();
                CalArrowDirection();
            } catch (KeyNotFoundException ex) {
                throw;
            }
            
            _radis = radis;
            _length = length;
            _width = width;
            _height = height;

            _pos = Vector3.zero;
            _dir = Vector3.right;
            _vel = 0.0f;

            Vector3 position = _ball.transform.localPosition;
            Vector3 min = position + new Vector3(-_length / 2, -_width / 2, -_height / 2);
            Vector3 max = position + new Vector3(_length / 2, _width / 2, _height / 2);
            _aabb = new AABB(min, max);

            var com = o.GetComponent<AABBBehaviour>();
            com.AABB = _aabb;
        }

        #region sync server
        public uint Uid { get { return _uid; } set { _uid = value; } }

        public uint Session { get { return _session; } set { _session = value; } }

        public Vector3 Dir { get { return _dir; } set { _dir = value; } }

        public float Vel { get { return _vel; } set { _vel = value; } }

        public AABB AABB { get { return _aabb; } }

        public void MoveBy(Vector2 v) {
            Vector3 shift = new Vector3(v.x, 0, v.y);
            MoveBy(shift);
        }

        public void MoveBy(Vector3 v) {
            _pos += v;
            if (_ball != null) {
                _ball.transform.Translate(v);
            }
        }

        public void MoveTo(Vector2 v) {
            // 保留原来的y值，
            var position = _ball.transform.localPosition;
            Vector3 shift = new Vector3(v.x, position.y, v.y);
            MoveTo(shift);
        }

        public void MoveTo(Vector3 v) {
            _pos = v;
            if (_ball != null) {
                _ball.transform.localPosition = v;
            }
        }

        protected void CalArrowPosition() {
            Vector3 position = _ball.transform.localPosition;
            Vector3 arr = _dir * _radis + position;
            _arrow.transform.position = arr;
        }

        protected void CalArrowDirection() {
            //_direction. Vector3.up;
        }

        public byte[] PackPos() {
            if (_ball != null) {
                _pos = _ball.transform.localPosition;
            }
            byte[] res = new byte[12];
            NetPack.Packlf(res, 0, _pos.x);
            NetPack.Packlf(res, 4, _pos.y);
            NetPack.Packlf(res, 8, _pos.z);
            return res;
        }

        public byte[] PackDir() {
            Vector3 direction = _dir;
            byte[] res = new byte[12];
            NetPack.Packlf(res, 0, direction.x);
            NetPack.Packlf(res, 4, direction.y);
            NetPack.Packlf(res, 8, direction.z);
            return res;
        }
        #endregion

        public void Leave() {
            GameObject.DestroyImmediate(_ball);
        }
    }
}
