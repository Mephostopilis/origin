using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ABBuild : EditorWindow {

    [MenuItem("Tools/Build AssetBundle")]
    public static void BuildAB() {

        if (Directory.Exists(Application.streamingAssetsPath)) {
            Directory.Delete(Application.streamingAssetsPath, true);
            Directory.CreateDirectory(Application.streamingAssetsPath);
        } else {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Win64");
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Android");

        //BuildPipeline.BuildAssetBundles("Assets/ABs", BuildAssetBundleOptions.None, BuildTarget.Android);
        BuildPipeline.BuildAssetBundles("Assets/streamingAssets/Win64", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        BuildPipeline.BuildAssetBundles("Assets/streamingAssets/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
