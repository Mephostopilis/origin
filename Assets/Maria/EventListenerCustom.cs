using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {
    public class EventListenerCustom : EventListener {

        public delegate void OnEventCustomHandler(EventCustom e);

        private string _name;
        private OnEventCustomHandler _callback;

        public EventListenerCustom(string name, OnEventCustomHandler callback) {
            _name = name;
            _callback = callback;
        }

        public OnEventCustomHandler Handler { get { return _callback; } }

    }
}
