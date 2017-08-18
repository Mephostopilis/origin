// The Barrack entity type.
using UnityEngine;
using UnityEngine.Networking;

public class Barrack : Entity {
    [Header("Health")]
    [SerializeField] int _hpMax = 400;
    public override int hpMax { get { return _hpMax; } }
    public override int mpMax { get { return 0; } }

    // other properties
    [Header("Damage & Defense")]
    [SyncVar, SerializeField] int baseDefense = 4;
    public override int defense { get { return baseDefense; } }

    public override int damage { get { return 0; } }

    [Header("Reward")]
    public long rewardExp = 40;
    public int rewardGold = 40;

    // networkbehaviour ////////////////////////////////////////////////////////
    [Server]
    public override void OnStartServer() {
        base.OnStartServer();

        // all barracks should spawn with full health and mana
        hp = hpMax;
        mp = mpMax;
    }

    // finite state machine events /////////////////////////////////////////////
    bool EventDied() {
        return hp == 0;
    }

    // finite state machine - server ///////////////////////////////////////////
    [Server]
    string UpdateServer_IDLE() {
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            return "DEAD";
        }        

        return "IDLE"; // nothing interesting happened
    }
        
    [Server]
    string UpdateServer_DEAD() {
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {} // don't care
        return "DEAD";
    }

    [Server]
    protected override string UpdateServer() {
        if (state == "IDLE")    return UpdateServer_IDLE();
        if (state == "DEAD")    return UpdateServer_DEAD();
        Debug.LogError("invalid state:" + state);
        return "IDLE";
    }

    // finite state machine - client ///////////////////////////////////////////
    [Client]
    protected override void UpdateClient() {}

    [Server]
    void OnDeath() {
        // disappear forever
        NetworkServer.Destroy(gameObject);
    }

    // skills //////////////////////////////////////////////////////////////////
    public override bool CanAttackType(System.Type t) {
        return false;
    }
}
