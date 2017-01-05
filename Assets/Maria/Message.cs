using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Maria
{
    public class Message
    {
        private Hashtable _hash = new Hashtable();

        public Message()
        {
        }

        public object this[string index] {
            get {
                if (_hash.Contains(index)) {
                    return _hash[index];
                } else {
                    throw new Exception("");
                }
            }
            set { _hash[index] = value; }
        }
    }
}
