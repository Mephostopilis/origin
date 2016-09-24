using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria
{
    public class Message
    {
        public delegate void Handler(object param);

        private Handler _handler = null;
        private object _param = null;

        public Message()
        {
        }

        public Message(Handler handler, object param)
        {
            _handler = handler;
            _param = param;
        }

        internal void execute()
        {
            _handler(_param);
        }
    }
}
