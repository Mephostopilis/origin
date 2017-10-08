using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml;

namespace Map {

    public class Grid : MonoBehaviour {

        [System.Serializable]
        public enum MapShape {
            Rectangle,
            Hexagon,
            Parrallelogram,
            Triangle
        }

        [System.Serializable]
        public enum HexOrientation {
            Pointy,
            Flat
        }

        public class GridWaypointHead {

            private int _current = 1;
            private int _pathid = 0;

            public GridWaypointHead(int pathid) {
                _pathid = pathid;
                Free = true;
            }

            public int Pathid { get { return _pathid; } }
            public bool Free { get; set; }
            public Tile Exit { get; set; }
            public List<TileWaypoint> Open { get; set; }
            public List<TileWaypoint> Closed { get; set; }

            // read
            public int Current { get { return _current; } set { _current = value; } }

            public void Clear() {
                _current = 1;
                Free = true;
                Exit = null;
                Open.Clear();
                Closed.Clear();
            }
        }

        //Map settings
        public MapShape mapShape = MapShape.Rectangle;
        public int mapWidth;
        public int mapHeight;

        //Hex Settings
        public HexOrientation hexOrientation = HexOrientation.Flat;
        public float hexRadius = 1;
        public Material hexMaterial;

        //Generation Options
        public bool addColliders = true;
        public bool drawOutlines = true;
        public Material lineMaterial;

        //File Name
        public string fileName = "map1";

        //Internal variables
        private Dictionary<string, Tile> grid = new Dictionary<string, Tile>();

        private Tile.CubeIndex[] directions =
            new Tile.CubeIndex[] {
            new Tile.CubeIndex(1, -1, 0),
            new Tile.CubeIndex(1, 0, -1),
            new Tile.CubeIndex(0, 1, -1),
            new Tile.CubeIndex(-1, 1, 0),
            new Tile.CubeIndex(-1, 0, 1),
            new Tile.CubeIndex(0, -1, 1)
            };
        private List<ObjectSampler> sampleres = new List<ObjectSampler>();
        private GridWaypointHead[] astars = new GridWaypointHead[100];

        #region Getters and Setters
        public Dictionary<string, Tile> Tiles {
            get { return grid; }
        }
        #endregion

        #region Public Methods
        public void GenerateGrid() {
            //Generating a new grid, clear any remants and initialise values
            ClearGrid();

            //Generate the grid shape
            switch (mapShape) {
                case MapShape.Hexagon:
                    GenHexShape();
                    break;

                case MapShape.Rectangle:
                    GenRectShape();
                    break;

                case MapShape.Parrallelogram:
                    GenParrallShape();
                    break;

                case MapShape.Triangle:
                    GenTriShape();
                    break;

                default:
                    break;
            }
        }

        public void ClearGrid() {
            Debug.Log("Clearing grid...");
            foreach (var tile in grid)
                DestroyImmediate(tile.Value.gameObject);
            grid.Clear();
        }

        public void SaveFile() {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("name", "map");
            dic.Add("width", mapWidth);
            dic.Add("height", mapHeight);
            dic.Add("orientation", (int)HexOrientation.Flat);
            dic.Add("shape", (int)MapShape.Rectangle);
            List<object> grids = new List<object>();
            foreach (var item in grid) {
                Dictionary<string, object> tile = new Dictionary<string, object>();
                tile.Add("g", item.Value.index.x);
                tile.Add("r", item.Value.index.y);
                tile.Add("s", item.Value.index.z);
                float height = item.Value.SamepleHeight();
                tile.Add("height", height.ToString());
                tile.Add("state", 1);
                grids.Add(tile);
            }
            dic.Add("grids", grids);
            Maria.PlistCS.Plist.writeXml(dic, UnityEngine.Application.streamingAssetsPath + "\\" + fileName + ".map");
        }

        public void RegisterObjectSampler(ObjectSampler sampler) {
            sampleres.Add(sampler);
        }

        public Tile TileAt(Tile.CubeIndex index) {
            if (grid.ContainsKey(index.ToString()))
                return grid[index.ToString()];
            return null;
        }

        public Tile TileAt(int x, int y, int z) {
            return TileAt(new Tile.CubeIndex(x, y, z));
        }

        public Tile TileAt(int x, int z) {
            return TileAt(new Tile.CubeIndex(x, z));
        }

        public List<Tile> Neighbours(Tile tile) {
            List<Tile> ret = new List<Tile>();
            Tile.CubeIndex o;

            for (int i = 0; i < 6; i++) {
                o = tile.index + directions[i];
                if (grid.ContainsKey(o.ToString()))
                    ret.Add(grid[o.ToString()]);
            }
            return ret;
        }

        public List<Tile> Neighbours(Tile.CubeIndex index) {
            return Neighbours(TileAt(index));
        }

        public List<Tile> Neighbours(int x, int y, int z) {
            return Neighbours(TileAt(x, y, z));
        }

        public List<Tile> Neighbours(int x, int z) {
            return Neighbours(TileAt(x, z));
        }

        public List<Tile> TilesInRange(Tile center, int range) {
            //Return tiles rnage steps from center, http://www.redblobgames.com/grids/hexagons/#range
            List<Tile> ret = new List<Tile>();
            Tile.CubeIndex o;

            for (int dx = -range; dx <= range; dx++) {
                for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++) {
                    o = new Tile.CubeIndex(dx, dy, -dx - dy) + center.index;
                    if (grid.ContainsKey(o.ToString()))
                        ret.Add(grid[o.ToString()]);
                }
            }
            return ret;
        }

        public List<Tile> TilesInRange(Tile.CubeIndex index, int range) {
            return TilesInRange(TileAt(index), range);
        }

        public List<Tile> TilesInRange(int x, int y, int z, int range) {
            return TilesInRange(TileAt(x, y, z), range);
        }

        public List<Tile> TilesInRange(int x, int z, int range) {
            return TilesInRange(TileAt(x, z), range);
        }

        public int Distance(Tile.CubeIndex a, Tile.CubeIndex b) {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }

        public int Distance(Tile a, Tile b) {
            return Distance(a.index, b.index);
        }

        public float SampleHeight(Vector3 worldPos) {
            float y = Terrain.activeTerrain.SampleHeight(worldPos) + Terrain.activeTerrain.GetPosition().y + 0.5f;
            return y;
        }

        public Tile ToTile(Vector3 pos) {
            Vector3 pt = new Vector3(pos.x / hexRadius, 0.0f, pos.z / hexRadius);
            switch (hexOrientation) {
                case HexOrientation.Pointy: {
                        double q = Math.Sqrt(3.0f) / 3.0 * pos.x + (-1.0 / 3.0f) * pt.z;
                        double r = 0.0f * pt.x + (2.0 / 3.0) * pt.z;
                        Tile.FractionalIndex hex = new Tile.FractionalIndex(q, r, -q - r);
                        Tile.CubeIndex cube = Tile.FractionalIndex.HexRound(hex);
                        UnityEngine.Debug.Log(cube.ToString());
                        return TileAt(cube);
                    }
                case HexOrientation.Flat: {
                        double q = 2.0 / 3.0f * pos.x + 0.0 * pt.z;
                        double r = -1.0 / 3.0f * pt.x + Mathf.Sqrt(3.0f) / 3.0f * pt.z;
                        Tile.FractionalIndex hex = new Tile.FractionalIndex(q, r, -q - r);
                        Tile.CubeIndex cube = Tile.FractionalIndex.HexRound(hex);
                        UnityEngine.Debug.Log(cube.ToString());
                        return TileAt(cube);
                    }
                default:
                    break;
            }
            return null;
        }

        public int FindPath(Vector3 start, Vector3 exit) {
            int i = 0;
            for (; i < 100; i++) {
                if (astars[i].Free) {
                    break;
                }
            }
            if (i >= 0 && i < 100) {
                Tile startTile = ToTile(start);
                Tile exitTile = ToTile(exit);
                if (exitTile == null) {
                    Debug.Log("exit tile is null");
                    return -1;
                }
                exitTile.LineColour(Color.red, Color.red);
                astars[i].Exit = exitTile;
                if (astars[i].Open == null) {
                    astars[i].Open = new List<TileWaypoint>();
                }
                if (astars[i].Open.Count > 0) {
                    astars[i].Open.Clear();
                }
                if (astars[i].Closed == null) {
                    astars[i].Closed = new List<TileWaypoint>();
                }
                if (astars[i].Closed.Count > 0) {
                    astars[i].Closed.Clear();
                }
                TileWaypoint star = new TileWaypoint();
                star.GCost = 0;
                star.HCost = TileWaypoint.Cost(start, exit);
                star.Tile = startTile;

                astars[i].Open.Add(star);
                astars[i].Open.Sort(star);

                while (astars[i].Open.Count > 0) {
                    TileWaypoint top = astars[i].Open[0];
                    astars[i].Open.Remove(top);
                    astars[i].Closed.Add(top);
                    if (top.Tile == astars[i].Exit) {
                        break;
                    }

                    UnityEngine.Debug.Log(top.Tile.index.ToString());

                    List<Tile> neighbours = Neighbours(top.Tile);
                    foreach (var item in neighbours) {
                        star = new TileWaypoint();
                        star.GCost = 0;
                        star.HCost = TileWaypoint.Cost(start, exit);
                        star.Tile = item;

                        if (astars[i].Open.Contains(star, star)) {
                            continue;
                        }

                        if (astars[i].Closed.Contains(star, star)) {
                            continue;
                        }

                        astars[i].Open.Add(star);
                        astars[i].Open.Sort(star);
                    }
                }
                // finish
                return i;
            }
            return -1;
        }

        public bool NextPoint(int pathid, ref Vector3 pos) {
            if (astars[pathid].Current < astars[pathid].Closed.Count) {
                pos = astars[pathid].Closed[astars[pathid].Current].Tile.position;
                astars[pathid].Current = astars[pathid].Current + 1;
                return true;
            } else {
                return false;
            }
        }
        #endregion

        #region Private Methods

        private void Start() {
            for (int i = 0; i < 100; i++) {
                astars[i] = new GridWaypointHead(i);
            }
        }

        private void OnEnable() {
            Tiles.Clear();
            for (int i = 0; i < transform.childCount; i++) {
                Transform t = transform.GetChild(i);
                Tile tile = t.GetComponent<Tile>();
                Tiles.Add(tile.index.ToString(), tile);
            }
        }

        private void GenHexShape() {
            Debug.Log("Generating hexagonal shaped grid...");

            Tile tile;
            Vector3 pos = Vector3.zero;

            int mapSize = Mathf.Max(mapWidth, mapHeight);

            for (int q = -mapSize; q <= mapSize; q++) {
                int r1 = Mathf.Max(-mapSize, -q - mapSize);
                int r2 = Mathf.Min(mapSize, -q + mapSize);
                for (int r = r1; r <= r2; r++) {
                    switch (hexOrientation) {
                        case HexOrientation.Flat:
                            pos.x = hexRadius * 3.0f / 2.0f * q;
                            pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                            break;

                        case HexOrientation.Pointy:
                            pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                            pos.z = hexRadius * 3.0f / 2.0f * r;
                            break;
                    }

                    tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                    tile.index = new Tile.CubeIndex(q, r, -q - r);
                    tile.position = pos;
                    grid.Add(tile.index.ToString(), tile);
                }
            }
        }

        private void GenRectShape() {
            Debug.Log("Generating rectangular shaped grid...");

            Tile tile;
            Vector3 pos = Vector3.zero;

            switch (hexOrientation) {
                case HexOrientation.Flat:
                    for (int q = 0; q < mapWidth; q++) {
                        int qOff = q >> 1;
                        for (int r = -qOff; r < mapHeight - qOff; r++) {
                            pos.x = hexRadius * 3.0f / 2.0f * q;
                            pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);

                            tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                            tile.index = new Tile.CubeIndex(q, r, -q - r);
                            tile.position = pos;
                            grid.Add(tile.index.ToString(), tile);
                        }
                    }
                    break;

                case HexOrientation.Pointy:
                    for (int r = 0; r < mapHeight; r++) {
                        int rOff = r >> 1;
                        for (int q = -rOff; q < mapWidth - rOff; q++) {
                            pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                            pos.z = hexRadius * 3.0f / 2.0f * r;

                            tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                            tile.index = new Tile.CubeIndex(q, r, -q - r);
                            tile.position = pos;
                            grid.Add(tile.index.ToString(), tile);
                        }
                    }
                    break;
            }
        }

        private void GenParrallShape() {
            Debug.Log("Generating parrellelogram shaped grid...");

            Tile tile;
            Vector3 pos = Vector3.zero;

            for (int q = 0; q <= mapWidth; q++) {
                for (int r = 0; r <= mapHeight; r++) {
                    switch (hexOrientation) {
                        case HexOrientation.Flat:
                            pos.x = hexRadius * 3.0f / 2.0f * q;
                            pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                            break;

                        case HexOrientation.Pointy:
                            pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                            pos.z = hexRadius * 3.0f / 2.0f * r;
                            break;
                    }

                    tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                    tile.index = new Tile.CubeIndex(q, r, -q - r);
                    tile.position = pos;
                    grid.Add(tile.index.ToString(), tile);
                }
            }
        }

        private void GenTriShape() {
            Debug.Log("Generating triangular shaped grid...");

            Tile tile;
            Vector3 pos = Vector3.zero;

            int mapSize = Mathf.Max(mapWidth, mapHeight);

            for (int q = 0; q <= mapSize; q++) {
                for (int r = 0; r <= mapSize - q; r++) {
                    switch (hexOrientation) {
                        case HexOrientation.Flat:
                            pos.x = hexRadius * 3.0f / 2.0f * q;
                            pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                            break;

                        case HexOrientation.Pointy:
                            pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                            pos.z = hexRadius * 3.0f / 2.0f * r;
                            break;
                    }

                    tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                    tile.index = new Tile.CubeIndex(q, r, -q - r);
                    tile.position = pos;
                    grid.Add(tile.index.ToString(), tile);
                }
            }
        }

        private Tile CreateHexGO(Vector3 postion, string name) {
            UnityEngine.Debug.Assert(postion.y == 0);
            //GameObject go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer), typeof(Tile));
            GameObject go = new GameObject(name, typeof(Tile));

            if (addColliders)
                go.AddComponent<MeshCollider>();

            if (drawOutlines)
                go.AddComponent<LineRenderer>();

            go.transform.position = postion;
            go.transform.parent = this.transform;
            go.layer = LayerMask.NameToLayer("Grid");

            Tile tile = go.GetComponent<Tile>();
            tile.grid = this;
            tile.orientation = hexOrientation;
            tile.AddHexMesh(hexRadius, hexOrientation);
            tile.SamepleHeight();

            //MeshFilter fil = go.GetComponent<MeshFilter>();
            //MeshRenderer ren = go.GetComponent<MeshRenderer>();

            //fil.sharedMesh = hexMesh;
            //ren.material = (hexMaterial)? hexMaterial : UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");

            if (addColliders) {
                MeshCollider col = go.GetComponent<MeshCollider>();
                col.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
            }

            if (drawOutlines) {
                tile.AddOutlines(hexRadius, hexOrientation);
            }

            tile.AddDesc();

            return tile;
        }
        #endregion

    }
}