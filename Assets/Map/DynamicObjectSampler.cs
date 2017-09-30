using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map {
    class DynamicObjectSampler : ObjectSampler {

        public Grid _grid;
        public Vector3 _start;
        public Vector3 _exit;

        private float _vel = 1;
        private Vector3 _dir = new Vector3(1.0f, 0.0f, 1.0f);

        private int _pathid;
        private bool _run = false;
        private Vector3 _dst;

        // Use this for initialization
        void Start() {
            base.Start();
            _type = Type.Dynamic;
            _grid.RegisterObjectSampler(this);
        }

        // Update is called once per frame
        void Update() {
            base.Update();
            if (_run) {
                Vector3 dir = _dst - transform.localPosition;
                dir.Normalize();
                Vector3 dist = dir * _vel * Time.deltaTime;
                transform.localPosition = transform.localPosition + dist;
                if (transform.localPosition == _dst) {
                    _run = false;
                    _run = _grid.NextPoint(_pathid, ref _dst);
                }
            }
            
        }

        public void FindPath(Vector3 dst) {
            Vector3 startPos = transform.position;
            Vector2 exitPos = new Vector3(dst.x, 0.0f, dst.z);
            _pathid = _grid.FindPath(startPos, exitPos);
            _run = _grid.NextPoint(_pathid, ref _dst);
            if (_run) {

            }
        }
    }
}