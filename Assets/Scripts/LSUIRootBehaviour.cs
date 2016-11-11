using UnityEngine;
using Maria;

public class LSUIRootBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public GameObject _signupPanel = null;
    public GameObject _loginPanel = null;

    // Use this for initialization
    void Start() {
        _loginPanel.SetActive(true);
        _signupPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }
}
