using UnityEngine;
using UnityEditor;
using System.IO;
using Maria.Json;

public class BuildVersionFile : EditorWindow {

    private static int _main = 1;
    private static int _mid = 0;
    private static int _min = 0;
    private static int _build = 0;

    public static string GetVersionPath() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return Application.streamingAssetsPath + "/Win64";
#elif UNITY_IOS
        return Application.streamingAssetsPath + "/iOS";
#elif UNITY_ANDROID
        return Application.streamingAssetsPath + "/Android";
#endif
    }

    public static bool InitVersionFile() {
        DirectoryInfo di = new DirectoryInfo(GetVersionPath());
        if (di.Exists) {

        } else {

        }
        return true;
    }

    [MenuItem("Tools/Build VersionFile")]
    public static void Do() {
        // 生成version.json
        int version = (_main << 24) | (_mid << 16) | (_min << 8) | (_build & 0xff);
        JSONObject root = new JSONObject(JSONObject.Type.OBJECT);
        root.AddField("version", version);
        JSONObject abs = new JSONObject(JSONObject.Type.OBJECT);
        root.AddField("abs", abs);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        DirectoryInfo streaming = new DirectoryInfo(Application.streamingAssetsPath + "/Win64");
#elif UNITY_IOS
        DirectoryInfo streaming = new DirectoryInfo(Application.streamingAssetsPath + "/iOS");
#elif UNITY_ANDROID
        DirectoryInfo streaming = new DirectoryInfo(Application.streamingAssetsPath + "/Android");
#endif

        XX(abs, string.Empty, streaming);

        string en = root.Print();

        FileStream fs = new FileStream(GetVersionPath() + "/version.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(en);
        sw.Close();
        fs.Close();
    }

    private static void XX(JSONObject root, string path, DirectoryInfo di) {
        DirectoryInfo[] dis = di.GetDirectories();
        for (int i = 0; i < dis.Length; i++) {
            XX(root, path + dis[i].Name, dis[i]);
        }
        FileInfo[] fis = di.GetFiles();
        for (int i = 0; i < fis.Length; i++) {
            if (fis[i].Extension == ".normal") {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                string abpath = Application.streamingAssetsPath + "/Win64/" + path + "/" + fis[i].Name;
#elif UNITY_IOS
                string abpath = Application.streamingAssetsPath + "/iOS/" + path + "/" + fis[i].Name;
#elif UNITY_ANDROID
                string abpath = Application.streamingAssetsPath + "/Android/" + path + "/" + fis[i].Name;
#endif
                string abname = path + "/" + fis[i].Name;
                AssetBundle ab = AssetBundle.LoadFromFile(abpath);
                if (ab != null) {
                    int hash = ab.GetHashCode();
                    root.AddField(abname, hash);

                    ab.Unload(true);
                }

//            } else if (fis[i].Extension == ".manifest") {
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
//                string abpath = Application.streamingAssetsPath + "/Win64/" + path + "/" + fis[i].Name;
//#elif UNITY_IOS
//                string abpath = Application.streamingAssetsPath + "/iOS/" + path + "/" + fis[i].Name;
//#elif UNITY_ANDROID
//                string abpath = Application.streamingAssetsPath + "/Android/" + path + "/" + fis[i].Name;
//#endif
//                string manifestname = path + "/" + fis[i].Name;
//                root.AddField(manifestname, 1);
            }
        }
    }
}
