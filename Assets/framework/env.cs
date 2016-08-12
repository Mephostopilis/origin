using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace sp
{
    public class env
    {
        private Thread _worker = null;
        private Queue<worker_param> _queue = new Queue<worker_param>();

        public env()
        {
            _worker = new Thread(new ThreadStart(worker));
            _worker.Start();
        }

        // Use this for initialization
        void start()
        {

        }

        // Update is called once per frame
        void update()
        {

        }

        public void enqueue(worker_param param)
        {
            lock (_queue)
            {
                _queue.Enqueue(param);
            }
        }

        private void worker()
        {
            while (true)
            {
                worker_param param = null;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        param = _queue.Dequeue();
                    }
                }
                if (param != null)
                {
                    param.execute();
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}

