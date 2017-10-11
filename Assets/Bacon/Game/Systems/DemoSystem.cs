using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public sealed class DemoSystem : Entitas.ISystem, Entitas.IInitializeSystem, Entitas.IExecuteSystem {


    public void Initialize() {
        UnityEngine.Debug.Log("DemoSystem Initialize");
    }

    public void Execute() {
        throw new NotImplementedException();
    }

}

