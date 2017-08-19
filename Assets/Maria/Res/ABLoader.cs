﻿/*******************************************************
 * 此类的设计
 * 只有两个路劲可以加载资源，一个res，另外一个是per，默认是res
 * 每当更新的时候，会自动路劲资源文件改写本地
 * path变量只记录assetboundl路劲
**/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Maria.Res {

    [XLua.LuaCallCSharp]
    public class ABLoader : MonoBehaviour {

        enum PathType {
            RES,
            STR,
            PER,
        }

        public static ABLoader current = null;

        private AssetBundleManifest _abmanifest = null;
        private Dictionary<string, UnityEngine.Object> _res = new Dictionary<string, UnityEngine.Object>();
        private Dictionary<string, AssetBundle> _assets = new Dictionary<string, AssetBundle>();
        private Dictionary<string, PathType> _path = new Dictionary<string, PathType>();  // 只记录assetbundle的路劲

        private int _step = 0;
        private int _max = 0;
        private WWW _request;
        private string _lversion;
        private string _sversion;

        void Awake() {
            if (current == null) {
                current = this;
            }
        }

        void Start() {
        }

        void Update() {
            if (_request != null) {
                float p = _request.progress;

            }
            //_request.progress;
        }

        public void FetchVersion(Action cb) {
            _step = 0;
            StartCoroutine(FetchVersionFile(cb));
        }

        private string GetWWWPersistentDataPath() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            string url = "file:///" + UnityEngine.Application.persistentDataPath;
#elif UNITY_IOS
        url = "http://127.0.0.1:80/mahjong/iOS";
#elif UNITY_ANDROID
        url = "jar:file:///" + Application.persistentDataPath;
#endif
            return url;
        }

        private string GetHttpUrl() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            string url = "http://127.0.0.1:80/mahjong/Win64";
#elif UNITY_IOS
        string url = "http://127.0.0.1:80/mahjong/iOS";
#elif UNITY_ANDROID
        string url = "http://127.0.0.1:80/mahjong/Android";
#endif
            return url;
        }

        private string GetABFilePath(string path) {
            return path + ".normal";
        }

        private string GetResFilePath(string path) {
            return path;
        }

        private string GetPERPath(string path) {
            return Path.Combine(UnityEngine.Application.persistentDataPath, path);
        }

        private string GetSTRPath(string path) {
            return Path.Combine(UnityEngine.Application.streamingAssetsPath, path);
        }

        IEnumerator FetchVersionFile(Action cb) {
            _step = 1;
            WWW lrequest = new WWW(GetWWWPersistentDataPath() + "/version.mf");
            _request = lrequest;
            yield return lrequest;
            if (lrequest.text == null || lrequest.text.Length == 0) {
                // copy
                TextAsset tasset = Resources.Load<TextAsset>("version");
                if (tasset == null || tasset.text == null) {
                    cb();
                    yield break;
                }
                _lversion = tasset.text;
                CreateDirOrFile(UnityEngine.Application.persistentDataPath, new string[] { "version.mf" }, 0, tasset.text);

                // init path
                JSONObject o = new JSONObject(tasset.text);
                JSONObject abs = o.GetField("abs");
                for (int i = 0; i < abs.keys.Count; i++) {
                    _path[abs.keys[i]] = PathType.RES;
                }

                // write path
                JSONObject path = new JSONObject(JSONObject.Type.OBJECT);
                for (int i = 0; i < abs.keys.Count; i++) {
                    path.AddField(abs.keys[i], (int)PathType.RES);
                }
                CreateDirOrFile(UnityEngine.Application.persistentDataPath, new string[] { "path.mf" }, 0, path.ToString());
            }

            // double check
            if (lrequest.text == null || lrequest.text.Length == 0) {
                _step = 2;
                lrequest = new WWW(GetWWWPersistentDataPath() + "/version.mf");
                _request = lrequest;
                yield return lrequest;
                if (lrequest.text == null || lrequest.text.Length == 0) {
                    // no
                    cb();
                    yield break;
                }
                _lversion = lrequest.text;
            } else {
                _lversion = lrequest.text;
            }

            // check path
            _step = 3;
            WWW lpathrequest = new WWW(GetWWWPersistentDataPath() + "/path.mf");
            _request = lpathrequest;
            yield return lpathrequest;
            if (lpathrequest.text == null || lpathrequest.text.Length == 0) {
                // create
                UnityEngine.Debug.Assert(_lversion != null);
                JSONObject json = new JSONObject(_lversion);
                JSONObject abs = json.GetField("abs");
                JSONObject path = new JSONObject(JSONObject.Type.OBJECT);
                for (int i = 0; i < abs.keys.Count; i++) {
                    _path.Add(abs.keys[i], PathType.RES);
                    path.AddField(abs.keys[i], (int)PathType.RES);
                }
                CreateDirOrFile(UnityEngine.Application.persistentDataPath, new string[] { "path.mf" }, 0, path.ToString());
            } else {
                JSONObject path = new JSONObject(lpathrequest.text);
                for (int i = 0; i < path.keys.Count; i++) {
                    _path.Add(path.keys[i], (PathType)path.GetField(path.keys[i]).i);
                }
            }

            _step = 4;
            string url = GetHttpUrl();
            url = GetHttpUrl();
            WWW srequest = new WWW(url + "/version.json");
            _request = srequest;
            yield return srequest;
            if (srequest.text != null && srequest.text.Length > 0) {
                _sversion = srequest.text;
            } else {
                cb();
                yield break;
            }

            JSONObject ljson = new JSONObject(_lversion);
            JSONObject sjson = new JSONObject(_sversion);
            // 比较服务器与当地，不同就下载更新
            long lversion = ljson.GetField("version").i;
            long sversion = sjson.GetField("version").i;
            if (sversion > lversion) {
                // 比较资源
                JSONObject sabs = sjson.GetField("abs");
                JSONObject labs = ljson.GetField("abs");
                int step = _step;
                for (int i = 0; i < sabs.keys.Count; i++) {
                    JSONObject shash = sabs.GetField(sabs.keys[i]);
                    JSONObject lhash = labs.GetField(labs.keys[i]);
                    if (lhash == null || (shash.i != lhash.i)) {
                        // 下载并存储
                        _step = step + i;
                        WWW frequest = new WWW(url + "/" + sabs.keys[i]);
                        _request = frequest;
                        yield return frequest;
                        string[] sp = sabs.keys[i].Split(new char[] { '/' });
                        if (sp.Length > 1) {
                            CreateDirOrFile(UnityEngine.Application.persistentDataPath, sp, 0, Encoding.ASCII.GetString(frequest.bytes));
                        } else {
                            CreateDirOrFile(UnityEngine.Application.persistentDataPath, sp, 0, Encoding.ASCII.GetString(frequest.bytes));
                        }

                        _path[sabs.keys[i]] = PathType.PER;

                        // manifest
                        WWW mrequest = new WWW(url + "/" + sabs.keys[i] + ".manifest");
                        _request = mrequest;
                        yield return mrequest;
                        string[] msp = sabs.keys[i].Split(new char[] { '/' });
                        if (msp.Length > 1) {
                            CreateDirOrFile(UnityEngine.Application.persistentDataPath, msp, 0, Encoding.ASCII.GetString(mrequest.bytes));
                        } else {
                            CreateDirOrFile(UnityEngine.Application.persistentDataPath, msp, 0, Encoding.ASCII.GetString(mrequest.bytes));
                        }
                        _path[sabs.keys[i] + ".manifest"] = PathType.PER;

                    } else {
                    }
                }
                // 写入版本文件
                CreateDirOrFile(UnityEngine.Application.persistentDataPath, new string[] { "version.mf" }, 0, sjson.ToString());
                // 写入路径文件
                JSONObject path = new JSONObject(JSONObject.Type.OBJECT);
                foreach (var item in _path) {
                    path.AddField(item.Key, (int)item.Value);
                }
                CreateDirOrFile(UnityEngine.Application.persistentDataPath, new string[] { "path.mf" }, 0, path.ToString());
                cb();
            } else {
                cb();
            }
        }

        private void CreateDirOrFile(string parent, string[] sp, int i, string value) {
            UnityEngine.Debug.Assert(parent.Length > 0);
            UnityEngine.Debug.Assert(sp != null && sp.Length > 0);
            if (i <= (sp.Length - 2)) {
                string name = sp[i];
                string path = Path.Combine(parent, name);

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                i++;
                CreateDirOrFile(path, sp, i, value);
            } else {
                string name = sp[i];
                string path = parent + "/" + name;
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(value);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        public TextAsset LoadTextAsset(string path, string name) {
            return LoadAsset<TextAsset>(path, name);
        }

        public T LoadAsset<T>(string path, string name) where T : UnityEngine.Object {
            if (_path.ContainsKey(path) && _path[path] == PathType.PER) {
                T res = LoadAB<T>(path, name);
                if (res == null) {
                    string xpath = path + "/" + name;
                    res = LoadRes<T>(xpath);
                }
                return res;
            } else {
                string xpath = path + "/" + name;
                return LoadRes<T>(xpath);
            }
        }

        public void LoadAssetAsync<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
            if (_path.ContainsKey(path) && _path[path] == PathType.PER) {
                LoadABAsync<T>(path, name, (T asset) => {
                    if (asset == null) {
                        LoadResAsync<T>(Path.Combine(path, name), cb);
                    } else {
                        cb(asset);
                    }
                });
            } else {
                LoadResAsync<T>(Path.Combine(path, name), cb);
            }
        }

        private T LoadAB<T>(string path, string name) where T : UnityEngine.Object {
            string abpath = GetABFilePath(path);
            if (_assets.ContainsKey(abpath)) {
                AssetBundle ab = _assets[abpath];
                if (ab != null) {
                    return ab.LoadAsset<T>(name);
                }
            }

            if (_abmanifest == null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                string manifest = "Win64";
#elif UNITY_IOS
                    string manifest = "iOS";
#elif UNITY_ANDROID
                    string manifest = "Android";
#endif
                if (_path.ContainsKey(manifest) && _path[manifest] == PathType.PER) {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(GetPERPath(manifest));
                    _abmanifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                } else {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(GetPERPath(manifest));
                    _abmanifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }

            if (_abmanifest != null) {
                string[] depends = _abmanifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    LoadAB<UnityEngine.Object>(path, depends[i]);
                }
            }

            if (_path.ContainsKey(abpath)) {
                if (_path[path] == PathType.PER) {
                    AssetBundle ab = AssetBundle.LoadFromFile(GetPERPath(path));
                    if (ab.Contains(name)) {
                        return ab.LoadAsset<T>(name);
                    } else {
                        UnityEngine.Debug.LogError("no exits");
                        return null;
                    }
                } else {
                    AssetBundle ab = AssetBundle.LoadFromFile(GetSTRPath(path));
                    if (ab != null && ab.Contains(name)) {
                        return ab.LoadAsset<T>(name);
                    } else {
                        UnityEngine.Debug.LogError("no exits");
                        return null;
                    }
                }
            } else {
                return null;
            }
        }

        private void LoadABAsync<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
            string abpath = GetABFilePath(path);
            if (_assets.ContainsKey(path)) {
                AssetBundle ab = _assets[path];
                if (ab.Contains(name)) {
                    T asset = ab.LoadAsset<T>(name);
                    cb(asset);
                }
            } else {
                StartCoroutine(LoadABAsyncImp(path, name, cb));
            }
        }

        IEnumerator LoadABAsyncImp<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
            if (_abmanifest == null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                string manifest = "Win64";
#elif UNITY_IOS
                string manifest = "iOS";
#elif UNITY_ANDROID
                string manifest = "Android";
#endif
                if (_path.ContainsKey(manifest) && _path[manifest] == PathType.PER) {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(GetPERPath(path));
                    _abmanifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                } else {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(GetSTRPath(path));
                    _abmanifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }

            if (_abmanifest != null) {
                string[] depends = _abmanifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    if (_path.ContainsKey(depends[i]) && _path[depends[i]] == PathType.PER) {
                        AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(GetPERPath(depends[i]));
                        yield return depend_request;
                        _assets[depends[i]] = depend_request.assetBundle;
                    } else {
                        AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(GetPERPath(depends[i]));
                        yield return depend_request;
                        _assets[depends[i]] = depend_request.assetBundle;
                    }
                }
            }

            if (_path.ContainsKey(path)) {
                if (_path[path] == PathType.PER) {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(UnityEngine.Application.persistentDataPath + "/" + path);
                    yield return request;
                    _assets[path] = request.assetBundle;
                    AssetBundle ab = request.assetBundle;
                    T asset = ab.LoadAsset<T>(name);
                    cb(asset);
                } else {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(UnityEngine.Application.streamingAssetsPath + "/" + path);
                    yield return request;
                    _assets[path] = request.assetBundle;
                    AssetBundle ab = request.assetBundle;
                    T asset = ab.LoadAsset<T>(name);
                    cb(asset);
                }
            } else {
                cb(null);
            }
        }

        private void LoadResAsync<T>(string path, Action<T> cb) where T : UnityEngine.Object {
            if (_res.ContainsKey(path)) {
                UnityEngine.Object o = _res[path];
                if (o != null) {
                    cb(o as T);
                } else {
                    UnityEngine.Debug.LogErrorFormat("path : {0} has been loaded res is null.");
                }
            } else {
                StartCoroutine(LoadResAsyncImp<T>(path, cb));
            }
        }

        IEnumerator LoadResAsyncImp<T>(string path, Action<T> cb) where T : UnityEngine.Object {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;
            T asset = request.asset as T;
            if (asset == null) {
                UnityEngine.Debug.LogErrorFormat("load res occor wrong, path is {0}", path);
                cb(asset);
            } else {
                _res[path] = asset;
                cb(asset);
            }
        }

        private T LoadRes<T>(string path) where T : UnityEngine.Object {
            if (_res.ContainsKey(path)) {
                return _res[path] as T;
            } else {
                UnityEngine.Debug.LogFormat("load res path: {0}", path);
                T res = Resources.Load(path) as T;
                if (res != null) {
                    _res[path] = res;
                    return res;
                }
                return null;
            }
        }

        public void Unload() {
            foreach (var item in _assets) {
                item.Value.Unload(true);
            }
        }
    }

}