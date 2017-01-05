using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Maria;

public class LoginPanelBehaviour : MonoBehaviour {


    public RootBehaviour _root = null;
    public LSUIRootBehaviour _uiroot = null;
    public InputField _usernmIF = null;
    public InputField _passwdIF = null;
    public Button _ok = null;

    private string _server = null;
    private string _username = null;
    private string _password = null;
    private bool _commit = false;

    // Use this for initialization
    void Start () {
        Maria.Command cmd = new Command(EventCmd.EVENT_SETUP_LOGINPANEL, gameObject);
        _root.App.Enqueue(cmd);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public string GetUsername()
    {
        return _usernmIF.GetComponent<InputField>().text;
    }

    public string GetPassword()
    {
        return _passwdIF.GetComponent<InputField>().text;
    }

    public void OnLoginCommit()
    {
        if (!_commit)
        {
            _commit = true;
            _ok.gameObject.SetActive(false);
            _username = GetUsername();
            _password = GetPassword();
            if (_username.Length < 4)
            {
                Debug.Log("you should have more lenth.");
                return;
            }
            if (_password.Length < 4)
            {
                return;
            }
            _server = "sample";

            Maria.Message msg = new Message();
            msg["username"] = _username;
            msg["password"] = _password;
            msg["server"] = _server;

            Maria.Command cmd = new Command(Bacon.MyEventCmd.EVENT_LOGIN, gameObject, msg);
            _root.App.Enqueue(cmd);
        }
    }

    public void EnableCommitOk() {
        _commit = false;
        _ok.gameObject.SetActive(true);
    }

    public void OnUserValueChanged(string v) {

    }

    public void OnPwdValueChanged(string v) {

    }

    public void OnUserEndEdit(string v) {

    }

    public void OnPwdEndEdit(string v) {

    }
}
