using UnityEngine;
using System.Collections;

namespace Maria
{
    public class WorkerParam
    {
        public delegate void worker_handler();

        private worker_handler _hander;
        private object _param;

        public WorkerParam(worker_handler handler, object param)
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

