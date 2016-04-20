using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanelController : MonoBehaviour
{

    private enum State
    {
        SNone,
        SLogin,
        SSignup,
    }

    private State state = State.SNone;
    private string account;
    private string password;
    private ClientLogin login;
    private User u;

    // Use this for initialization
    void Start()
    {
        login = GameObject.Find("Login").GetComponent<ClientLogin>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLoginBtnClick()
    {
        var g = GameObject.Find("Start");
        g.SetActive(false);
        g = GameObject.Find("SignupPanel");
        g.SetActive(false);
    }

    public void OnLoginCommit()
    {
        account = GameObject.Find("AccountIFText").GetComponent<Text>().text;
        password = GameObject.Find("PwdIFText").GetComponent<Text>().text;
        Debug.Assert(account.Length > 6);
        Debug.Assert(password.Length > 6);
        if (login != null)
        {
            login = GameObject.Find("Login").GetComponent<ClientLogin>();
        }
        string ip = "192.168.1.239";
        int port = 3002;
        login.Auth(ip, port, "sample", account, password, null, OnLoginCallback);
    }

    public void OnLoginCallback(bool ok, object ud, byte[] subid, byte[] secret)
    {
        if (ok)
        {
            string ip = "192.168.1.239";
            int port = 8888;
            u = new User();
            u.Server = "sample";
            u.Account = account;
            u.Password = password;
            u.Secret = secret;
            u.Subid = subid;
            var client = GameObject.Find("Agent").GetComponent<ClientSocket>();
            client.Auth(ip, port, u, null, OnAuthCallback);
        }
        else
        {

        }
    }

    public void OnAuthCallback(bool ok, object ud, byte[] subid, byte[] secret)
    {
    }

    public void OnSignupBtnClick()
    {
        state = State.SSignup;
        var g = GameObject.Find("Start");
        g.SetActive(false);
        g = GameObject.Find("LoginPanel");
        g.SetActive(false);
    }

    public void OnSignupCommit()
    {
        var g = GameObject.Find("AccountIFText");
        var c = g.GetComponent<Text>();
        account = c.text;
        password = GameObject.Find("PwdIFText").GetComponent<Text>().text;
        var cfmPassword = GameObject.Find("CfmPwdIFText").GetComponent<Text>().text;
        if (password != cfmPassword)
        {
            account = null;
            password = null;
        }
        else
        {
            if (login != null)
            {
                login = GameObject.Find("Login").GetComponent<ClientLogin>();
            }
            string ip = "192.168.1.239";
            int port = 3001;
            login.Auth(ip, port, "sample", account, password, null, OnSignupCallback);
        }
    }

    public void OnSignupCallback(bool ok, object ud, byte[] subid, byte[] secret)
    {
        if (ok)
        {

        }
        else
        {

        }
    }
}
