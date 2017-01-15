using UnityEngine;
using System.Collections;
//using Bacon;

public class GameRootBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;
    public Camera _view = null;
    public GameObject _map = null;

	// Use this for initialization
	void Start () {
        Debug.Assert(_root != null);
        Debug.Assert(_view != null);
        Debug.Assert(_map != null);
        transform.position = new Vector3(100, 0.0f, 100);
        //var com = _root.GetComponent<RootBehaviour>();
        //Maria.Command cmd1 = new Maria.Command(MyEventCmd.EVENT_SETUP_SCENE, gameObject);
        //com.App.Enqueue(cmd1);

        //Maria.Command cmd2 = new Maria.Command(MyEventCmd.EVENT_SETUP_VIEW, _view);
        //com.App.Enqueue(cmd2);

        //Maria.Command cmd3 = new Maria.Command(MyEventCmd.EVENT_SETUP_MAP, _map);
        //com.App.Enqueue(cmd3);


	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
