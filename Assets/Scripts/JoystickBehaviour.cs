using UnityEngine;
using Bacon;
using Maria;

public class JoystickBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public ETCJoystick _joystick = null;

    private Context _ctx = null;
    private GameController _controller = null;

	// Use this for initialization
	void Start () {
        _joystick.anchor = ETCBase.RectAnchor.BottomLeft;
        //_joystick.enableKeySimulation = false;
        //_joystick.axisX.range


        _ctx = _root.App.AppContext;
        _controller = _ctx.Top() as GameController;
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
        _controller.OnMoveSpeed(s);
    }

    public void OnMoveEnd()
    {
        //Camera.current.WorldToScreenPoint();
    }

    public void OnPressUp() {
        _controller.OnPressUp();
    }

    public void OnPressRight() {
        _controller.OnPressRight();
    }

    public void OnPressDown() {
        _controller.OnPressDown();
    }

    public void OnPressLeft() {
        _controller.OnPressLeft();
    }
}
