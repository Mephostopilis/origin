using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map {
    public class TileAStar : IComparer<TileAStar>, IEqualityComparer<TileAStar> {
        public int MoveCost { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get { return GCost + HCost; } }
        public TileAStar Parent { get; set; }

        public Tile Tile { get; set; }

        public int Compare(TileAStar x, TileAStar y) {
            return x.FCost - y.FCost;
        }

        public static int Cost(Vector3 start, Vector3 exit) {
            return (int)(Mathf.Abs(exit.x - start.x) + Mathf.Abs(exit.z - start.z));
        }

        public bool Equals(TileAStar x, TileAStar y) {
            return (x.Tile == y.Tile);
        }

        public int GetHashCode(TileAStar obj) {
            throw new NotImplementedException();
        }
    }
}