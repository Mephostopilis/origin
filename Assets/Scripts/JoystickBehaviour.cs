using UnityEngine;
using System.Collections;
using Bacon;
using Maria;

public class JoystickBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    private Context _ctx = null;
    private GameController _controller = null;

	// Use this for initialization
	void Start () {
        _ctx = _root.App.AppContext;
        _controller = _ctx.GetCurController() as GameController;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMoveStart()
    {
        _controller.OnMoveStart();
    }

    public void OnMove(Vector2 d)
    {
        _controller.OnMove(d);
    }

    public void OnMoveSpeed(Vector2 s)
    {
    }

    public void OnMoveEnd()
    {
        //Camera.current.WorldToScreenPoint();

    }
}
