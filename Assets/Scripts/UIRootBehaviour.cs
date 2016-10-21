using UnityEngine;
using System.Collections;
using Bacon;

public class UIRootBehaviour : MonoBehaviour {

    public GameObject _root = null;
    public GameObject _join = null;

	// Use this for initialization
	void Start () {
        var root = _root.GetComponent<RootBehaviour>();
        AppContext ctx = root.App.AppContext;
        GameController ctr = ctx.Top() as GameController;
        ctr.SetupUI(gameObject);
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

    public void disableJoin() {
        var com = _join.GetComponent<UnityEngine.UI.Button>();
    }
}
