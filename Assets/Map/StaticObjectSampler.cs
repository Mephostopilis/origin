using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map { 
public class StaticObjectSampler : ObjectSampler {

	// Use this for initialization
	void Start () {
        base.Start();
        _type = Type.Static;
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
}