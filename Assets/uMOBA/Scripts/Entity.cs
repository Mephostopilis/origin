// The Entity class is rather simple. It contains a few basic entity properties
// like health, mana and level _(which are not public)_ and then offers several
// public functions to read and modify them.
// 
// Entities also have a _target_ Entity that can't be synchronized with a
// SyncVar. Instead we created a EntityTargetSync component that takes care of
// that for us.
// 
// Entities use a deterministic finite state machine to handle IDLE/MOVING/DEAD/
// CASTING etc. states and events. Using a deterministic FSM means that we react
// to every single event that can happen in every state (as opposed to just
// taking care of the ones that we care about right now). This means a bit more
// code, but it also means that we avoid all kinds of weird situations like 'the
// monster doesn't react to a dead target when casting' etc.
// The next state is always set with the return value of the UpdateServer
// function. It can never be set outside of it, to make sure that all events are
// truly handled in the state machine and not outside of it. Otherwise we may be
// tempted to set a state in CmdBeingTrading etc., but would likely forget of
// special things to do depending on the current state.
//
// Each entity needs two colliders. First of all, the proximity checks don't
// work if there is no collider on that same GameObject, hence why all Entities
// have a very small trigger BoxCollider on them. They also need a real trigger
// that always matches their position, so that Raycast selection works. The
// real trigger is always attached to the pelvis in the bone structure, so that
// it automatically follows the animation. Otherwise we wouldn't be able to
// select dead entities because their death animation often throws them far
// behind.
//
// Entities also need a kinematic Rigidbody so that OnTrigger functions can be
// called. Note that there is currently a Unity bug that slows down the agent
// when having lots of FPS(300+) if the Rigidbody's Interpolate option is
// enabled. So for now it's important to disable Interpolation - which is a good
// idea in general to increase performance.
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Utils = uMoba.Utils;

// Team Enum (we use an enum instead of a string so that changing a team name
// will force us to update the code in all places)
public enum Team {Good, Evil, Neutral};

// note: no animator required, towers, dummies etc. may not have one
[RequireComponent(typeof(Rigidbody))] // kinematic, only needed for OnTrigger
//[RequireComponent(typeof(NetworkProximityChecker))]
//[RequireComponent(typeof(NavMeshAgent))] // towers don't need them
//[RequireComponent(typeof(NetworkNavMeshAgent))] // towers don't need them
[RequireComponent(typeof(EntityTargetSync))]
public abstract class Entity : NetworkBehaviour {
    // finite state machine
    // -> state only writable by entity class to avoid all kinds of confusion
    [Header("State")]
    [SyncVar, SerializeField] private string _state = "IDLE";
    public string state { get { return _state; } }

    // [SyncVar] NetworkIdentity wouldnt work because targets can get null if
    // they disappear or disconnect, which would result in unet exceptions.
    [Header("Target")]
    public Entity target;

    [Header("Level")]
    [SyncVar] public int level = 1;
    
    [Header("Health")]
    [SyncVar, SerializeField] protected bool hpRecovery = true; // can be disabled in combat etc.
    [SyncVar, SerializeField] protected int hpRecoveryRate = 1; // per second
    [SyncVar                ] private int _hp = 1;
    public int hp {
        get { return Mathf.Min(_hp, hpMax); } // min in case hp>hpmax after buff ends etc.
        set { _hp = Mathf.Clamp(value, 0, hpMax); }
    }
    public abstract int hpMax{ get; }
    [SerializeField] Entity[] invincibleWhileAllAlive; // base invincible while barracks exist etc.

    [Header("Mana")]
    [SyncVar, SerializeField] protected bool mpRecovery = true; // can be disabled in combat etc.
    [SyncVar, SerializeField] protected int mpRecoveryRate = 1; // per second
    [SyncVar                ] private int _mp = 1;
    public int mp {
        get { return Mathf.Min(_mp, mpMax); } // min in case hp>hpmax after buff ends etc.
        set { _mp = Mathf.Clamp(value, 0, mpMax); }
    }
    public abstract int mpMax{ get; }

    [Header("Team")] // to only attack different team etc.
    [SyncVar] public Team team = Team.Good;

    [Header("Popups")]
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] GameObject goldPopupPrefab;

    // other properties
    public float speed { get{ return agent.speed; } }
    public abstract int damage { get; }
    public abstract int defense { get; }

    // skill system for all entities (players, monsters, npcs, towers, ...)
    // 'skillTemplates' are the available skills (first one is default attack)
    // 'skills' are the loaded skills with cooldowns etc.
    [Header("Skills, Buffs, Status Effects")]
    public SkillTemplate[] skillTemplates;
    public SyncListSkill skills = new SyncListSkill();
    // current skill (synced because we need it as an animation parameter)
    [SyncVar] protected int skillCur = -1;

    // cache team members to avoid FindObjectsOfType usage
    // for NetworkProximityCheckerTeam and FogOfWar, which would be very costly
    public static Dictionary<Team, HashSet<Entity>> teams = new Dictionary<Team, HashSet<Entity>>() {
        {Team.Good, new HashSet<Entity>()},
        {Team.Evil, new HashSet<Entity>()},
        {Team.Neutral, new HashSet<Entity>()}
    };

    // cache
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector] public NetworkProximityChecker proxchecker;
    [HideInInspector] public NetworkIdentity netIdentity;
    [HideInInspector] new public Collider collider;

    // networkbehaviour ////////////////////////////////////////////////////////
    // cache components on server and clients
    protected virtual void Awake() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        proxchecker = GetComponent<NetworkProximityChecker>();
        netIdentity = GetComponent<NetworkIdentity>();
        // the collider can also be a child in case of animated entities (where
        // it sits on the pelvis for example). equipment colliders etc. aren't
        // a problem because they are added after awake in case
        collider = GetComponentInChildren<Collider>();
    }

    [Server]
    public override void OnStartServer() {
        // health recovery every second
        InvokeRepeating("Recover", 1, 1);

        // HpDecreaseBy changes to "DEAD" state when hp drops to 0, but there is
        // a case where someone might instantiated a Entity with hp set to 0,
        // hence we have to check that case once at start
        if (hp == 0) _state = "DEAD";
        
        // load skills based on skill templates
        foreach (var t in skillTemplates)
            skills.Add(new Skill(t));
    }

    void Start() {
        teams[team].Add(this);
    }

    void OnDestroy() {
        teams[team].Remove(this);
    }

    // entity logic will be implemented with a finite state machine. we will use
    // UpdateIDLE etc. so that we have a 100% guarantee that it works properly
    // and we never miss a state or update two states after another
    // note: can still use LateUpdate for Updates that should happen in any case
    // -> we can even use parameters if we need them some day.
    void Update() {
        // unlike uMMORPG, all entities have to be updated at all times in uMOBA
        // because monster's must always run towards the enemy base, even if no
        // one is around.
        if (isClient) UpdateClient();
        if (isServer) _state = UpdateServer();
    }

    // update for server. should return the new state.
    protected abstract string UpdateServer();

    // update for client.
    protected abstract void UpdateClient();

    // visibility //////////////////////////////////////////////////////////////
    // hide a entity
    // note: using SetActive won't work because its not synced and it would
    //       cause inactive objects to not receive any info anymore
    // note: this won't be visible on the server as it always sees everything.
    [Server]
    public void Hide() {
        proxchecker.forceHidden = true;
    }

    [Server]
    public void Show() {
        proxchecker.forceHidden = false;
    }

    // is the entity currently hidden?
    // note: usually the server is the only one who uses forceHidden, the
    //       client usually doesn't know about it and simply doesn't see the
    //       GameObject.
    public bool IsHidden() {
        return proxchecker.forceHidden;
    }

    public float VisRange() {
        return proxchecker.visRange;
    }

    // look at a transform while only rotating on the Y axis (to avoid weird
    // tilts)
    public void LookAtY(Vector3 pos) {
        transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
    }
    
    // note: client can find out if moving by simply checking the state!
    [Server] // server is the only one who has up-to-date NavMeshAgent
    public bool IsMoving() {
        // -> agent.hasPath will be true if stopping distance > 0, so we can't
        //    really rely on that.
        // -> pathPending is true while calculating the path, which is good
        // -> remainingDistance is the distance to the last path point, so it
        //    also works when clicking somewhere onto a obstacle that isn'
        //    directly reachable.
        return agent.pathPending ||
               agent.remainingDistance > agent.stoppingDistance ||
               agent.velocity != Vector3.zero;
    }

    // health & mana ///////////////////////////////////////////////////////////
    public float HpPercent() {
        return (hp != 0 && hpMax != 0) ? (float)hp / (float)hpMax : 0.0f;
    }

    [Server]
    public void Revive() {
        hp = hpMax;
    }
    
    public float MpPercent() {
        return (mp != 0 && mpMax != 0) ? (float)mp / (float)mpMax : 0.0f;
    }

    // bases have the ability to be invincible while ALL of the 3 barracks exist
    // users can also make barracks invincible while a tower exists etc.
    [Server]
    public bool IsInvincible() {
        // return true if all are still alive, false otherwise
        // (also false if the list is empty)
        return invincibleWhileAllAlive.Length > 0 &&
               invincibleWhileAllAlive.All(e => e != null && e.hp > 0);
    }

    // combat //////////////////////////////////////////////////////////////////
    // no need to instantiate damage popups on the server
    [ClientRpc]
    public void RpcShowDamagePopup(int amount, Vector3 pos) {
        // spawn the damage popup (if any) and set the text
        if (damagePopupPrefab) {
            var popup = (GameObject)Instantiate(damagePopupPrefab, pos, Quaternion.identity);
            popup.GetComponentInChildren<TextMesh>().text = amount.ToString();
        }
    }

    // no need to instantiate gold popups on the server
    [ClientRpc]
    public void RpcShowGoldPopup(int amount, Vector3 pos) {
        // spawn the gold popup (if any) and set the text
        if (goldPopupPrefab) {
            var popup = (GameObject)Instantiate(goldPopupPrefab, pos, Quaternion.identity);
            popup.GetComponentInChildren<TextMesh>().text = "+" + amount.ToString();
        }
    }    

    // deal damage at another entity
    // (can be overwritten for players etc. that need custom functionality)
    // (can also return the set of entities that were hit, just in case they are
    //  needed when overwriting it etc.)
    [Server]
    public virtual HashSet<Entity> DealDamageAt(Entity entity, int n, float aoeRadius=0f) {
        // build the set of entities that were hit within AoE range
        var entities = new HashSet<Entity>();

        // add main target in any case, because non-AoE skills have radius=0
        entities.Add(entity);

        // add all targets in AoE radius around main target
        var colliders = Physics.OverlapSphere(entity.transform.position, aoeRadius); //, layerMask);
        foreach (var c in colliders) {
            var candidate = c.GetComponentInParent<Entity>();
            // overlapsphere cast uses the collider's bounding volume (see
            // Unity scripting reference), hence is often not exact enough
            // in our case (especially for radius 0.0). let's also check the
            // distance to be sure.
            if (candidate != null && candidate != this && candidate.hp > 0 &&
                Vector3.Distance(entity.transform.position, candidate.transform.position) < aoeRadius)
                entities.Add(candidate);
        }

        // now deal damage at each of them
        foreach (var e in entities) {
            // subtract defense (but leave at least 1 damage, otherwise it may be
            // frustrating for weaker players)
            // [dont deal any damage if invincible]
            var dmg = !e.IsInvincible() ? Mathf.Max(n-e.defense, 1) : 0;
            e.hp -= dmg;

            // show damage popup in observers via ClientRpc
            // showing them above their head looks best, and we don't have to
            // use a custom shader to draw world space UI in front of the entity
            // note: we send the RPC to ourselves because whatever we killed
            //       might disappear before the rpc reaches it
            var bounds = e.GetComponentInChildren<Collider>().bounds;
            RpcShowDamagePopup(dmg, new Vector3(bounds.center.x, bounds.max.y, bounds.center.z));
        }

        return entities;
    }

    // recovery ////////////////////////////////////////////////////////////////
    // receover health and mana once a second
    // (can be overwritten for players etc. that need custom functionality)
    // note: when stopping the server with the networkmanager gui, it will
    //       generate warnings that Recover was called on client because some
    //       entites will only be disabled but not destroyed. let's not worry
    //       about that for now.
    [Server]
    public virtual void Recover() {
        if (enabled && hp > 0) {
            if (hpRecovery) hp += hpRecoveryRate;
            if (mpRecovery) mp += mpRecoveryRate;
        }
    }

    // aggro ///////////////////////////////////////////////////////////////////
    // this function is called by the AggroArea (if any) on clients and server
    public virtual void OnAggro(Entity entity) {}

    // skill system ////////////////////////////////////////////////////////////
    // we can't have a public array of types that we can modify in the Inspector
    // so we need an abstract function to check if players can attack players,
    // monsters, npcs etc.
    public abstract bool CanAttackType(System.Type t);

    // the first check validates the caster
    // (the skill won't be ready if we check self while casting it. so the
    //  checkSkillReady variable can be used to ignore that if needed)
    public bool CastCheckSelf(Skill skill, bool checkSkillReady = true) {
        // no cooldown, hp, mp?
        return (!checkSkillReady || skill.IsReady()) &&
               hp > 0 &&
               mp >= skill.manaCosts;
    }

    // the second check validates the target and corrects it for the skill if
    // necessary (e.g. when trying to heal an npc, it sets target to self first)
    public bool CastCheckTarget(Skill skill) {
        // attack: target exists, alive, not self, different team, oktype
        // (we can't have a public array of types that we can modify
        //  in the Inspector, so we need an abstract function)
        if (skill.category == "Attack") {
            return target != null &&
                   target != this &&
                   target.hp > 0 &&
                   target.team != team &&
                   CanAttackType(target.GetType());
        // heal: on target? (if exists, not self, team, type) or self
        } else if (skill.category == "Heal") {
            if (target != null &&
                target != this &&
                target.team == team &&
                target.GetType() == this.GetType()) {
                // can only heal the target if it's not dead 
                return target.hp > 0;
            // otherwise we want to heal ourselves, which is always allowed
            // (we already checked if we are alive in castcheckself)
            } else {
                target = this;
                return true;
            }
        // buff: only buff self => ok
        } else if (skill.category == "Buff") {
            target = this;
            return true;
        }
        // otherwise the category is invalid
        Debug.LogWarning("invalid skill category for: " + skill.name);
        return false;
    }

    // the third check validates the distance between the caster and the target
    // (in case of buffs etc., the target was already corrected to 'self' by
    //  castchecktarget, hence we don't have to worry about anything here)
    public bool CastCheckDistance(Skill skill) {
        return target != null &&
               Utils.ClosestDistance(collider, target.collider) <= skill.castRange;
    }

    // applies the skill effects. casting and waiting has to be done in the
    // state machine
    public void CastSkill(Skill skill) {
        // check self again (alive, mana, weapon etc.). ignoring the skill cd
        // and check target again
        // note: we don't check the distance again. the skill will be cast even
        // if the target walked a bit while we casted it (it's simply better
        // gameplay and less frustrating)
        if (CastCheckSelf(skill, false) && CastCheckTarget(skill)) {
            // attack
            if (skill.category == "Attack") {
                // decrease mana in any case
                mp -= skill.manaCosts;

                // deal damage directly or shoot a projectile?
                if (skill.projectile == null) {
                    // deal damage directly
                    DealDamageAt(target, damage + skill.damage, skill.aoeRadius);
                } else {
                    // spawn the projectile and shoot it towards target
                    // (make sure that the weapon prefab has a ProjectileMount
                    //  somewhere in the hierarchy)
                    var pm = transform.FindRecursively("ProjectileMount");
                    if (pm != null) {
                        var pos = pm.position;
                        var go = (GameObject)Instantiate(skill.projectile.gameObject, pos, Quaternion.identity);
                        var proj = go.GetComponent<Projectile>();
                        proj.target = target;
                        proj.caster = this;
                        proj.damage = damage + skill.damage;
                        proj.aoeRadius = skill.aoeRadius;
                        NetworkServer.Spawn(go);
                    } else {
                        Debug.LogWarning(name + " has no ProjectileMount. Can't fire the projectile.");
                    }
                }
            // heal
            } else if (skill.category == "Heal") {
                // note: 'target alive' checks were done above already
                mp -= skill.manaCosts;
                target.hp += skill.healsHp;
                target.mp += skill.healsMp;
            // buff
            } else if (skill.category == "Buff") {
                // set the buff end time (the rest is done in .damage etc.)
                mp -= skill.manaCosts;
                skill.buffTimeEnd = Time.time + skill.buffTime;
            }
            
            // start the cooldown (and save it in the struct)
            skill.cooldownEnd = Time.time + skill.cooldown;

            // save any skill modifications in any case
            skills[skillCur] = skill;
        } else {
            // not all requirements met. no need to cast the same skill again
            skillCur = -1;
        }
    }

    // helper function to stop all buffs if needed (e.g. in OnDeath)
    public void StopBuffs() {
        for (int i = 0; i < skills.Count; ++i) {
            if (skills[i].category == "Buff") { // not for Murder status etc.
                var skill = skills[i];
                skill.buffTimeEnd = Time.time;
                skills[i] = skill;
            }
        }
    }
}
