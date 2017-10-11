using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game {
    class NetFrameQueue {
        private int _k;
        private SortedDictionary<int, NetFrame> _sorteddic = new SortedDictionary<int, NetFrame>();

        public void InitK(int k) {
            _k = k;
        }

        public void Enqueue(NetFrame frame) {
            _sorteddic.Add(frame.k, frame);
        }

        public int Dequeue(out int k, out NetFrame frame) {
            if (_sorteddic.ContainsKey(_k)) {
                frame = _sorteddic[_k];
                _k++;
                k = _k;
                return 1;
            }
            k = _k;
            frame = null;
            return 0;
        }

        public int BackK() {
            return (--_k);
        }
    }
}
