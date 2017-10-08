using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Bacon.GL.Game {

    public class Animatii : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, 0);
                RaycastHit hitInfo;
                if (Bacon.GL.Util.Utils.Raycast(ray, 1000000, plane, out hitInfo)) {
                    Vector3 worldPos = hitInfo.point;
                    GetComponent<DynamicObjectSampler>().FindPath(worldPos);
                }

                //Physics.Raycast()
                //int layerMask = LayerMask.GetMask("Grid");
                //RaycastHit hitInfo;
                //if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)) {
                //    Vector3 worldPos = hitInfo.point;
                //    GetComponent<DynamicObjectSampler>().FindPath(worldPos);
                //}
            }
        }
    }
}