using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour {

    public GameObject _target = null;
    public Camera _camera = null;
    
    public bool Enable { get; set; }

	// Use this for initialization
	void Start () {
        Enable = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Enable) {

        }
	}
}
