using Bacon;
using UnityEngine;

public class App : MonoBehaviour
{
    private AppContext _ctx = null;
    
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

    public AppContext AppContext { get { return _ctx; } set { _ctx = value; } }

}
