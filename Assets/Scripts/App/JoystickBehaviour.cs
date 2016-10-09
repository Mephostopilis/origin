using UnityEngine;
using System.Collections;
using Bacon;

public class JoystickBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public GameObject _ball = null;
    public GameObject _camera = null;
    public GameObject _light = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMoveStart()
    {
    }

    public void OnMove(Vector2 d)
    {
        //AppContext ctx = _root.App.AppContext;

    }

    public void OnMoveSpeed(Vector2 s)
    {
    }

    public void OnMoveEnd()
    {
        //Camera.current.WorldToScreenPoint();

    }

    

}
