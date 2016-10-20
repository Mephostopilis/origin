using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    public class View
    {
        private Scene _scene = null;

        private GameObject _camera = null;

        private Vector3 _pos = Vector3.zero;
        private AABB _aabb = null;

        public View(Scene scene, GameObject camera)
        {
            _scene = scene;

            _camera = camera;
            _pos = _camera.transform.localPosition;

            var vp = _camera.transform.FindChild("Main Camera");

            Quaternion q = Quaternion.AngleAxis(90, new Vector3(1.0f, 0.0f, 0.0f));
           vp.transform.localRotation = q;

            Camera c = vp.GetComponent<Camera>();
            float aspect = c.aspect;
            float fov = c.fieldOfView;
            float near = c.nearClipPlane;
            float depth = c.transform.localPosition.y;
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

            var com = _camera.gameObject.GetComponent<AABBBehaviour>();
            com.AABB = _aabb;
        }

        public AABB AABB
        {
            get
            {
                return _aabb;
            }
        }

        public GameObject Go {
            get { return _camera; }
        }

        public void Translate(Vector3 t)
        {
            _camera.transform.Translate(t);
        }

    }
}
