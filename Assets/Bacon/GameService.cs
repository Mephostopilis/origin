using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Bacon  {
    class GameService : Service {

        public static readonly string Name = "game";

        private long _mysession = 0;
        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();
        
        private Hashtable _hasht = new Hashtable();

        public GameService(Context ctx) : base(ctx) {
        }

        public object this[string key] {
            get {
                return _hasht[key];
            }
        }

        public void AddPlayer(long session, Player player) {
            _playes[session] = player;
        }

        public void AddMyPlayer(long session, Player player) {
            _mysession = session;
            _playes[session] = player;
        }

        public int RoomId { get; set; }

    }
}
