using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    public class CommandQueue {
        protected Queue<Command> _queue = new Queue<Command>();

        public CommandQueue() {
        }

        public int Count { get { return _queue.Count; } }

        public bool Contains(Command cmd) {
            return _queue.Contains(cmd);
        }

        public Command Dequeue() {
            Command cmd = null;
            lock (_queue) {
                if (_queue.Count > 0) {
                    cmd = _queue.Dequeue();
                }
            }
            return cmd;
        }

        public void Enqueue(Command cmd) {
            lock (_queue) {
                _queue.Enqueue(cmd);
            }
        }

        public Command Peek() {
            return _queue.Peek();
        }
    }
}
