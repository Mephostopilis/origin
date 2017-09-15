using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DynamicObjectSampler : ObjectSampler {

    // Use this for initialization
    void Start() {
        base.Start();
        _type = Type.Dynamic;
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }
}

