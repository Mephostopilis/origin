using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maria.Util {
    public class SetMeshRendererSortingLayer : MonoBehaviour {

        public string _sortingLayerName = string.Empty;
        public int _sortingOrder = 0;

        // Use this for initialization
        void Start() {
            if (_sortingLayerName != null && _sortingLayerName != string.Empty) {
                var mr = GetComponent<MeshRenderer>();
                mr.sortingLayerName = _sortingLayerName;
                mr.sortingOrder = _sortingOrder;
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}

