using System;
using System.Collections.Generic;
using System.Text;

namespace Maria {

    [XLua.LuaCallCSharp]
    public class EventDispatcher {

        protected Context _ctx;
        protected Dictionary<string, EventListenerCustom> _custom;
        protected Dictionary<string, List<EventListenerCustom>> _customSub;
        protected Dictionary<uint, EventListenerCmd> _cmd;

        public EventDispatcher(Context ctx) {
            _ctx = ctx;
            _custom = new Dictionary<string, EventListenerCustom>();
            _cmd = new Dictionary<uint, EventListenerCmd>();
        }

        public void AddCmdEventListener(EventListenerCmd listener) {
            uint cmd = listener.Cmd;
            _cmd[cmd] = listener;
        }

        public EventListenerCustom AddCustomEventListener(string eventName, EventListenerCustom.OnEventCustomHandler callback, object addition) {
            EventListenerCustom listener = null;
            if (_custom.ContainsKey(eventName)) {
                listener = _custom[eventName];
                if (listener == null) {
                    listener = new EventListenerCustom(eventName, callback, addition);
                    _custom[eventName] = listener;
                } else {
                    // remove 
                    _custom.Remove(eventName);
                    listener = new EventListenerCustom(eventName, callback, addition);
                    _custom[eventName] = listener;
                }
            } else {
                listener = new EventListenerCustom(eventName, callback, addition);
                _custom[eventName] = listener;
            }

            return listener;
        }

        public void DispatchCmdEvent(Command cmd) {
            try {
                if (_cmd.ContainsKey(cmd.Cmd)) {
                    EventListenerCmd listener = _cmd[cmd.Cmd];
                    if (listener.Enable) {
                        EventCmd e = new EventCmd(_ctx, cmd.Cmd, cmd.Orgin, cmd.Msg);
                        listener.Handler(e);
                    }
                } else {
                    throw new KeyNotFoundException(string.Format("custom not contains {0}", cmd.Cmd));
                }
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        private void DispatchCustomEvent(string eventName, object ud) {
            try {
                if (_custom.ContainsKey(eventName)) {
                    EventListenerCustom listener = _custom[eventName];
                    if (listener.Enable) {
                        EventCustom e = new EventCustom(_ctx, eventName, listener.Addition, ud);
                        listener.Handler(e);
                    }
                } else {
                    throw new KeyNotFoundException(string.Format("custom not contains {0}", eventName));
                }
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        public void FireCustomEvent(string eventName, object ud) {
            DispatchCustomEvent(eventName, ud);
        }

        public EventListenerCustom SubCustomEventListener(string eventName, EventListenerCustom.OnEventCustomHandler callback, object addition) {
            List<EventListenerCustom> li = null;
            if (_custom.ContainsKey(eventName)) {
                li = _customSub[eventName];
                var listener = new EventListenerCustom(eventName, callback, addition);
                li.Add(listener);
                return listener;
            } else {
                li = new List<EventListenerCustom>();
                var listener = new EventListenerCustom(eventName, callback, addition);
                li.Add(listener);
                _customSub[eventName] = li;
                return listener;
            }
        }

        public bool UnsubCustomEventListener(EventListenerCustom listener) {
            List<EventListenerCustom> li = null;
            if (_customSub.ContainsKey(listener.Name)) {
                li = _customSub[listener.Name];
                return li.Remove(listener);
            } else {
                return false;
            }
        }

        public void PubCustomEvent(string eventName, object ud) {
            try {
                if (_customSub.ContainsKey(eventName)) {
                    List<EventListenerCustom> li = _customSub[eventName];
                    if (li.Count > 0) {
                        for (int i = 0; i < li.Count; i++) {
                            EventListenerCustom listener = li[i];
                            if (listener.Enable) {
                                EventCustom e = new EventCustom(_ctx, eventName, listener.Addition, ud);
                                listener.Handler(e);
                            }
                        }
                    }
                } else {
                    throw new KeyNotFoundException(string.Format("custom not contains {0}", eventName));
                }
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }
    }
}
