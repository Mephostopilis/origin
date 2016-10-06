using UnityEngine;
using Maria.App;
using Maria;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    private AppContext _ctx = null;
    private Stack<GameObject> _stack = new Stack<GameObject>();
    public GameObject _stackGo = null;
    public GameObject _cur = null;
    private GameObject _root = null;
    
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        _ctx = new AppContext(this);    
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (_ctx != null)
        {
            _ctx.Update(deltaTime);
        }
    }

    public T GetController<T>(string name) where T : Controller
    {
        return _ctx.GetController<T>(name);
    }

    public AppContext AppContext { get { return _ctx; } set { _ctx = value; } }

    public void Push(string name)
    {
        _cur.transform.SetParent(_stackGo.transform);
        _cur.SetActive(false);
        _stack.Push(_cur);

        string path = "Prefabs/Ball/";
        string pathname = path + name;

        //string pathname2 = "file///" + Application.dataPath + path + name + ".prefab";
        //string pathname2 = Application.dataPath + path + name + ".prefab";
        //AssetBundle b = AssetBundle.LoadFromFile(pathname2);

        Object o = Resources.Load(pathname, typeof(GameObject));
        GameObject go = Instantiate(o) as GameObject;
        _cur = go;
        _cur.transform.SetParent(transform);
    }

    public void Pop()
    {
        if (_stack.Count > 0)
        {
            transform.DetachChildren();
            Destroy(_cur);

            GameObject go = _stack.Pop();
            _cur = go;
            _cur.transform.SetParent(transform);
        } else
        {

        }
    }
}
