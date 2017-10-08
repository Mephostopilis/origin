using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map {
    public class TileWaypoint : IComparer<TileWaypoint>, IEqualityComparer<TileWaypoint> {
        public int MoveCost { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get { return GCost + HCost; } }
        public TileWaypoint Parent { get; set; }

        public Tile Tile { get; set; }

        public int Compare(TileWaypoint x, TileWaypoint y) {
            return x.FCost - y.FCost;
        }

        public static int Cost(Vector3 start, Vector3 exit) {
            return (int)(Mathf.Abs(exit.x - start.x) + Mathf.Abs(exit.z - start.z));
        }

        public bool Equals(TileWaypoint x, TileWaypoint y) {
            return (x.Tile == y.Tile);
        }

        public int GetHashCode(TileWaypoint obj) {
            throw new NotImplementedException();
        }
    }
}