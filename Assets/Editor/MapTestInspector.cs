using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map;

[CustomEditor(typeof(MapTest))]
public class MapTestInspector : Editor {

    private bool _run = true;

    void OnSceneGUI() {
        if (!_run) {
            return;
        }
        MapTest test = target as MapTest;
        Event e = Event.current;
        if (e.isMouse && e.control) {
            switch (e.keyCode) {
                case KeyCode.A: {
                        Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        int layermask = 1 << 12;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(r, out hitInfo, Mathf.Infinity, layermask)) {
                            hitInfo.collider.gameObject.GetComponent<Tile>().ChooseTileState(Tile.TileState.Block);
                            EditorGUIUtility.PingObject(hitInfo.collider.gameObject);
                        }
                    }
                    break;
                case KeyCode.D: {
                        Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        int layermask = 1 << 12;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(r, out hitInfo, Mathf.Infinity, layermask)) {
                            hitInfo.collider.gameObject.GetComponent<Tile>().ChooseTileState(Tile.TileState.Block);
                            EditorGUIUtility.PingObject(hitInfo.collider.gameObject);
                        }
                    }
                    break;
                case KeyCode.B: {
                        Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        int layermask = 1 << 12;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(r, out hitInfo, Mathf.Infinity, layermask)) {
                            hitInfo.collider.gameObject.GetComponent<Tile>().ChooseTileState(Tile.TileState.Block);
                            EditorGUIUtility.PingObject(hitInfo.collider.gameObject);
                        }
                    }
                    break;
                case KeyCode.T: {
                        Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        int layermask = 1 << 12;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(r, out hitInfo, Mathf.Infinity, layermask)) {
                            hitInfo.collider.gameObject.GetComponent<Tile>().ChooseTileState(Tile.TileState.Tree);
                            EditorGUIUtility.PingObject(hitInfo.collider.gameObject);
                        }
                    }
                    break;
                default:
                    break;
            }
        } else if (e.isMouse) {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            int layerMask = 1 << 12;
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000, layerMask)) {
                Vector3 worldPos = hitInfo.point;
                Tile tile = test.grid.ToTile(worldPos);
                if (tile) {
                    EditorGUIUtility.PingObject(tile.gameObject);
                }
            }
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        MapTest test = target as MapTest;

        if (GUILayout.Button("Test Grid")) {
            EditorGUILayout.TextField(string.Format("grids num: {0}", test.grid.Tiles.Count));
            UnityEngine.Debug.Log(string.Format("grids num: {0}", test.grid.Tiles.Count));
        }

        _run = EditorGUILayout.Toggle("Enable Grid Editor", _run);

    }
}
