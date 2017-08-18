using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maria.Util { 
    public class FindApp : MonoBehaviour {

        private App _app = null;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        protected App InitApp() {
            if (_app == null) {
                _app = GameObject.Find("App").GetComponent<App>();
                if (_app == null) {
                    UnityEngine.Debug.Assert(false, "why ");
                    return null;
                } else {
                    return _app;
                }
            } else {
                return _app;
            }
        }

        public App App {
            get {
                return InitApp();
            }
            set {
                InitApp();
            }
        }
    }
}