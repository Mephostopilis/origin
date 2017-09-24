using UnityEditor;
using UnityEngine;
using System.Collections;
using Map;

[CustomEditor(typeof(Grid))]
public class GridInspector : Editor {

    public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		Grid grid = target as Grid;

		if(GUILayout.Button("Generate Hex Grid")) {
            grid.transform.localPosition = Vector3.zero;
            grid.ClearGrid();
            grid.GenerateGrid();
        }
			
		if(GUILayout.Button("Clear Hex Grid"))
			grid.ClearGrid();

        if (GUILayout.Button("Save File")) {
            grid.SaveFile();
        }
	}
}