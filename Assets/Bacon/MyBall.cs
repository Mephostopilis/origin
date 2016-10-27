using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Bacon {
    class MyBall : Ball {

        public MyBall(Context ctx, Controller controller, Scene scene, float radis, float length, float width, float height)
            : base(ctx, controller, null, scene, radis, length, width, height) {
        }

        public MyBall(Context ctx, Controller controller, GameObject go, Scene scene, float radis, float length, float width, float height)
            : base(ctx, controller, go, scene, radis, length, width, height) {

        }
    }
}
