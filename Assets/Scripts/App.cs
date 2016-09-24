using UnityEngine;
using System.Collections;
using Maria.App;

public class App : MonoBehaviour {

    private AppContext _ctx = new AppContext();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.deltaTime;
        _ctx.Update(deltaTime);
	}
}
