using UnityEngine;
using System.Collections;

namespace sp
{
    public class worker_param
    {
        public delegate void worker_handler();

        private worker_handler _hander;
        private object _param;

        public worker_param(worker_handler handler, object param)
        {
            _hander = handler;
            _param = param;
        }

        public void execute()
        {
            _hander();
        }
    }
}

