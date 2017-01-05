using Bacon;
using Maria;
using UnityEngine;
using System.Collections.Generic;

public class App : MonoBehaviour {

    public StartBehaviour _start = null;
    private Bacon.App _app = null;
    
    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this);
        _app = new Bacon.App(this);
        _start.SetupStartRoot();
    }

    // Update is called once per frame
    void Update() {
        try {
            _app.Update();
        } catch (System.Exception ex) {
            Debug.LogError(string.Format("ex message: {0}", ex.Message));
            Debug.LogError(ex.StackTrace);
        }
    }

    void OnApplicationFocus(bool isFocus) {
        if (_app != null) {
            _app.OnApplicationFocus(isFocus);
        }
    }

    void OnApplicationPause(bool isPause) {
        if (_app != null) {
            _app.OnApplicationPause(isPause);
        }
    }

    void OnApplicationQuit() {
        if (_app != null) {
            _app.OnApplicationQuit();
        }
    }

    public void Enqueue(Command cmd) {
        _app.Enqueue(cmd);
    }

}
