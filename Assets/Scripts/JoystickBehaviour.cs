﻿using UnityEngine;
using Bacon;
using Maria;

public class JoystickBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public ETCJoystick _joystick = null;

    //private Context _ctx = null;
    //private GameController _controller = null;

	// Use this for initialization
	void Start () {
        _joystick.anchor = ETCBase.RectAnchor.BottomLeft;
        //_joystick.enableKeySimulation = false;
        //_joystick.axisX.range
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnMoveStart()
    {
        //_controller.OnMoveStart();

    }

    public void OnMove(Vector2 d)
    {
        //_controller.OnMove(d);
    }

    public void OnMoveSpeed(Vector2 s)
    {
        //_controller.OnMoveSpeed(s);
    }

    public void OnMoveEnd()
    {
        //Camera.current.WorldToScreenPoint();
    }

    public void OnPressUp() {

        Maria.Command cmd = new Command(MyEventCmd.EVENT_PRESSUP);
        _root.App.Enqueue(cmd);
    }

    public void OnPressRight() {
        Maria.Command cmd = new Command(MyEventCmd.EVENT_PRESSRIGHT);
        _root.App.Enqueue(cmd);
    }

    public void OnPressDown() {
        Maria.Command cmd = new Command(MyEventCmd.EVENT_PRESSDOWN);
        _root.App.Enqueue(cmd);
    }

    public void OnPressLeft() {
        Maria.Command cmd = new Command(MyEventCmd.EVENT_PRESSLEFT);
        _root.App.Enqueue(cmd);
    }
}
