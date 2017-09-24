using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        serializedObject.Update();

        Tile tile = target as Tile;

        string[] options = new string[] { "Normal", "Path", "Block" };
        int index = EditorGUILayout.Popup(0, options);
        if (GUILayout.Button("Create")) {
            switch (index) {
                case 0:
                    tile.ChooseTileState(Tile.TileState.Normal);
                    break;
                case 1:
                    tile.ChooseTileState(Tile.TileState.Block);
                    break;
                case 2:
                    tile.ChooseTileState(Tile.TileState.Tree);
                    break;
                default:
                    break;
            }
        }
    }
}
