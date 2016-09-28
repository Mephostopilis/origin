using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;
using UnityEngine;

namespace Maria.App
{
    class LoginController : Controller
    {
        private GameObject _scene = null;

        public LoginController(Context ctx) : base(ctx)
        {
        }

        public void Enter()
        {
            _scene = GameObject.Find("Root");
        }
        
        public void Login(string username, string password)
        {
            //C2sSprotoType.role_info.request requestObj = new C2sSprotoType.role_info.request();
            //requestObj.role_id = 1;
            //_ctx.SendReq<C2sSprotoType.role_info>("role_info", requestObj);
            LoginCallback();
        }

        public void LoginCallback()
        {
            //var go = GameObject.Find("LoginScene");
            //var co = go.GetComponent<LoginSceneBehaviourScript>();
            //co.CloseLogin();
        }
    }
}
