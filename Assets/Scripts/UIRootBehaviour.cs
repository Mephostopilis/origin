using UnityEngine;
using System.Collections;
//using Bacon;
using UnityEngine.UI;

public class UIRootBehaviour : MonoBehaviour {

    public GameObject _root = null;
    public GameObject _born = null;
    public GameObject _join = null;
    public GameObject _ping = null;

    // Use this for initialization
    void Start() {
        _born.SetActive(false);

        //Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_SETUP_UIROOT, gameObject);
        //var root = _root.GetComponent<RootBehaviour>();
        //root.App.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {
    }

    public void OnBorn() {
        _born.SetActive(false);
        //Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_ONBORN);
        //var root = _root.GetComponent<RootBehaviour>();
        //root.App.Enqueue(cmd);
    }

    public void OnJoin() {
        _join.SetActive(false);
        _born.SetActive(true);
        //Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_ONJOIN);
        //var root = _root.GetComponent<RootBehaviour>();
        //root.App.Enqueue(cmd);
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
