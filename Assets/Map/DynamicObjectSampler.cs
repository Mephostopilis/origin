using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map {
    class DynamicObjectSampler : ObjectSampler {

        public Vector3 _start;
        public Vector3 _exit;

        private float _vel = 1;
        private Vector3 _dir = Vector3.one;

        // Use this for initialization
        void Start() {
            base.Start();
            _type = Type.Dynamic;

            int pathid = Grid.current.FindPath(_start, _exit);
            //Grid.current.
        }

        // Update is called once per frame
        void Update() {
            base.Update();
            Vector3 dist = _dir * _vel *Time.deltaTime;
            transform.localPosition = transform.localPosition + dist;
        }
    }
}