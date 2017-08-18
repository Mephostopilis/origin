using UnityEngine;
using UnityEngine.UI;
using Bacon.Event;
using Bacon.GL.Common;
using Maria.Util;

namespace Bacon.GL.Login.UI {
    public class LoginPanel : MonoBehaviour {

        public InputField _usernmIF = null;
        public InputField _passwdIF = null;
        public Button _ok = null;
        public Text _Tips;

        private string _server = null;
        private string _username = null;
        private string _password = null;
        private bool _commit = false;

        // Use this for initialization
        void Start() {
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_SETUP_LOGINPANEL, gameObject);
            GetComponent<FindApp>().App.Enqueue(cmd);
            Maria.Util.App.current.AddObserver("AndroidLogin", OnAnroidLogin);
        }

        // Update is called once per frame
        void Update() {

        }

        void OnEnable() {
            _commit = false;
        }

        public string GetUsername() {
            return _usernmIF.GetComponent<InputField>().text;
        }

        public string GetPassword() {
            return _passwdIF.GetComponent<InputField>().text;
        }

        public void EnableCommitOk(bool ok) {
            _commit = ok;
        }

        public void OnUserValueChanged(string v) {

        }

        public void OnPwdValueChanged(string v) {

        }

        public void OnUserEndEdit(string v) {

        }

        public void OnPwdEndEdit(string v) {

        }


        public void OnLogin() {
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable) {
                _Tips.text = "当前没有网络，请链接后再试";
                return;
            } else {
                if (UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
                    _Tips.text = "你正在使用个移动网路";
                } else if (UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
                    _Tips.text = "你正在是用wifi";
                }
            }

            if (!_commit) {
                _commit = true;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                string code = string.Format("{0}", Random.Range(100, 10000));
                Login(code);
#elif UNITY_IOS || UNITY_ANDROID
        try {
            AndroidJavaClass c = new AndroidJavaClass("com.emberfarkas.mahjong.wxapi.WXEntryActivity");
            AndroidJavaObject o = c.GetStatic<AndroidJavaObject>("currentWXActivity");
            o.Call("login");
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogException(ex);
        }
#endif
            }
        }

        void Login(string code) {
            UnityEngine.Debug.Log(code);

            Maria.Message msg = new Maria.Message();
            msg["username"] = code;
            msg["password"] = "Password";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            msg["server"] = "sample1";
#elif UNITY_IOS || UNITY_ANDROID
        msg["server"] = "sample";
#endif
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_LOGIN, gameObject, msg);
            GetComponent<FindApp>().App.Enqueue(cmd);
        }

        public void ShowTips(string tips) {
            _Tips.text = tips;
        }

        public void OnAnroidLogin(Maria.Util.App.Notification n) {
            string code = n.data.ToString();
            Login(code);
        }

    }
}