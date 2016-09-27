using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;

namespace Maria
{
    public class Context
    {
        protected Thread _worker = null;
        protected Queue<Message> _queue = new Queue<Message>();
        protected Dictionary<string, Controller> _hash = new Dictionary<string, Controller>();
        protected ClientLogin _login = null;
        protected ClientSocket _client = null;
        protected Gate _gate = null;

        public Context()
        {
            _worker = new Thread(new ThreadStart(Worker));
            _worker.Start();

            _gate = new Gate(this);

            _login = new ClientLogin(this);
            _client = new ClientSocket(this);
        }

        // Use this for initialization
        public void Start()
        {
            
        }

        // Update is called once per frame
        public void Update(float delta)
        {
            _login.Update();
            _client.Update();
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
                _gate.Run();

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

        public T GetController<T>(string name) where T : Controller
        {
            return (T)_hash[name];
        }

        public void SendReq<T>(String callback, SprotoTypeBase obj)
        {
            _client.SendReq<T>(callback, obj);
        }

        public void Auth()
        {

        }
    }
}

