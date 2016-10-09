using UnityEngine;
using System.Collections;

public class MapBehaviour : MonoBehaviour {


    public GameObject _cube;
    public GameObject _sprite;

	// Use this for initialization
	void Start () {

        Vector3 sz = _cube.GetComponent<MeshFilter>().mesh.bounds.size;
        Sprite sprte = _sprite.GetComponent<SpriteRenderer>().sprite;
        Vector3 spritesz = sprte.bounds.size;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
