using UnityEngine;
using System.Collections;

public class StartBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetupStartRoot() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, gameObject);
        _root.App.Enqueue(cmd);
    }
}
