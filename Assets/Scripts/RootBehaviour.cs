using UnityEngine;
using System.Collections;
using Maria;

public class RootBehaviour : MonoBehaviour {

    private Context _ctx;
    private Controller _controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnEnter(Context ctx, Controller ctr)
    {
        _ctx = ctx;
        _controller = ctr;
    }
}
