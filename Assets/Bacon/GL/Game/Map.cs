using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Bacon;

namespace Bacon.GL.Game {
    class Map : UnityEngine.MonoBehaviour {
        void Start() {
            Command cmd = new Command(Bacon.Event.MyEventCmd.EVENT_SETUP_MAP);
            Bacon.GL.Util.App.current.Enqueue(cmd);
        }
    }
}
