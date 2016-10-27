using UnityEngine;
using System.Collections;
using Bacon;
using UnityEngine.UI;

public class UIRootBehaviour : MonoBehaviour {

    public GameObject _root = null;
    public GameObject _join = null;
    public GameObject _ping = null;

    // Use this for initialization
    void Start() {
        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_SETUP_UIROOT, gameObject);
        var root = _root.GetComponent<RootBehaviour>();
        root.Application.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {
    }

    public void OnBorn() {
        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_ONBORN);
        var root = _root.GetComponent<RootBehaviour>();
        root.Application.Enqueue(cmd);
    }

    public void OnJoin() {
        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_ONJOIN);
        var root = _root.GetComponent<RootBehaviour>();
        root.Application.Enqueue(cmd);
    }

    public void disableJoin() {
        var com = _join.GetComponent<UnityEngine.UI.Button>();
    }

    public void ShowPing(int lag) {
        if (_ping != null) {
            var com = _ping.GetComponent<Text>();
            com.text = string.Format("ping: {0} ms", lag);
        }
    }
}
