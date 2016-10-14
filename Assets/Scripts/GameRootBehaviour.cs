using UnityEngine;
using System.Collections;
using Bacon;

public class GameRootBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public GameObject _mainCamera = null;
    public GameObject _map = null;

	// Use this for initialization
	void Start () {
        var com = _root.GetComponent<RootBehaviour>();
        GameController controller = com.Controller as GameController;

        controller.SetupMap(_map);
        
        var camera = _mainCamera.GetComponent<Camera>();
        controller.SetupCamera(camera);

        controller.SetupScene(gameObject);
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
