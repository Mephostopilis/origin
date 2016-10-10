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
        private Vector3 _min = new Vector3(0, 0, 0);
        private Vector3 _max = new Vector3(1000, 1000, 0);

        public Map(GameObject o)
        {
            _map = o;
            _map.transform.localPosition = Vector3.zero;
        }

        public void Start()
        {

        }

        public void Close()
        {

        }

        // 边界碰撞在本地
        public void CheckBorder()
        {
        }

        public MyBall Born(float radis, Vector3 position, Vector3 direction)
        {

            return null;
        }
    }
}
