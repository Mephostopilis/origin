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
                int layerMask = 1 << 12;
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 1000, layerMask)) {
                    Vector3 worldPos = hitInfo.point;
                    GetComponent<DynamicObjectSampler>().FindPath(worldPos);
                }
            }
        }
    }
}