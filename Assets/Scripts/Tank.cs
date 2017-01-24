using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

    private bool _moved = false;
    private float _vel = 1.2f;
    private Vector3 _dir = Vector3.one;


    // Use this for initialization
    void Start() {
        _vel = 1.2f;
    }

    // Update is called once per frame
    void Update() {
        if (_moved) {
            Vector3 dis = _dir * _vel * Time.deltaTime;
            //transform.localPosition += dis;
            transform.Translate(dis);

            int a = 3;

            //_vel * Time
        }
    }

    public void Move(float rad)  {
        _dir.x = Mathf.Cos(rad);
        _dir.y = 0.0f;
        _dir.z = Mathf.Sin(rad);
        Quaternion rod = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(rad * Mathf.Rad2Deg, Vector3.up);

        
        _moved = true;
    }

    public void Stop() {
        _moved = false;
    }
}
