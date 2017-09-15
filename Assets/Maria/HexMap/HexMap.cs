using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria.HexMap {
    public class HexMap {
        public static float radis = 1.0f;

        public enum MapState {

        }

        public enum MapOrientation {
            FLAT,
            POINTY,
        }

        public struct CubeCoord {
            public int q;
            public int r;
            public int s;

            public CubeCoord(int q_, int r_, int s_) {
                q = q_;
                r = r_;
                s = s_;
            }
        }

        public struct OffsetCoord {
            public int c;
            public int r;
        }

        public struct AxialCoord {
            public int q;
            public int r;
        }

        public struct Hex {
            public CubeCoord cube;
            public OffsetCoord offset;
            public AxialCoord axial;
            public float height;
            public Vector3 pos;
            public MapState state;
            public Hex[] neighbor;

            //public Hex() {
            //     neighbor = new Hex[6];
            //}

            public Hex(AxialCoord coord_, float height_) {
                cube = Axial2Cube(coord_);
                offset = Axial2Offset(coord_);
                height = height_;
                pos = Axial2Vec3(coord_, height_);

            }

            //public 

            public long ToHash() {
                long h = 0;
                h |= (long)axial.q;
                h = h << 32;
                h |= (long)axial.r;
                return h;
            }

            public Hex Get0Neighbor() {
                return new Hex();
            }

            
        }

        private Dictionary<long, Hex> _dic = new Dictionary<long, Hex>();

        public void Scan() {
            foreach (var item in _dic) {
                //item.Value
            }
        }

        public void ClickGenHex(Vector3 pos) {
            Hex hex = new Hex()
        }

        private CubeCoord Axial2Cube(AxialCoord coord) {
            return new CubeCoord();
        }

        private OffsetCoord Axial2Offset(AxialCoord coord) {
            return new OffsetCoord();
        }

        private Vector3 Axial2Vec3(AxialCoord coord, float height) {
            return Vector3.zero;
        }

        private AxialCoord Vec32Axial(Vector3 pos) {
            return new AxialCoord();
        }

    }
}
