using UnityEngine;
using System.Collections;
using Maria;

public class RootBehaviour : MonoBehaviour
{
    private App _app = null;
    private Context _ctx;
    private Controller _controller;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnter(Context ctx, Controller ctr)
    {
        _ctx = ctx;
        _controller = ctr;
    }

    protected void InitApp()
    {
        if (_app == null)
        {
            _app = GameObject.Find("App").GetComponent<App>();
            Debug.Assert(_app != null);
        }
    }

    public App App
    {
        get
        {
            if (_app == null)
            {
                InitApp();
                return _app;
            }
            else
            {
                return _app;
            }
        }
        set
        {
            InitApp();
        }
    }
}
