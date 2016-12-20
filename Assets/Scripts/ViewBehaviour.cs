using UnityEngine;
using System.Collections;
//using Bacon;

public class ViewBehaviour : MonoBehaviour {

    //private View _view = null;
    //private AABB _aabb = null;
    private GameObject _mainCamera = null;

	// Use this for initialization
	void Start () {
        _mainCamera = transform.FindChild("Main Camera").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void UpdateView(Vector3 orgin, Vector3 target) {
        Vector3 forward = (target - orgin).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 up = Vector3.Cross(forward, right);
        Matrix4x4 mat = Matrix4x4.identity;
        mat.m00 = right.x;
        mat.m01 = right.y;
        mat.m02 = right.z;
        mat.m03 = orgin.x;
        mat.m10 = up.x;
        mat.m11 = up.y;
        mat.m12 = up.z;
        mat.m13 = orgin.y;
        mat.m20 = forward.x;
        mat.m21 = forward.y;
        mat.m22 = forward.z;
        mat.m23 = orgin.z;

        gameObject.transform.localPosition = orgin;
        //_go.transform.
    }

    //public void SetupView(View view) {
    //    _view = view;
    //}
}
