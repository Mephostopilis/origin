using UnityEngine;
using Maria.Ball;
using Maria;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    private AppContext _ctx = new AppContext();
    private Stack<GameObject> _stack = new Stack<GameObject>();
    public GameObject _cur = null;

    // Use this for initialization
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        _ctx.Update(deltaTime);
    }

    public T GetController<T>(string name) where T : Controller
    {
        return _ctx.GetController<T>(name);
    }

    public AppContext AppContext { get { return _ctx; } set { _ctx = value; } }

    public void Push(string name)
    {
        _cur.SetActive(false);

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

            GameObject go = _stack.Pop();
            _cur = go;
            _cur.transform.SetParent(transform);
        } else
        {

        }
    }
}
