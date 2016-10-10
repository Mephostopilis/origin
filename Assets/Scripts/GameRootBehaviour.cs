using UnityEngine;
using System.Collections;
using Bacon;

public class GameRootBehaviour : MonoBehaviour {

    public GameObject _root = null;
    public GameObject _mainCamera = null;
    public GameObject _map = null;
    public GameObject _ball = null;

	// Use this for initialization
	void Start () {
        var com = _root.GetComponent<RootBehaviour>();
        GameController controller = com.Controller as GameController;

        var camera = _mainCamera.GetComponent<Camera>();
        controller.SetupCamera(camera);

        controller.SetupMap(_map);
        controller.SetupMyBall(_ball);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Camera GetCamera()
    {
        Debug.Assert(_mainCamera != null);
        if (_mainCamera != null)
        {
            return _mainCamera.GetComponent<Camera>();
        }
        else
        {
            return null;
        }
    }
}
