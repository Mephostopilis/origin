using UnityEngine;

namespace Maria {
    public class AABB {
        public Vector3 _min;
        public Vector3 _max;

        public AABB() {
            reset();
        }

        public AABB(Vector3 min, Vector3 max) {
            _min = min;
            _max = max;
        }

        public Vector3 getCenter() {
            Vector3 center = new Vector3();
            center.x = 0.5f * (_min.x + _max.x);
            center.y = 0.5f * (_min.y + _max.y);
            center.z = 0.5f * (_min.z + _max.z);
            return center;
        }

        public void getCorners(Vector3[] dst) {
            Debug.Assert(dst != null && dst.Length >= 8);
            // left-top-front
            dst[0] = new Vector3(_min.x, _max.y, _max.z);
            // left-bottom-front
            dst[1] = new Vector3(_min.x, _min.y, _max.z);
            // right-bottom-front
            dst[2] = new Vector3(_max.x, _min.y, _max.z);
            // right-top-front
            dst[3] = new Vector3(_max.x, _max.y, _max.z);

            // right-top_back
            dst[4] = new Vector3(_max.x, _max.y, _min.z);
            // right-bottom-back
            dst[5] = new Vector3(_max.x, _min.y, _min.z);
            // left-bottom-back
            dst[6] = new Vector3(_max.x, _min.y, _min.z);
            // left-top-back
            dst[7] = new Vector3(_min.x, _max.y, _min.z);
        }

        public bool intersects(AABB aabb) {
            return ((_min.x >= aabb._min.x && _min.x <= aabb._max.x) || (aabb._min.x >= _min.x && aabb._min.x <= _max.x)) &&
           ((_min.y >= aabb._min.y && _min.y <= aabb._max.y) || (aabb._min.y >= _min.y && aabb._min.y <= _max.y)) &&
           ((_min.z >= aabb._min.z && _min.z <= aabb._max.z) || (aabb._min.z >= _min.z && aabb._min.z <= _max.z));
        }

        public bool containPoint(Vector3 point) {
            if (point.x < _min.x) return false;
            if (point.y < _min.y) return false;
            if (point.z < _min.z) return false;
            if (point.x > _max.x) return false;
            if (point.y > _max.y) return false;
            if (point.z > _max.z) return false;
            return true;
        }

        public void merge(AABB box) {
            // Calculate the new minimum point.
            _min.x = Mathf.Min(_min.x, box._min.x);
            _min.y = Mathf.Min(_min.y, box._min.y);
            _min.z = Mathf.Min(_min.z, box._min.z);

            // Calculate the new maximum point.
            _max.x = Mathf.Max(_max.x, box._max.x);
            _max.y = Mathf.Max(_max.y, box._max.y);
            _max.z = Mathf.Max(_max.z, box._max.z);
        }

        public void set(Vector3 min, Vector3 max) {
            _min = min;
            _max = max;
        }

        public void reset() {
            _min = Vector3.zero;
            _max = Vector3.zero;
        }

        public bool isEmpty() {
            return _min.x > _max.x || _min.y > _max.y || _min.z > _max.z;
        }

        public void updateMinMax(Vector3[] point) {
            Debug.Assert(point != null);
            for (int i = 0; i < point.Length; i++) {
                // Leftmost point.
                if (point[i].x < _min.x)
                    _min.x = point[i].x;

                // Lowest point.
                if (point[i].y < _min.y)
                    _min.y = point[i].y;

                // Farthest point.
                if (point[i].z < _min.z)
                    _min.z = point[i].z;

                // Rightmost point.
                if (point[i].x > _max.x)
                    _max.x = point[i].x;

                // Highest point.
                if (point[i].y > _max.y)
                    _max.y = point[i].y;

                // Nearest point.
                if (point[i].z > _max.z)
                    _max.z = point[i].z;
            }
        }

        public void transform(Matrix4x4 mat) {
            Vector3[] corners = new Vector3[8];
            // Near face, specified counter-clockwise
            // Left-top-front.
            corners[0] = new Vector3(_min.x, _max.y, _max.z);
            // Left-bottom-front.
            corners[1] = new Vector3(_min.x, _min.y, _max.z);
            // Right-bottom-front.
            corners[2] = new Vector3(_max.x, _min.y, _max.z);
            // Right-top-front.
            corners[3] = new Vector3(_max.x, _max.y, _max.z);

            // Far face, specified clockwise
            // Right-top-back.
            corners[4] = new Vector3(_max.x, _max.y, _min.z);
            // Right-bottom-back.
            corners[5] = new Vector3(_max.x, _min.y, _min.z);
            // Left-bottom-back.
            corners[6] = new Vector3(_min.x, _min.y, _min.z);
            // Left-top-back.
            corners[7] = new Vector3(_min.x, _max.y, _min.z);

            for (int i = 0; i < 8; i++) {
                corners[i] = mat * corners[i];
            }

            reset();
            updateMinMax(corners);
        }

        public bool containAABB2D(AABB box, out Vector2 offset) {
            Vector3 dir = box._min - _min;
            Vector3 n = dir.normalized;
            float nrad = Mathf.Atan2(n.z, n.x);
            // 先判断是否在第一象限
            if (n.x == 1) {
                // 在边界上，不偏移
                offset = Vector2.one;
                return true;
            } else if (n.z == 1) {
                offset = Vector2.one;
                return true;
            } else if (nrad > 0 && nrad < Mathf.PI / 2) {
                offset = Vector2.one;
                return true;
            } else {
                offset.x = dir.x > 0 ? dir.x : (-dir.x);
                offset.y = dir.z > 0 ? dir.z : (-dir.z);
                return false;
            }

            Vector3 xdir = box._max - _max;
            Vector3 x = xdir.normalized;
            float xrad = Mathf.Atan2(n.z, n.x);
            if (x.x == 1) {
                offset = Vector2.one;
                return true;
            } else if (x.z == 1) {
                offset = Vector2.one;
                return true;
            } else if (xrad > 0 && xrad < Mathf.PI / 2) {
                offset = Vector2.one;
                return true;
            } else {
                offset.x = xdir.x > 0 ? xdir.x : (-xdir.x);
                offset.y = xdir.z > 0 ? xdir.z : (-xdir.z);
                return false;
            }
        }
    }
}
