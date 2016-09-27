using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Maria.Network
{
    public class ProxySocket
    {
        private Gate _gate = null;

        public ProxySocket(Gate g)
        {
            _gate = g;
        }

        // Update is called once per frame
        void Update()
        {
        }

    }
}