using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Maria;

public class LoginPanelBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public GameObject _uiroot = null;
    public GameObject _usernmIF = null;
    public GameObject _passwdIF = null;

    private string _server = null;
    private string _username = null;
    private string _password = null;

    // Use this for initialization
    void Start () {
	
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
        
        _username = GetUsername();
        _password = GetPassword();
        if (_username.Length < 4)
        {
            Debug.Log("you should have more lenth.");
            return;
        }
        if (_password.Length < 3)
        {
            return;
        }
        _server = "sample";
        Context ctx = _root.App.AppContext;

        LoginController ctr = ctx.GetController<LoginController>("login");
        ctr.Auth(_server, _username, _password);
    }
}
