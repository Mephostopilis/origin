using Maria.Network;
using System.Collections.Generic;
using UnityEngine;
using Maria;

namespace Bacon.Game {
    public class Ball : Maria.Component
    {
        public enum BallId {

        }


        protected Scene _scene = null;

        protected long _id = 0;
        protected uint _uid = 0;
        protected uint _session = 0;

        protected GameObject _toy = null;
        protected GameObject _arrow = null;

        protected float _radis = 0;
        protected float _length = 0.0f;
        protected float _width = 0.0f;
        protected float _height = 0.0f;

        protected Vector3 _pos = Vector3.zero;
        protected Vector3 _dir = new Vector3(0, 0, 0);
        protected float _vel = 0.0f;

        protected AABB _aabb = null;

        public Ball(Maria.Entity entity, float radis, float length, float width, float height)
            : base(entity)
        {
            //_scene = scene;

            _session = 0;
            _id = 0;

            _radis = radis;
            _length = length;
            _width = width;
            _height = height;

            _pos = Vector3.zero;
            _dir = Vector3.right;
            _vel = 0.0f;

            //_ctx.EnqueueRenderQueue(RenderInitBall);
        }

        //public void RenderInitBall() {
        //    string path = "Prefabs/Ball";
        //    UnityEngine.Object o = Resources.Load(path, typeof(GameObject));
        //    GameObject go = UnityEngine.Object.Instantiate(o) as GameObject;
        //    _go = go;
        //    _go.transform.localPosition = _pos;
        //    var world = _scene.Go;
        //    _go.transform.SetParent(world.transform);

        //    try {
        //        if (_go != null) {
        //            _toy = _go.transform.Find("Toy").gameObject;
        //            _arrow = _go.transform.Find("Arrow").gameObject;
        //            CalArrowPosition();
        //            CalArrowDirection();
        //        }
        //    } catch (KeyNotFoundException ex) {
        //        Debug.LogError(ex.Message);
        //        //throw ex;
        //    }

        //    Vector3 min = _pos + new Vector3(-_length / 2, -_width / 2, -_height / 2);
        //    Vector3 max = _pos + new Vector3(_length / 2, _width / 2, _height / 2);
        //    _aabb = new AABB(min, max);
        //}

        //#region sync server

        //public uint Session { get { return _session; } set { _session = value; } }

        //public long Id { get { return _id; } set { _id = value; } }

        //public Vector3 Pos { get { return _pos; } }

        //public Vector3 Dir { get { return _dir; } set { _dir = value; } }

        //public float Vel { get { return _vel; } set { _vel = value; } }

        //public AABB AABB { get { return _aabb; } }

        //public void MoveBy(Vector2 v) {
        //    Vector3 shift = new Vector3(v.x, 0, v.y);
        //    MoveBy(shift);
        //}

        //public void MoveBy(Vector3 v) {
        //    _pos += v;
        //    _ctx.EnqueueRenderQueue(RenderTransform);
        //}

        //public void MoveTo(Vector2 v) {
        //    // 保留原来的y值，
        //    Vector3 shift = new Vector3(v.x, _pos.y, v.y);
        //    MoveTo(shift);
        //}

        //public void MoveTo(Vector3 v) {
        //    _pos = v;
        //    _ctx.EnqueueRenderQueue(RenderTransform);
        //}

        //protected void CalArrowPosition() {
        //    Vector3 arr = _dir * _radis + _pos;
        //    _arrow.transform.position = arr;
        //}

        //protected void CalArrowDirection() {
        //    //_direction. Vector3.up;
        //}

        //public byte[] PackPos() {
        //    byte[] res = new byte[12];
        //    NetPack.Packlf(res, 0, _pos.x);
        //    NetPack.Packlf(res, 4, _pos.y);
        //    NetPack.Packlf(res, 8, _pos.z);
        //    return res;
        //}

        //public byte[] PackDir() {
        //    Vector3 direction = _dir;
        //    byte[] res = new byte[12];
        //    NetPack.Packlf(res, 0, direction.x);
        //    NetPack.Packlf(res, 4, direction.y);
        //    NetPack.Packlf(res, 8, direction.z);
        //    return res;
        //}
        //#endregion

        //public void Leave() {
        //    _ctx.EnqueueRenderQueue(RenderLeave);
        //}

        //public void RenderLeave() {
        //    GameObject.DestroyImmediate(_go);
        //}

        //public void RenderTransform() {
        //    if (_go != null) {
        //        _go.transform.localPosition = _pos;
        //    }
        //}
    }
}
