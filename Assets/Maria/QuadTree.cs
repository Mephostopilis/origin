using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maria {
    public class QuadTree {
        public class QuadNode {
            public QuadNode() {
                UL = new Vector4(0, 0, 0, 1);
                UR = new Vector4(1, 0, 0, 1);
                TL = new Vector4(0, 1, 0, 1);
                TR = new Vector4(1, 1, 0, 1);
            }

            public Vector4 UL { get; set; }
            public Vector4 UR { get; set; }
            public Vector4 TL { get; set; }
            public Vector4 TR { get; set; }

            public QuadNode ULChild { get; set; }
            public QuadNode URChild { get; set; }
            public QuadNode TLChild { get; set; }
            public QuadNode TRChild { get; set; }
        }

        public QuadTree() {
            _depth = 7;
            _root = new QuadNode();
            Init(_root, 6);
        }


        public void Init(QuadNode parent, int depth) {
            depth--;
            if (depth == 0) {
                return;
            }
            Vector4 lc = (parent.UL + parent.TL) / 2;
            Vector4 rc = (parent.UR + parent.TR) / 2;
            Vector4 tc = (parent.TL + parent.TR) / 2;
            Vector4 uc = (parent.UL + parent.TL) / 2;
            Vector4 c = (parent.UL + parent.TR) / 2;
            QuadNode ul = new QuadNode();
            ul.TL = lc;
            ul.UL = parent.UL;
            ul.TR = c;
            ul.UR = uc;
            parent.ULChild = ul;
            Init(ul, depth);

            QuadNode tl = new QuadNode();
            tl.TL = parent.TL;
            tl.UL = lc;
            tl.TR = tc;
            tl.UR = c;
            parent.TLChild = tl;
            Init(tl, depth);

            QuadNode ur = new QuadNode();
            ur.TL = c;
            ur.UL = uc;
            ur.TR = rc;
            ur.UR = parent.UR;
            parent.URChild = ur;
            Init(ur, depth);

            QuadNode tr = new QuadNode();
            tr.TL = tc;
            tr.UL = c;
            tr.TR = parent.TR;
            tr.UR = rc;
            parent.TRChild = tr;
            Init(tr, depth);
        }

        //public addShape()

        private QuadNode _root;
        private int _depth;
    }
}
