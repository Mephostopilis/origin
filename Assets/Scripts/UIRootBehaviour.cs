using UnityEngine;
using System.Collections;
using Bacon;

public class UIRootBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBorn()
    {
        AppContext ctx = _root.App.AppContext;
        ctx.SendReq<C2sProtocol.born>("born", null);
    }

    public void OnTest()
    {
        //AppContext ctx = _root.App.AppContext;
        //ctx.SendReq<C2sProtocol.test>("test", null);
    }

    public void OnJoin()
    {
        AppContext ctx = _root.App.AppContext;
        ctx.AuthUdp(null);
    }
}
