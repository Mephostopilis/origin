using UnityEngine;
using System.Collections;

namespace Maria.Util {
    public class DontDestroy : MonoBehaviour {

        // Use this for initialization
        void Start() {
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}