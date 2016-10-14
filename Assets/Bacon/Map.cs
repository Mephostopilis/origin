using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon
{
    class Map
    {
        private GameObject _map = null;
        private AABB _aabb = null;

        public Map(GameObject o)
        {
            _map = o;
            _aabb = new AABB(new Vector3(0, 0, 0), new Vector3(100, 100, 100));
        }

        public void Start()
        {

        }

        public void Close()
        {

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
