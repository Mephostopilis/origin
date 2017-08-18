using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria {
    public sealed class Debug {

        private static bool _enabled = true;

        public static void Assert(bool condition, string message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Assert(condition, message, context);
        }

        public static void Assert(bool condition)
        {
            UnityEngine.Debug.Assert(condition);
        }

        public static void Log(object message) {
            if (_enabled)
            {
                UnityEngine.Debug.Log(message);
            }
        }

        public void Log(object message, UnityEngine.Object context) {
            if (_enabled)
            {
                UnityEngine.Debug.Log(message, context);
            }
        }

        public static void LogAssertion(object message)
        {
            if (_enabled)
            {
                UnityEngine.Debug.LogAssertion(message);
            }
        }

        public static void LogError(object message) {
            if (_enabled)
            {
                UnityEngine.Debug.LogError(message);
            }
        }

        public void LogError(object message, UnityEngine.Object context) {
            if (_enabled)
            {
                UnityEngine.Debug.LogError(message, context);
            }
        }
    }
}
