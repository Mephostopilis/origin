using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using Maria.Network;
using Maria.Encrypt;

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
    public GameObject startPanel;
    public GameObject signupPanel;
    public GameObject loginPanel;

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
        startPanel.SetActive(false);
        signupPanel.SetActive(false);
    }

    public void OnLoginCommit()
    {
        account = GameObject.Find("AccountIF").GetComponent<InputField>().text;
        password = GameObject.Find("PwdIF").GetComponent<InputField>().text;
        if (account.Length < 6)
        {
            Debug.Log("you shoud give me a");
            return;
        }
        if (password.Length < 6)
        {
            Debug.Log("you should give me a.");
            return;
        }
        if (login != null)
        {
            login = GameObject.Find("Login").GetComponent<ClientLogin>();
        }
        string ip = "192.168.1.239";
        int port = 3002;
        login.Auth(ip, port, "sample", account, password, null, OnLoginCallback);
    }

    public void OnLoginCallback(bool ok, object ud, byte[] secret, string dummy)
    {
        if (ok)
        {
            int _1 = dummy.IndexOf(':');
            int _2 = dummy.IndexOf('@', _1);
            int _3 = dummy.IndexOf('#', _2);
            string uen = dummy.Substring(_1 + 1, _2 - _1 - 1);
            byte[] uid = Crypt.base64decode(Encoding.ASCII.GetBytes(uen));
            string sen = dummy.Substring(_2+1, _3-_2-1);
            byte[] subid = Crypt.base64decode(Encoding.ASCII.GetBytes(sen));
            string gated = dummy.Substring(_3 + 1);
            
            Debug.Log(string.Format("{0},{1}", uid, subid));
            Debug.Log("login");
            var s = gated.Split(':');
            string ip = s[0];
            int port = int.Parse(s[1]);
            u = new User();
            u.Server = "sample";
            u.Account = account;
            u.Password = password;
            u.Secret = secret;
            u.Uid = uid;
            u.Subid = subid;
            var client = GameObject.Find("Agent").GetComponent<ClientSocket>();
            client.Auth(ip, port, u, null, OnAuthCallback);
        }
        else
        {
            Debug.Log("auth failture");
        }
    }

    public void OnAuthCallback(bool ok, object ud, byte[] subid, byte[] secret)
    {
        if (ok)
        {
            Debug.Log("auth success.");
        }
        else
        {

        }
    }

    public void OnSignupBtnClick()
    {
        startPanel.SetActive(false);
        loginPanel.SetActive(false);
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

    public void OnSignupCallback(bool ok, object ud, byte[] secret, string dummy)
    {
        if (ok)
        {
            startPanel.SetActive(true);
            loginPanel.SetActive(true);
        }
        else
        {
            Debug.Log("please enter your username.");
            GameObject.Find("AccountIF").GetComponent<InputField>().text = string.Empty;
            GameObject.Find("PwdIF").GetComponent<InputField>().text = string.Empty;
            GameObject.Find("CfmPwdIF").GetComponent<InputField>().text = string.Empty;
        }
    }
}
