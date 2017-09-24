using UnityEngine;
using System.Collections.Generic;

namespace Map {

    public class Tile : MonoBehaviour {

        [System.Serializable]
        public struct FractionalIndex {
            public FractionalIndex(double q, double r, double s) {
                this.q = q;
                this.r = r;
                this.s = s;
            }
            public readonly double q;
            public readonly double r;
            public readonly double s;

            static public CubeIndex HexRound(FractionalIndex h) {
                int q = (int)(Mathf.RoundToInt((float)h.q));
                int r = (int)(Mathf.RoundToInt((float)h.r));
                int s = (int)(Mathf.RoundToInt((float)h.s));
                double q_diff = Mathf.Abs((float)(q - h.q));
                double r_diff = Mathf.Abs((float)(r - h.r));
                double s_diff = Mathf.Abs((float)(s - h.s));
                if (q_diff > r_diff && q_diff > s_diff) {
                    q = -r - s;
                } else
                    if (r_diff > s_diff) {
                    r = -q - s;
                } else {
                    s = -q - r;
                }
                return new CubeIndex(q, r, s);
            }


            //static public FractionalHex HexLerp(FractionalHex a, FractionalHex b, double t) {
            //    return new FractionalHex(a.q * (1 - t) + b.q * t, a.r * (1 - t) + b.r * t, a.s * (1 - t) + b.s * t);
            //}


            //static public List<Hex> HexLinedraw(Hex a, Hex b) {
            //    int N = Hex.Distance(a, b);
            //    FractionalHex a_nudge = new FractionalHex(a.q + 0.000001, a.r + 0.000001, a.s - 0.000002);
            //    FractionalHex b_nudge = new FractionalHex(b.q + 0.000001, b.r + 0.000001, b.s - 0.000002);
            //    List<Hex> results = new List<Hex> { };
            //    double step = 1.0 / Math.Max(N, 1);
            //    for (int i = 0; i <= N; i++) {
            //        results.Add(FractionalHex.HexRound(FractionalHex.HexLerp(a_nudge, b_nudge, step * i)));
            //    }
            //    return results;
            //}

        }

        [System.Serializable]
        public struct OffsetIndex {
            public int row;
            public int col;

            public OffsetIndex(int row, int col) {
                this.row = row; this.col = col;
            }
        }

        [System.Serializable]
        public struct CubeIndex {
            public int x;
            public int y;
            public int z;

            public CubeIndex(int x, int y, int z) {
                this.x = x; this.y = y; this.z = z;
            }

            public CubeIndex(int x, int z) {
                this.x = x; this.z = z; this.y = -x - z;
            }

            public static CubeIndex operator +(CubeIndex one, CubeIndex two) {
                return new CubeIndex(one.x + two.x, one.y + two.y, one.z + two.z);
            }

            public override bool Equals(object obj) {
                if (obj == null)
                    return false;
                CubeIndex o = (CubeIndex)obj;
                if ((System.Object)o == null)
                    return false;
                return ((x == o.x) && (y == o.y) && (z == o.z));
            }

            public override int GetHashCode() {
                return (x.GetHashCode() ^ (y.GetHashCode() + (int)(Mathf.Pow(2, 32) / (1 + Mathf.Sqrt(5)) / 2) + (x.GetHashCode() << 6) + (x.GetHashCode() >> 2)));
            }

            public override string ToString() {
                return string.Format("[" + x + "," + y + "," + z + "]");
            }
        }

        public struct AxialIndex {
            public int g;
            public int r;
            AxialIndex(int g, int r) {
                this.g = g;
                this.r = r;
            }
        }

        [System.Serializable]
        public enum TileState {
            Normal = 0,
            Block,
            Tree,
        }

        public Grid grid;
        public CubeIndex index;
        public Grid.HexOrientation orientation;
        public Vector3 position;
        private TileState state;
        private float[] cornerheight = new float[6];
        

        public static Vector3 Corner(Vector3 origin, float radius, int corner, Grid.HexOrientation orientation) {
            float angle = 60 * corner;
            if (orientation == Grid.HexOrientation.Pointy)
                angle += 30;
            angle *= Mathf.PI / 180;
            return new Vector3(origin.x + radius * Mathf.Cos(angle), origin.y, origin.z + radius * Mathf.Sin(angle));
        }

        public void AddHexMesh(float radius, Grid.HexOrientation orientation) {
            Mesh mesh = new Mesh();

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < 6; i++) {
                Vector3 pos = Corner(Vector3.zero, radius, i, orientation);
                Vector3 worldPos = transform.TransformPoint(pos);
                float y = grid.SampleHeight(worldPos);
                Vector3 localPos = transform.InverseTransformPoint(new Vector3(worldPos.x, y, worldPos.z));
                verts.Add(localPos);
            }

            tris.Add(0);
            tris.Add(2);
            tris.Add(1);

            tris.Add(0);
            tris.Add(5);
            tris.Add(2);

            tris.Add(2);
            tris.Add(5);
            tris.Add(3);

            tris.Add(3);
            tris.Add(5);
            tris.Add(4);

            //UVs are wrong, I need to find an equation for calucalting them
            uvs.Add(new Vector2(0.5f, 1f));
            uvs.Add(new Vector2(1, 0.75f));
            uvs.Add(new Vector2(1, 0.25f));
            uvs.Add(new Vector2(0.5f, 0));
            uvs.Add(new Vector2(0, 0.25f));
            uvs.Add(new Vector2(0, 0.75f));

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.name = "Hexagonal Plane";

            mesh.RecalculateNormals();

            if (GetComponent<MeshFilter>() == null) {
                gameObject.AddComponent<MeshFilter>();
            }
            GetComponent<MeshFilter>().mesh = mesh;
        }

        public void AddOutlines(float radius, Grid.HexOrientation orientation) {
            LineRenderer lines = GetComponent<LineRenderer>();
            lines.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lines.receiveShadows = false;
            lines.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            lines.startWidth = 0.1f;
            lines.endWidth = 0.1f;
            lines.startColor = Color.cyan;
            lines.endColor = Color.blue;

            //lines.material = lineMaterial;
            lines.useWorldSpace = false;

            lines.material = new Material(Shader.Find("Particles/Additive"));
            lines.positionCount = 7;

            for (int vert = 0; vert <= 6; vert++) {
                Vector3 pos = Tile.Corner(Vector3.zero, radius, vert, orientation);
                Vector3 worldPos = transform.TransformPoint(pos);
                float y = grid.SampleHeight(worldPos);
                Vector3 localPos = transform.InverseTransformPoint(new Vector3(worldPos.x, y, worldPos.z));
                lines.SetPosition(vert, localPos);
            }
        }

        public void ChooseTileState(TileState s) {
            LineRenderer li = GetComponent<LineRenderer>();
            state = s;
            switch (state) {
                case TileState.Normal: {
                        li.startColor = Color.red;
                        li.endColor = Color.magenta;
                    }
                    break;
                case TileState.Block:
                    li.startColor = Color.black;
                    li.endColor = Color.magenta;
                    break;
                case TileState.Tree:
                    li.startColor = Color.gray;
                    li.endColor = Color.cyan;
                    break;
                default:
                    break;
            }
        }

        #region Coordinate Conversion Functions
        public static OffsetIndex CubeToEvenFlat(CubeIndex c) {
            OffsetIndex o;
            o.row = c.x;
            o.col = c.z + (c.x + (c.x & 1)) / 2;
            return o;
        }

        public static CubeIndex EvenFlatToCube(OffsetIndex o) {
            CubeIndex c;
            c.x = o.col;
            c.z = o.row - (o.col + (o.col & 1)) / 2;
            c.y = -c.x - c.z;
            return c;
        }

        public static OffsetIndex CubeToOddFlat(CubeIndex c) {
            OffsetIndex o;
            o.col = c.x;
            o.row = c.z + (c.x - (c.x & 1)) / 2;
            return o;
        }

        public static CubeIndex OddFlatToCube(OffsetIndex o) {
            CubeIndex c;
            c.x = o.col;
            c.z = o.row - (o.col - (o.col & 1)) / 2;
            c.y = -c.x - c.z;
            return c;
        }

        public static OffsetIndex CubeToEvenPointy(CubeIndex c) {
            OffsetIndex o;
            o.row = c.z;
            o.col = c.x + (c.z + (c.z & 1)) / 2;
            return o;
        }

        public static CubeIndex EvenPointyToCube(OffsetIndex o) {
            CubeIndex c;
            c.x = o.col - (o.row + (o.row & 1)) / 2;
            c.z = o.row;
            c.y = -c.x - c.z;
            return c;
        }

        public static OffsetIndex CubeToOddPointy(CubeIndex c) {
            OffsetIndex o;
            o.row = c.z;
            o.col = c.x + (c.z - (c.z & 1)) / 2;
            return o;
        }

        public static CubeIndex OddPointyToCube(OffsetIndex o) {
            CubeIndex c;
            c.x = o.col - (o.row - (o.row & 1)) / 2;
            c.z = o.row;
            c.y = -c.x - c.z;
            return c;
        }

        public static Tile operator +(Tile one, Tile two) {
            //Tile ret = new Tile();
            CubeIndex index = one.index + two.index;
            Tile ret = one.grid.TileAt(index);
            return ret;
        }

        public void LineColour(Color colour) {
            LineRenderer lines = GetComponent<LineRenderer>();
            if (lines)
                lines.SetColors(colour, colour);
        }

        public void LineColour(Color start, Color end) {
            LineRenderer lines = GetComponent<LineRenderer>();
            if (lines)
                lines.SetColors(start, end);
        }

        public void LineWidth(float width) {
            LineRenderer lines = GetComponent<LineRenderer>();
            if (lines)
                lines.SetWidth(width, width);
        }

        public void LineWidth(float start, float end) {
            LineRenderer lines = GetComponent<LineRenderer>();
            if (lines)
                lines.SetWidth(start, end);
        }
        #endregion
    }

}
