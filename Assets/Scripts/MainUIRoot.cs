using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Bacon;
using Maria;

public class MainUIRoot : MonoBehaviour {

    public RootBehaviour _root;
    public Button _match;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMatch() {
        _match.enabled = false;

        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_MUI_MATCH);
        _root.App.Enqueue(cmd);
    }
}
