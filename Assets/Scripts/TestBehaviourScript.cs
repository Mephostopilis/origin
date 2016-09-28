using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        float delta = Time.deltaTime;
        Debug.Log(delta);
	}

    public void OnClick()
    {
        SceneManager.LoadScene("2");
    }
}
