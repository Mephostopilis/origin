using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map { 
public class ObjectSampler : MonoBehaviour {

    public enum Type {
        Dynamic,
        Static,
    }

    protected Type _type = Type.Static;

    // Use this for initialization
    public void Start() {
        Grid.current.RegisterObjectSampler(this);
    }

    // Update is called once per frame
    public void Update() {

    }
}
}