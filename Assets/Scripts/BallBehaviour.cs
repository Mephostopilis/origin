using UnityEngine;
using System.Collections;
using Bacon;

public class BallBehaviour : MonoBehaviour {

    public GameObject _camera = null;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (_camera) {
            // 更新camera
        }
    }

    public GameObject Camera { get { return _camera; } set { _camera = value; } }

}
