using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map;

public class SceneViewEditorWindow : EditorWindow {

    static SceneViewEditorWindow _instance;

    [MenuItem("Tools/Scene View Editor")]
	static void OnMenu() {
        if (_instance == null) {
            _instance = EditorWindow.GetWindow<SceneViewEditorWindow>();
            SceneView.onSceneGUIDelegate += OnSceneFunc;
        }
    }

    public static void OnSceneFunc(SceneView sceneView) {
        _instance.CustomeSceneGui(sceneView);
    }

    private int _index = 0;

    private void OnEnable() {
    }

    private void OnDisable() {
    }

    private void OnDestroy() {
        UnityEngine.Debug.Log("destroy");
        SceneView.onSceneGUIDelegate -= OnSceneFunc;
    }

    public void CustomeSceneGui(SceneView sceneView) {
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
                case KeyCode.B: 
                    {
                        Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        int layermask = 1 << 12;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(r, out hitInfo, Mathf.Infinity, layermask)) {
                            hitInfo.collider.gameObject.GetComponent<Tile>().ChooseTileState(Tile.TileState.Block);
                            EditorGUIUtility.PingObject(hitInfo.collider.gameObject);
                        }
                    }
                    break;
                case KeyCode.T: 
                    {
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
        }
    }
}
