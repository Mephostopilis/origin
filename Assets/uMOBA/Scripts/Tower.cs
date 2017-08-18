// The Tower entity type. Automatically attacks entities from the opposite team.
using UnityEngine;
using UnityEngine.Networking;

public class Tower : Entity {
    [Header("Health")]
    [SerializeField] int _hpMax = 200;
    public override int hpMax { get { return _hpMax; } }
    public override int mpMax { get { return 0; } }

    // other properties
    [Header("Damage & Defense")]
    [SyncVar, SerializeField] int baseDamage = 1;
    public override int damage { get { return baseDamage; } }

    [SyncVar, SerializeField] int baseDefense = 1;
    public override int defense { get { return baseDefense; } }

    [Header("Reward")]
    public long rewardExp = 20;
    public int rewardGold = 20;
    
    // networkbehaviour ////////////////////////////////////////////////////////
    [Server]
    public override void OnStartServer() {
        base.OnStartServer();

        // all towers should spawn with full health and mana
        hp = hpMax;
        mp = mpMax;
    }

    // finite state machine events /////////////////////////////////////////////
    bool EventDied() {
        return hp == 0;
    }

    bool EventTargetDisappeared() {
        return target == null;
    }

    bool EventTargetDied() {
        return target != null && target.hp == 0;
    }

    bool EventTargetTooFarToAttack() {
        return target != null &&
               0 <= skillCur && skillCur < skills.Count &&
               !CastCheckDistance(skills[skillCur]);
    }

    bool EventAggro() {
        return target != null && target.hp > 0;
    }
    
    bool EventSkillRequest() {
        return 0 <= skillCur && skillCur < skills.Count;        
    }
    
    bool EventSkillFinished() {
        return 0 <= skillCur && skillCur < skills.Count &&
               skills[skillCur].CastTimeRemaining() == 0f;        
    }

    // finite state machine - server ///////////////////////////////////////////
    [Server]
    string UpdateServer_IDLE() {
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            skillCur = -1; // in case we died while trying to cast
            return "DEAD";
        }
        if (EventTargetTooFarToAttack()) {
            // invalid target. stop trying to cast.
            target = null;
            skillCur = -1;
            return "IDLE";
        }
        if (EventSkillRequest()) {
            // we had a target in attack range before and trying to cast a skill
            // on it. check self (alive, mana, weapon etc.) and target
            var skill = skills[skillCur];
            if (CastCheckSelf(skill) && CastCheckTarget(skill)) {
                // start casting and set the casting end time
                skill.castTimeEnd = Time.time + skill.castTime;
                skills[skillCur] = skill;
                return "CASTING";
            } else {
                // invalid target. stop trying to cast.
                target = null;
                skillCur = -1;
                return "IDLE";
            }
        }
        if (EventAggro()) {
            // target in attack range. try to cast a first skill on it
            if (skills.Count > 0) skillCur = 0;
            else Debug.LogError(name + " has no skills to attack with.");
            return "IDLE";
        }
        if (EventTargetDied()) {} // don't care
        if (EventTargetDisappeared()) {} // don't care
        if (EventSkillFinished()) {} // don't care

        return "IDLE"; // nothing interesting happened
    }
    
    [Server]
    string UpdateServer_CASTING() {
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            skillCur = -1; // in case we died while trying to cast
            return "DEAD";
        }
        if (EventTargetDisappeared()) {
            // target disappeared, stop casting
            skillCur = -1;
            target = null;
            return "IDLE";
        }
        if (EventTargetDied()) {
            // target died, stop casting
            skillCur = -1;
            target = null;
            return "IDLE";
        }
        if (EventSkillFinished()) {
            // finished casting. apply the skill on the target.
            CastSkill(skills[skillCur]);

            // did the target die? then clear it so that the monster doesn't
            // run towards it if the target respawned
            if (target.hp == 0) target = null;
            
            // go back to IDLE
            skillCur = -1;
            return "IDLE";
        }
        if (EventTargetTooFarToAttack()) {} // don't care, we were close enough when starting to cast
        if (EventAggro()) {} // don't care, always have aggro while casting
        if (EventSkillRequest()) {} // don't care, that's why we are here
        
        return "CASTING"; // nothing interesting happened
    }
    
    [Server]
    string UpdateServer_DEAD() {
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventSkillRequest()) {} // don't care
        if (EventSkillFinished()) {} // don't care
        if (EventTargetDisappeared()) {} // don't care
        if (EventTargetDied()) {} // don't care
        if (EventTargetTooFarToAttack()) {} // don't care
        if (EventAggro()) {} // don't care
        if (EventDied()) {} // don't care
        return "DEAD";
    }

    [Server]
    protected override string UpdateServer() {
        if (state == "IDLE")    return UpdateServer_IDLE();
        if (state == "CASTING") return UpdateServer_CASTING();
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

    // aggro ///////////////////////////////////////////////////////////////////
    [ServerCallback] // called by AggroArea from servers and clients
    public override void OnAggro(Entity entity) {
        // alive? (dead entities have colliders too) and different team?
        if (entity && entity.hp > 0 && entity.team != team && CanAttackType(entity.GetType())) {
            // no target yet(==self), or closer than current target?
            // use collider point(s) to also work with big entities
            if (target == null || Utils.ClosestDistance(collider, entity.collider) < Utils.ClosestDistance(collider, target.collider))
                target = entity;
        }
    }

    // skills //////////////////////////////////////////////////////////////////
    public override bool CanAttackType(System.Type t) {
        return t == typeof(Monster) || t == typeof(Player);
    }
}
