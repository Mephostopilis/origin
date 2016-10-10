using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class Rect
    {
        public Vector3 min;
        public Vector3 max;

        public Rect(Vector3 n, Vector3 x)
        {
            min = n;
            max = x;
        }


    }
}
