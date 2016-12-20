using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Maria;

namespace Bacon {
    public class View : Maria.Actor {

        private Scene _scene = null;
        private GameObject _camera = null;

        private Matrix4x4 _centerMat = Matrix4x4.identity;
        private Vector3 _centerPos = Vector3.zero;
        private Matrix4x4 _viewMat = Matrix4x4.identity;
        private Quaternion _viewQuat = Quaternion.identity;
        private float _aspect = 0.0f;
        private float _fov = 0.0f;
        private float _near = 0.0f;
        private float _depth = 0.0f;

        private Maria.AABB _aabb = null;
        private Vector3 _min = Vector3.zero;
        private Vector3 _max = Vector3.zero;

        public View(Context ctx, Controller controller, GameObject go, Scene scene) : base(ctx, controller, go) {
            _scene = scene;

            _ctx.EnqueueRenderQueue(RenderInitView);
        }

        public void RenderInitView() {
            if (_go != null) {
                _centerMat = _go.transform.localToWorldMatrix;
                _centerPos = _go.transform.localPosition;

                _camera = _go.transform.FindChild("Main Camera").gameObject;
                
                _viewQuat = Quaternion.AngleAxis(90, new Vector3(1.0f, 0.0f, 0.0f));
                Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, _viewQuat, Vector3.one);
                _viewMat = _centerMat * r;
                _camera.transform.localRotation = _viewQuat;

                Camera c = _camera.GetComponent<Camera>();
                _aspect = c.aspect;
                _fov = c.fieldOfView;
                _near = c.nearClipPlane;
                _depth = _centerPos.y;

                float yMax = Mathf.Tan(_fov / 2.0f / 180 * Mathf.PI) * _depth;
                float yMin = -yMax;
                float z = _depth;
                float xMax = _aspect * yMax;
                float xMin = -xMax;

                Matrix4x4 maxtm = Matrix4x4.TRS(new Vector3(xMax, yMax, z), Quaternion.identity, Vector3.one);
                Matrix4x4 maxm = _viewMat * maxtm * _centerMat.inverse;
                Matrix4x4 mintm = Matrix4x4.TRS(new Vector3(xMin, yMin, z), Quaternion.identity, Vector3.one);
                Matrix4x4 minm = _viewMat * mintm * _centerMat.inverse;
                Vector3 min = new Vector3(minm.m03, minm.m13, minm.m23);
                Vector3 max = new Vector3(maxm.m03, maxm.m13, maxm.m23);
                _aabb = new AABB(min, max);
            } else {
                Debug.LogError("_go is empty.");
            }
        }

        public void InitView(Vector3 pos) {
            Debug.Assert(false);

            _centerPos.x = pos.x;
            _centerPos.z = pos.z;

            _centerMat.m03 = _centerPos.x;
            _centerMat.m23 = _centerPos.y;

            Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, _viewQuat, Vector3.one);
            _viewMat = _centerMat * r;

            _depth = _centerPos.y - pos.y;
            Debug.Assert(_depth == 15);

            float yMax = Mathf.Tan(_fov / 2.0f / 180 * Mathf.PI) * _depth;
            float yMin = -yMax;
            float z = _depth;
            float xMax = _aspect * yMax;
            float xMin = -xMax;

            Matrix4x4 maxtm = Matrix4x4.TRS(new Vector3(xMax, yMax, z), Quaternion.identity, Vector3.one);
            Matrix4x4 maxm = _viewMat * maxtm * _centerMat.inverse;
            Matrix4x4 mintm = Matrix4x4.TRS(new Vector3(xMin, yMin, z), Quaternion.identity, Vector3.one);
            Matrix4x4 minm = _viewMat * mintm * _centerMat.inverse;
            Vector3 min = new Vector3(minm.m03, minm.m13, minm.m23);
            Vector3 max = new Vector3(maxm.m03, maxm.m13, maxm.m23);
            _max = max;
            _min = min;
        }

        public AABB AABB {
            get {
                return _aabb;
            }
        }

        public void MoveTo(Vector2 v) {
            _centerPos.x = v.x;
            _centerPos.z = v.y;
            _ctx.EnqueueRenderQueue(RenderTransform);
        }

        public void Contains(Vector3 pivot) {
            Debug.Assert(false);
            float padding = 2;
            float dx = 0, dy = 0, dz = 0;
            if (pivot.x + padding < _max.x) {
            } else {
                dx = _max.x - (pivot.x + padding);
            }
            if (pivot.x - padding > _min.x) {
            } else {
                dx = _min.x - (pivot.x - padding);
            }
            if (pivot.z + padding < _max.z) {
            } else {
                dz = _max.z - (pivot.z + padding);
            }
            if (pivot.z - padding > _min.z) {
            } else {
                dz = _min.z - (pivot.z - padding);
            }
            _centerPos.x += dx;
            _centerPos.z += dz;

            if (dx > 0 || dz > 0) {
                InitView(pivot);
            }

            _ctx.EnqueueRenderQueue(RenderTransform);
        }

        public void RenderTransform() {
            if (_go != null) {
                _go.transform.localPosition = _centerPos;
            }
        }

        public void RenderView() {
            if (_go != null) {
                _camera.transform.localRotation = _viewQuat;
            }
        }
    }
}
