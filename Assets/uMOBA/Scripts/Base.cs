// The Base entity type, used for buildings that can be destroyed.
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class Base : Entity {
    [Header("Health")]
    [SerializeField] int _hpMax = 1;
    public override int hpMax { get { return _hpMax; } }
    public override int mpMax { get { return 0; } }

    // other properties
    [Header("Defense")]
    [SyncVar, SerializeField] int baseDefense = 1;
    public override int defense { get { return baseDefense; } }
    public override int damage { get { return 0; } }

    [Header("Death")]
    [SerializeField] GameObject deathEffect;
    
    // networkbehaviour ////////////////////////////////////////////////////////
    [Server]
    public override void OnStartServer() {
        base.OnStartServer();

        // all buildings should spawn with full health and mana
        hp = hpMax;
        mp = mpMax;
    }

    [ClientCallback] // no need to do animations on the server
    void LateUpdate() {
        // pass parameters to animation state machine
        GetComponent<Animator>().SetInteger("Hp", hp);
    }

    // finite state machine states /////////////////////////////////////////////
    [Server] protected override string UpdateServer() { return state; }
    [Client] protected override void UpdateClient() {
        // did we just die? then load the death effect once
        if (hp == 0 && deathEffect) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            deathEffect = null;
        }
    }

    // skills //////////////////////////////////////////////////////////////////
    public override bool CanAttackType(System.Type t) { return false; }
}
