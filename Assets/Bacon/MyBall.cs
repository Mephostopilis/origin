using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class MyBall : Ball {
        public MyBall(Scene scene, GameObject o, float radis, float length, float width, float height) : base(scene, o, radis, length, width, height) {
            View v = scene.View;
            GameObject go = v.Go;
            var com = o.GetComponent<BallBehaviour>();
            com.Camera = go;
        }
    }
}
