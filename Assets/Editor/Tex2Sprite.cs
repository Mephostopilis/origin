using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tex2Sprite : ScriptableWizard {

    [MenuItem("Tools/Tex To Sprite")]
    public static void CreateWizard() {
        ScriptableWizard.DisplayWizard<Tex2Sprite>("Tex To Sprite", "Create");
    }

    void OnWizardUpdate() {
    }

    void OnWizardCreate() {
        string path = "Assets/Resources/Textures/MainUI";
        Object[] asstes = AssetDatabase.LoadAllAssetsAtPath(path);
        for (int i = 0; i < asstes.Length; i++) {
            Texture2D tex = asstes[i] as Texture2D;
            Sprite sprite = Sprite.Create(tex, new Rect(Vector2.zero, tex.texelSize), new Vector2(0.5f, 0.5f));
            //WWW.LoadFromCacheOrDownload()
        }
        
    }
}
