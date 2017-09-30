using UnityEditor;
using UnityEngine;
using System.Collections;
using Map;

[CustomEditor(typeof(Grid))]
public class GridInspector : Editor {

    void OnEnable() {
        Grid grid = target as Grid;
        grid.Tiles.Clear();
        for (int i = 0; i < grid.transform.childCount; i++) {
            Transform t = grid.transform.GetChild(i);
            Tile tile = t.GetComponent<Tile>();
            grid.Tiles.Add(tile.index.ToString(), tile);
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Grid grid = target as Grid;

        if (GUILayout.Button("Generate Hex Grid")) {
            grid.transform.localPosition = Vector3.zero;
            grid.GenerateGrid();
        }

        if (GUILayout.Button("Clear Hex Grid"))
            grid.ClearGrid();

        if (GUILayout.Button("Save File")) {
            grid.SaveFile();
        }
    }
}