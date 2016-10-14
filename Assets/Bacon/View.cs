using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class View
    {
        private Camera _camera = null;
        private AABB _aabb = null;

        public View(Camera camera)
        {
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

            Matrix4x4 maxtm = Matrix4x4.TRS(new Vector3(xMax, yMax, z), Quaternion.identity, Vector3.one);
            Matrix4x4 maxm = camera.transform.localToWorldMatrix * maxtm;
            Matrix4x4 mintm = Matrix4x4.TRS(new Vector3(xMin, yMin, z), Quaternion.identity, Vector3.one);
            Matrix4x4 minm = camera.transform.localToWorldMatrix * mintm;
            Vector3 min = new Vector3(minm.m03, minm.m13, minm.m23);
            Vector3 max = new Vector3(maxm.m03, maxm.m13, maxm.m23);

            _aabb = new AABB(min, max);
        }

        public AABB AABB
        {
            get
            {
                return _aabb;
            }
        }

        public void Translate(Vector3 t)
        {
            _camera.transform.Translate(t);
        }

    }
}
