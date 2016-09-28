using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginPanelBehaviour : MonoBehaviour {

    public GameObject _usernmIF;
    public GameObject _passwdIF;

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
}
