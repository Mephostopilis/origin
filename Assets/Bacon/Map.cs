using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    public class Map
    {
        private Scene _scene = null;

        private GameObject _map = null;
        private AABB _aabb = null;

        public Map(Scene scene, GameObject o)
        {
            _scene = scene;

            _map = o;
            _aabb = new AABB(new Vector3(0, 0, 0), new Vector3(100, 100, 100));
        }

        public AABB AABB
        {
            get
            {
                return _aabb;
            }
        }
    }
}
