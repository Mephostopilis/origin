using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Bacon {
    public class Map : Maria.Actor {
        private Scene _scene = null;
        private AABB _aabb = null;

        public Map(Context ctx, Controller controller, GameObject o, Scene scene) : base(ctx, controller, o) {
            _scene = scene;
            _aabb = new AABB(new Vector3(0, 0, 0), new Vector3(100, 0, 100));
        }

        public AABB AABB {
            get {
                return _aabb;
            }
        }
    }
}
