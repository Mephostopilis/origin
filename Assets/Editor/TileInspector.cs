using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
                    tile.ChooseTileState(TileState.Normal);
                    break;
                case 1:
                    tile.ChooseTileState(TileState.Block);
                    break;
                case 2:
                    tile.ChooseTileState(TileState.Tree);
                    break;
                default:
                    break;
            }
        }
    }
}
