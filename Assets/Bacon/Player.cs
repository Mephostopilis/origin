using Maria.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class Player {

        protected uint _session = 0;
        private Dictionary<long, Ball> _myballs = new Dictionary<long, Ball>();

        public Player(uint session) {
            _session = session;
        }

        public uint Session { get { return _session; } set { _session = value; } }

        public void Add(Ball ball) {
            _myballs.Add(ball.Id, ball);
        }

        public void Remove(Ball ball) {
            _myballs.Remove(ball.Id);
        }

        public void Clear() {
            _myballs.Clear();
        }

        public List<long> GetBallids() {
            return new List<long>(_myballs.Keys);
        }

        public byte[] PackBall() {
            int idx = 0;
            byte[] buf = new byte[32 * _myballs.Count];
            foreach (var ball in _myballs) {
                NetPack.Packll(buf, 32 * idx + 0, ball.Value.Id);
                byte[] pos = ball.Value.PackPos();
                byte[] dir = ball.Value.PackDir();
                Array.Copy(pos, 0, buf, 28 * idx + 8, pos.Length);
                Array.Copy(dir, 0, buf, 28 * idx + 20, dir.Length);
                idx++;
            }
            return buf;
        }

        public void ChangeDir(Vector3 dir) {
            for (int i = 0; i < _myballs.Count; i++) {
                var ball = _myballs[i];
                ball.Dir = dir;
            }
        }

        public Vector3 GetPivot() {
            Vector3 pivot = Vector3.zero;
            if (_myballs.Count > 0) {
                foreach (var item in _myballs) {
                    pivot += item.Value.Pos;
                }
                return pivot * (1 / _myballs.Count);
            } else {
                return new Vector3(10, 20, 10);
            }
            
        }
    }
}
