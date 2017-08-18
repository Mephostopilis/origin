using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏对象的抽象
/// </summary>
namespace Maria {

    [XLua.LuaCallCSharp]
    public class Actor : DisposeObject {

        [XLua.CSharpCallLua]
        [XLua.LuaCallCSharp]
        public delegate void RenderHandler();

        protected Context _ctx = null;
        protected Controller _controller = null;
        protected Service _service = null;
        protected GameObject _go = null;

        public Actor(Context ctx, Controller controller)
            : this(ctx, controller, null) {
        }

        public Actor(Context ctx, Controller controller, GameObject go) {
            _ctx = ctx;
            _controller = controller;
            _go = go;
            _controller.Add(this);
            UnityEngine.Debug.Assert(_service == null);
        }

        public Actor(Context ctx, Service service)
            : this(ctx, service, null) {
        }

        public Actor(Context ctx, Service service, GameObject go) {
            _ctx = ctx;
            _service = service;
            _go = go;
            _service.Add(this);
            UnityEngine.Debug.Assert(_controller == null);
        }

        public GameObject Go { get { return _go; } set { _go = value; } }
        public Controller Controller { get { return _controller; } set { _controller = value; } }
        public Service Service { get { return _service; } set { _service = value; } }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update(float delta) {
        }
    }
}
