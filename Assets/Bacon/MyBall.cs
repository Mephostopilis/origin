using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class MyBall : Ball
    {
        public MyBall(GameObject o, float radis) : base(o, radis)
        {
        }

        public static Ball Create()
        {
            return null;
        }
    }
}
