using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Maria
{
    public class Context
    {
        private Thread _worker = null;
        private Queue<Message> _queue = new Queue<Message>();

        public Context()
        {
            _worker = new Thread(new ThreadStart(Worker));
            _worker.Start();
        }

        // Use this for initialization
        public void Start()
        {

        }

        // Update is called once per frame
        public void Update(float delta)
        {
        }

        public void Enqueue(Message msg)
        {
            lock (_queue)
            {
                _queue.Enqueue(msg);
            }
        }

        private void Worker()
        {
            while (true)
            {
                Message msg = null;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        msg = _queue.Dequeue();
                    }
                }
                if (msg != null)
                {
                    msg.execute();
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}

