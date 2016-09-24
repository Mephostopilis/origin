using UnityEngine;
using System.Collections;
using Maria.App;

public class LoginSceneBehaviourScript : MonoBehaviour {

    public App _app;
    private string _username;
    private string _password;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnUsernmIFChanged(string value)
    {
        _username = value;
    }

    public void OnPasswdIFChanged(string value)
    {
        _password = value;
    }

    public void OnOkClick()
    {
        var controller = _app.GetController<LoginController>("login");
        controller.Login(_username, _password);
    }

    public void CloseLogin()
    {
        var go = GameObject.Find("LoginPanel");
        go.SetActive(false);
    }
}
