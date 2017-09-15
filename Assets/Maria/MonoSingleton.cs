using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria {
    class MonoSingleton<T> : MonoBehaviour where T : new() {
    }
}
