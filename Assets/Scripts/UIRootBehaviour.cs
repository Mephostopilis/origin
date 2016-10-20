using UnityEngine;
using System.Collections;
using Bacon;

public class UIRootBehaviour : MonoBehaviour {

    public GameObject _root = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnBorn()
    {
        var root = _root.GetComponent<RootBehaviour>();
        AppContext ctx = root.App.AppContext;
        ctx.SendReq<C2sProtocol.born>("born", null);
    }

    public void OnJoin()
    {
        var root = _root.GetComponent<RootBehaviour>();
        AppContext ctx = root.App.AppContext;
        ctx.AuthUdp(null);
    }
}
