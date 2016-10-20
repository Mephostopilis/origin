using UnityEngine;
using System.Collections;
using Bacon;

public class BallBehaviour : MonoBehaviour {

    public GameObject _camera = null;

    private Vector3 _speed = Vector3.right;
    private Bacon.Ball _ball = null; 

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 shift = _speed * Time.deltaTime;
        transform.localPosition += shift;
        Matrix4x4 mat = Matrix4x4.TRS(shift, Quaternion.identity, Vector3.zero);
        var com = GetComponent<AABBBehaviour>();
        com.Transform(mat);

        if (_camera) {
            // 更新camera
        }
    }

    public Vector3 Speed { get { return _speed; } set { _speed = value; } }

    public GameObject Camera { get { return _camera; } set { _camera = value; } }

    public void SetupBall(Bacon.Ball ball) {
        _ball = ball;
    }

}
