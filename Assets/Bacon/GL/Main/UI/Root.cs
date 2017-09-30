using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon.GL.Main.UI { 
public class Root : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMatch() {
        Maria.Command cmd = new Maria.Command(Bacon.Event.MyEventCmd.EVENT_MUI_MATCH);
        Bacon.GL.Util.App.current.Enqueue(cmd);
    }
}
}