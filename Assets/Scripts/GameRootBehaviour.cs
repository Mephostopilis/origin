using UnityEngine;
using System.Collections;
using Bacon;

public class GameRootBehaviour : MonoBehaviour {

    public GameObject _root = null;
    public GameObject _view = null;
    public GameObject _map = null;

	// Use this for initialization
	void Start () {
        Debug.Assert(_root != null);
        Debug.Assert(_view != null);
        Debug.Assert(_map != null);

        var com = _root.GetComponent<RootBehaviour>();
        GameController controller = com.Controller as GameController;

        controller.SetupScene(gameObject);
        controller.SetupCamera(_view);
        controller.SetupMap(_map);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
