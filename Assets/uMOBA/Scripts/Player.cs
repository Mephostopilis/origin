// All player logic was put into this class. We could also split it into several
// smaller components, but this would result in many GetComponent calls and a
// more complex syntax.
//
// The default Player class takes care of the basic player logic like the state
// machine and some properties like damage and defense.
//
// The Player class stores the maximum experience for each level in a simple
// array. So the maximum experience for level 1 can be found in expMax[0] and
// the maximum experience for level 2 can be found in expMax[1] and so on. The
// player's health and mana are also level dependent in most MMORPGs, hence why
// there are hpMax and mpMax arrays too. We can find out a players's max health
// in level 1 by using hpMax[0] and so on.
//
// The class also takes care of selection handling, which detects 3D world
// clicks and then targets/navigates somewhere/interacts with someone.
//
// Animations are not handled by the NetworkAnimator because it's still very
// buggy and because it can't really react to movement stops fast enough, which
// results in moonwalking. Not synchronizing animations over the network will
// also save us bandwidth.
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils = uMoba.Utils;

[RequireComponent(typeof(NetworkName))]
[RequireComponent(typeof(PlayerChat))]
[RequireComponent(typeof(Animator))]
public class Player : Entity {
    [Header("Health")]
    [SerializeField] int[] _hpMax = {100, 110, 120};
    public override int hpMax {
        get {
            // calculate item bonus
            var itemBonus = (from item in inventory
                             where item.valid
                             select item.equipHpBonus).Sum();

            // calculate buff bonus
            var buffBonus = (from skill in skills
                             where skill.BuffTimeRemaining() > 0
                             select skill.buffsHpMax).Sum();

            // return base + attribute + equip + buffs
            return _hpMax[level-1] + itemBonus + buffBonus;
        }
    }
    
    [Header("Mana")]
    [SerializeField] int[] _mpMax = {10, 20, 30};
    public override int mpMax {
        get {
            // calculate item bonus
            var itemBonus = (from item in inventory
                             where item.valid
                             select item.equipMpBonus).Sum();

            // calculate buff bonus
            var buffBonus = (from skill in skills
                             where skill.BuffTimeRemaining() > 0
                             select skill.buffsMpMax).Sum();
            
            // return base + attribute + equip + buffs
            return _mpMax[level-1] + itemBonus + buffBonus;
        }
    }

    [Header("Damage")]
    [SyncVar] public int baseDamage = 1;
    public override int damage {
        get {
            // calculate item bonus
            var itemBonus = (from item in inventory
                             where item.valid
                             select item.equipDamageBonus).Sum();

            // calculate buff bonus
            var buffBonus = (from skill in skills
                             where skill.BuffTimeRemaining() > 0
                             select skill.buffsDamage).Sum();
            
            // return base + equip + buffs
            return baseDamage + itemBonus + buffBonus;
        }
    }

    [Header("Defense")]
    [SyncVar] public int baseDefense = 1;
    public override int defense {
        get {
            // calculate item bonus
            var itemBonus = (from item in inventory
                         where item.valid
                         select item.equipDefenseBonus).Sum();

            // calculate buff bonus
            var buffBonus = (from skill in skills
                             where skill.BuffTimeRemaining() > 0
                             select skill.buffsDefense).Sum();
            
            // return base + equip + buffs
            return baseDefense + itemBonus + buffBonus;
        }
    }

    [Header("Experience")] // note: int is not enough (can have > 2 mil. easily)
    [SyncVar, SerializeField] long _exp = 0;
    public long exp {
        get { return _exp; }
        set {
            if (value <= exp) {
                // decrease
                _exp = Utils.MaxLong(value, 0);
            } else {
                // increase with level ups
                // set the new value (which might be more than expMax)
                _exp = value;

                // now see if we leveled up (possibly more than once too)
                // (can't level up if already max level)
                while (_exp >= expMax && level < levelMax) {
                    // subtract current level's required exp, then level up
                    _exp -= expMax;
                    ++level;
                }

                // set to expMax if there is still too much exp remaining
                if (_exp > expMax) _exp = expMax;
            }
        }
    }
    [SerializeField] long[] _expMax = {10, 20, 30};
    public long expMax { get { return _expMax[level-1]; } }
    public int levelMax { get { return _expMax.Length; } }
    
    [Header("Indicator")]
    [SerializeField] GameObject indicatorPrefab;
    GameObject indicator;

    [Header("Inventory")]
    public int inventorySize = 30;
    public SyncListItem inventory = new SyncListItem();
    public ItemTemplate[] defaultItems;
    public KeyCode[] inventoryHotkeys = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6};

    [Header("Gold")] // note: int is not enough (can have > 2 mil. easily)
    [SerializeField, SyncVar] long _gold = 0;
    public long gold { get { return _gold; } set { _gold = Utils.MaxLong(value, 0); } }

    // 'skillTemplates' holds all skillTemplates and can be modified in the
    // Inspector 'skills' holds the dynamic skills that were based on the
    // skillTemplates (with cooldown, learned, etc.)
    // -> 1 default attack and 4 learnable skills
    [Header("Skillbar")]
    public KeyCode[] skillHotkeys = new KeyCode[] {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};

    [Header("Loot")]
    public float lootRange = 4f;

    [Header("Interaction")]
    public float talkRange = 4f;
    [SerializeField] KeyCode focusKey = KeyCode.Space;

    [Header("Respawn")]
    [SerializeField] float deathTime = 10f; // enough for animation
    float deathTimeEnd;
    [SerializeField] float respawnTime = 10f;
    [SyncVar, HideInInspector] public  float respawnTimeEnd; // syncvar for UI
    [SerializeField] float deathExpLossPercent = 0.05f;
    [SerializeField] float deathGoldLossPercent = 0.30f;

    // the next skill to be set if we try to set it while casting
    int skillNext = -1;

    // the next target to be set if we try to set it while casting
    Entity targetNext = null;

    // the selected skill that is pending while the player selects a target
    [HideInInspector] public int skillWanted = -1; // client sided

    // networkbehaviour ////////////////////////////////////////////////////////
    protected override void Awake() {
        // cache base components
        base.Awake();
    }

    public override void OnStartLocalPlayer() {
        // setup camera target
        Camera.main.GetComponent<CameraScrolling>().FocusOn(transform.position);
    }

    public override void OnStartServer() {
        base.OnStartServer();

        // all players should spawn with full health and mana
        hp = hpMax;
        mp = mpMax;

        // load inventory
        for (int i = 0; i < inventorySize; ++i)
            if (i < defaultItems.Length)
                inventory.Add(new Item(defaultItems[i]));
            else
                inventory.Add(new Item());
    }

    [ClientCallback] // no need to do animations on the server
    void LateUpdate() {
        // pass parameters to animation state machine
        // note: 'Speed' is the ONLY reliable parameter for movement animations.
        //       'RemainingDistance' has nothing to do with animations, since it
        //       is often tiny like 0.1 when following a target with full speed.
        GetComponent<Animator>().SetInteger("Hp", hp);
        GetComponent<Animator>().SetFloat("Speed", agent.velocity.magnitude);
        GetComponent<Animator>().SetInteger("skillCur", skillCur);
    }

    void OnDestroy() {
        // note: this function isn't called if it has a [ClientCallback] tag,
        // so let's use isLocalPlayer etc.
        // note: trying to do this in OnNetworkDestroy doesn't work well
        if (isLocalPlayer) Destroy(indicator);
    }

    // finite state machine events - status based //////////////////////////////
    // status based events
    bool EventDied() {
        return hp == 0;
    }

    bool EventDeathTimeElapsed() {
        return state == "DEAD" && Time.time + NetworkTime.offset >= deathTimeEnd;
    }

    bool EventRespawnTimeElapsed() {
        return state == "DEAD" && Time.time + NetworkTime.offset >= respawnTimeEnd;
    }

    bool EventTargetDisappeared() {
        return target == null;
    }

    bool EventTargetDied() {
        return target != null && target.hp == 0;
    }
    
    bool EventSkillRequest() {
        return 0 <= skillCur && skillCur < skills.Count;        
    }
    
    bool EventSkillFinished() {
        return 0 <= skillCur && skillCur < skills.Count &&
               skills[skillCur].CastTimeRemaining() == 0f;        
    }

    bool EventMoveEnd() {
        return state == "MOVING" && !IsMoving();
    }

    // finite state machine events - command based /////////////////////////////
    // client calls command, command sets a flag, event reads and resets it
    // => we use a set so that we don't get ultra long queues etc.
    // => we use set.Return to read and clear values
    HashSet<string> cmdEvents = new HashSet<String>();
    
    [Command] public void CmdCancelAction() { cmdEvents.Add("CancelAction"); }
    bool EventCancelAction() { return cmdEvents.Remove("CancelAction"); }

    Vector3 navigatePos = Vector3.zero;
    float navigateStop = 0;
    [Command] public void CmdNavigateTo(Vector3 pos, float stoppingDistance) {
        navigatePos = pos; navigateStop = stoppingDistance;
        cmdEvents.Add("NavigateTo");
    }
    bool EventNavigateTo() { return cmdEvents.Remove("NavigateTo"); }

    // finite state machine - server ///////////////////////////////////////////
    [Server]
    string UpdateServer_IDLE() {        
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            skillCur = skillNext = -1; // in case we died while trying to cast
            return "DEAD";
        }
        if (EventCancelAction()) {
            // the only thing that we can cancel is the target
            target = null;
            return "IDLE";
        }
        if (EventNavigateTo()) {
            // cancel casting (if any) and start moving
            skillCur = skillNext = -1;
            // move
            agent.stoppingDistance = navigateStop;
            agent.destination = navigatePos;
            return "MOVING";
        }
        if (EventSkillRequest()) {
            // user wants to cast a skill.            
            // check self (alive, mana, weapon etc.) and target
            var skill = skills[skillCur];
            targetNext = target; // return to this one after any corrections by CastCheckTarget
            if (CastCheckSelf(skill) && CastCheckTarget(skill)) {
                // check distance between self and target
                if (CastCheckDistance(skill)) {
                    // start casting and set the casting end time
                    skill.castTimeEnd = Time.time + skill.castTime;
                    skills[skillCur] = skill;
                    return "CASTING";
                } else {
                    // move to the target first
                    // (use collider point(s) to also work with big entities)
                    agent.stoppingDistance = skill.castRange;
                    agent.destination = target.collider.ClosestPointOnBounds(transform.position);
                    return "MOVING";
                }
            } else {
                // checks failed. stop trying to cast.
                skillCur = skillNext = -1;
                return "IDLE";
            }
        }
        if (EventSkillFinished()) {} // don't care
        if (EventMoveEnd()) {} // don't care
        if (EventDeathTimeElapsed()) {} // don't care
        if (EventRespawnTimeElapsed()) {} // don't care
        if (EventTargetDied()) {} // don't care
        if (EventTargetDisappeared()) {} // don't care

        return "IDLE"; // nothing interesting happened
    }

    [Server]
    string UpdateServer_MOVING() {        
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            skillCur = skillNext = -1; // in case we died while trying to cast
            return "DEAD";
        }
        if (EventMoveEnd()) {
            // finished moving. do whatever we did before.
            return "IDLE";
        }
        if (EventCancelAction()) {
            // cancel casting (if any) and stop moving
            skillCur = skillNext = -1;
            agent.ResetPath();
            return "IDLE";
        }
        if (EventNavigateTo()) {
            // cancel casting (if any) and start moving
            skillCur = skillNext = -1;
            agent.stoppingDistance = navigateStop;
            agent.destination = navigatePos;
            return "MOVING";
        }
        if (EventSkillRequest()) {
            // if and where we keep moving depends on the skill and the target
            // check self (alive, mana, weapon etc.) and target
            var skill = skills[skillCur];
            targetNext = target; // return to this one after any corrections by CastCheckTarget
            if (CastCheckSelf(skill) && CastCheckTarget(skill)) {
                // check distance between self and target
                if (CastCheckDistance(skill)) {
                    // stop moving, start casting and set the casting end time
                    agent.ResetPath();
                    skill.castTimeEnd = Time.time + skill.castTime;
                    skills[skillCur] = skill;
                    return "CASTING";
                } else {
                    // keep moving towards the target
                    // (use collider point(s) to also work with big entities)
                    agent.stoppingDistance = skill.castRange;
                    agent.destination = target.collider.ClosestPointOnBounds(transform.position);
                    return "MOVING";
                }
            } else {
                // invalid target. stop trying to cast, but keep moving.
                skillCur = skillNext = -1;
                return "IDLE";
            }
        }
        if (EventSkillFinished()) {} // don't care
        if (EventDeathTimeElapsed()) {} // don't care
        if (EventRespawnTimeElapsed()) {} // don't care
        if (EventTargetDied()) {} // don't care
        if (EventTargetDisappeared()) {} // don't care

        return "MOVING"; // nothing interesting happened
    }

    [Server]
    string UpdateServer_CASTING() { 
        // keep looking at the target for server & clients (only Y rotation)
        if (target) LookAtY(target.transform.position);

        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventDied()) {
            // we died.
            OnDeath();
            skillCur = skillNext = -1; // in case we died while trying to cast
            return "DEAD";
        }
        if (EventNavigateTo()) {
            // cancel casting and start moving
            skillCur = skillNext = -1;
            agent.stoppingDistance = navigateStop;
            agent.destination = navigatePos;
            return "MOVING";
        }
        if (EventCancelAction()) {
            // cancel casting
            skillCur = skillNext = -1;
            return "IDLE";
        }
        if (EventTargetDisappeared()) {
            // cancel if we were trying to cast an attacks kill
            if (skills[skillCur].category == "Attack") {
                skillCur = skillNext = -1;
                return "IDLE";
            }
        }
        if (EventTargetDied()) {
            // cancel if we were trying to cast an attack skill
            if (skills[skillCur].category == "Attack") {
                skillCur = skillNext = -1;
                return "IDLE";
            }
        }
        if (EventSkillFinished()) {
            // apply the skill after casting is finished
            // note: we don't check the distance again. it's more fun if players
            //       still cast the skill if the target ran a few steps away
            var skill = skills[skillCur];

            // apply the skill on the target
            CastSkill(skill);

            // casting finished for now. user pressed another skill button?
            if (skillNext != -1) {
                skillCur = skillNext;
                skillNext = -1;
            // skill should be followed with default attack? otherwise clear
            } else skillCur = skill.followupDefaultAttack ? 0 : -1;

            // user tried to target something while casting?
            // (we have to wait until the skill is finished, otherwise people
            //  may start to cast and then switch to a far away target while
            //  casting, etc.)
            if (targetNext != null) {
                target = targetNext;
                targetNext = null;
            }

            // go back to IDLE
            return "IDLE";
        }
        if (EventMoveEnd()) {} // don't care
        if (EventDeathTimeElapsed()) {} // don't care
        if (EventRespawnTimeElapsed()) {} // don't care
        if (EventSkillRequest()) {} // don't care

        return "CASTING"; // nothing interesting happened
    }

    [Server]
    string UpdateServer_DEAD() {        
        // events sorted by priority (e.g. target doesn't matter if we died)
        if (EventRespawnTimeElapsed()) {
            // find team's spawn point and go there; restore health; go to idle
            Show(); // Hide was called before, Show again now.
            var start = FindObjectsOfType<PlayerSpawn>().Where(g => g.team == team).First();
            agent.Warp(start.transform.position); // recommended over transform.position
            Revive();
            return "IDLE";
        }
        if (EventDeathTimeElapsed()) {
            // we were lying around dead for long enough now.
            // hide while respawning, or disappear forever
            Hide();
            return "DEAD";
        }
        if (EventMoveEnd()) {} // don't care
        if (EventSkillFinished()) {} // don't care
        if (EventDied()) {} // don't care
        if (EventCancelAction()) {} // don't care
        if (EventTargetDisappeared()) {} // don't care
        if (EventTargetDied()) {} // don't care
        if (EventNavigateTo()) {} // don't care
        if (EventSkillRequest()) {} // don't care

        return "DEAD"; // nothing interesting happened
    }

    [Server]
    protected override string UpdateServer() {
        if (state == "IDLE")    return UpdateServer_IDLE();
        if (state == "MOVING")  return UpdateServer_MOVING();
        if (state == "CASTING") return UpdateServer_CASTING();
        if (state == "DEAD")    return UpdateServer_DEAD();
        Debug.LogError("invalid state:" + state);
        return "IDLE";
    }

    // finite state machine - client ///////////////////////////////////////////
    [Client]
    protected override void UpdateClient() {
        // pressing/holding space bar makes camera focus on the player
        // (not while typing in chat etc.)
        if (isLocalPlayer) {
            if (Input.GetKey(focusKey) && !UIUtils.AnyInputActive()) {
                // focus on it once, then disable scrolling while holding the
                // button, otherwise camera gets shaky when moving cursor to the
                // edge of the screen
                Camera.main.GetComponent<CameraScrolling>().FocusOn(transform.position);
                Camera.main.GetComponent<CameraScrolling>().enabled = false;
            } else {
                Camera.main.GetComponent<CameraScrolling>().enabled = true;
            }
        }

        if (state == "IDLE" || state == "MOVING") {
            if (isLocalPlayer) {
                // simply accept input
                SelectionHandling();

                // canel action if escape key was pressed, clear skillWanted
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    skillWanted = -1;
                    CmdCancelAction();
                }
            }
        } else if (state == "CASTING") {            
            // keep looking at the target for server & clients (only Y rotation)
            if (target) LookAtY(target.transform.position);

            if (isLocalPlayer) {            
                // simply accept input
                SelectionHandling();

                // canel action if escape key was pressed, clear skillWanted
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    skillWanted = -1;
                    CmdCancelAction();
                }
            }
        } else if (state == "DEAD") {
            
        } else Debug.LogError("invalid state:" + state);
    }
    
    // recover /////////////////////////////////////////////////////////////////
    public override void Recover() {
        // base recovery
        base.Recover();

        // additional buff recovery
        if (enabled && hp > 0) {
            // health percent    
            var buffHpPercent = (from skill in skills
                                 where skill.BuffTimeRemaining() > 0
                                 select skill.buffsHpPercentPerSecond).Sum();
            hp += Convert.ToInt32(buffHpPercent * hpMax);

            // mana percent    
            var buffMpPercent = (from skill in skills
                                 where skill.BuffTimeRemaining() > 0
                                 select skill.buffsMpPercentPerSecond).Sum();
            mp += Convert.ToInt32(buffMpPercent * mpMax);
        }
    }

    // combat //////////////////////////////////////////////////////////////////
    // custom DealDamageAt function that also rewards experience if we killed
    // the monster
    [Server]
    public override HashSet<Entity> DealDamageAt(Entity entity, int n, float aoeRadius=0f) {
        // deal damage with the default function. get all entities that were hit
        // in the AoE radius
        var entities = base.DealDamageAt(entity, n, aoeRadius);
        foreach (var e in entities) {
            // entity still alive, or did we kill it?
            if (e.hp > 0) {
                if (e is Monster)
                    // let's make sure to pull aggro in any case so that archers
                    // are still attacked if they are outside of the aggro range
                    ((Monster)e).OnAggro(this);
            } else {
                // any exp or gold rewards? (depends on type)
                long deathExp = 0;
                int deathGold = 0;

                if (e is Monster) {
                    deathExp = ((Monster)e).rewardExp;
                    deathGold = ((Monster)e).rewardGold;
                } else if (e is Tower) {
                    deathExp = ((Tower)e).rewardExp;
                    deathGold = ((Tower)e).rewardGold;
                } else if (e is Barrack) {
                    deathExp = ((Barrack)e).rewardExp;
                    deathGold = ((Barrack)e).rewardGold;
                }

                // gain experience reward
                exp = BalanceExpReward(deathExp, level, e.level);
                
                // gain gold reward
                gold += deathGold;

                // show gold popup in observers via ClientRpc (if >0)
                // showing them above their head looks best, and we don't have to
                // use a custom shader to draw world space UI in front of the entity
                // note: we send the RPC to ourselves because whatever we killed
                //       might disappear before the rpc reaches it
                if (deathGold > 0) {
                    var bounds = e.GetComponentInChildren<Collider>().bounds;
                    RpcShowGoldPopup(deathGold, new Vector3(bounds.center.x, bounds.max.y, bounds.center.z));
                }
            }
        }
        return entities; // not really needed anywhere
    }

    // experience //////////////////////////////////////////////////////////////
    public float ExpPercent() {
        return (exp != 0 && expMax != 0) ? (float)exp / (float)expMax : 0.0f;
    }
    
    // players gain exp depending on their level. if a player has a lower level
    // than the monster, then he gains more exp (up to 100% more) and if he has
    // a higher level, then he gains less exp (up to 100% less)
    // -> test with monster level 20 and expreward of 100:
    //   BalanceExpReward( 1, 20, 100)); => 200
    //   BalanceExpReward( 9, 20, 100)); => 200
    //   BalanceExpReward(10, 20, 100)); => 200
    //   BalanceExpReward(11, 20, 100)); => 190
    //   BalanceExpReward(12, 20, 100)); => 180
    //   BalanceExpReward(13, 20, 100)); => 170
    //   BalanceExpReward(14, 20, 100)); => 160
    //   BalanceExpReward(15, 20, 100)); => 150
    //   BalanceExpReward(16, 20, 100)); => 140
    //   BalanceExpReward(17, 20, 100)); => 130
    //   BalanceExpReward(18, 20, 100)); => 120
    //   BalanceExpReward(19, 20, 100)); => 110
    //   BalanceExpReward(20, 20, 100)); => 100
    //   BalanceExpReward(21, 20, 100)); =>  90
    //   BalanceExpReward(22, 20, 100)); =>  80
    //   BalanceExpReward(23, 20, 100)); =>  70
    //   BalanceExpReward(24, 20, 100)); =>  60
    //   BalanceExpReward(25, 20, 100)); =>  50
    //   BalanceExpReward(26, 20, 100)); =>  40
    //   BalanceExpReward(27, 20, 100)); =>  30
    //   BalanceExpReward(28, 20, 100)); =>  20
    //   BalanceExpReward(29, 20, 100)); =>  10
    //   BalanceExpReward(30, 20, 100)); =>   0
    //   BalanceExpReward(31, 20, 100)); =>   0
    public static long BalanceExpReward(long reward, int attackerLevel, int victimLevel) {
        var levelDiff = Mathf.Clamp(victimLevel - attackerLevel, -10, 10);
        var multiplier = 1f + levelDiff*0.1f;
        return Convert.ToInt64(reward * multiplier);
    }

    // death ///////////////////////////////////////////////////////////////////
    [Server]
    void OnDeath() {
        // stop any movement and buffs, clear target
        agent.ResetPath();
        StopBuffs();
        target = null;
        
        // lose experience
        var loseExp = Convert.ToInt64(expMax * deathExpLossPercent);
        exp -= loseExp;

        // lose gold
        var loseGold = Convert.ToInt64(gold * deathGoldLossPercent);
        gold -= loseGold;

        // set death and respawn end times. we set both of them now to make sure
        // that everything works fine even if a player isn't updated for a
        // while. so as soon as it's updated again, the death/respawn will
        // happen immediately if current time > end time.
        deathTimeEnd = Time.time + deathTime;
        respawnTimeEnd = deathTimeEnd + respawnTime; // after death time ended

        // send an info chat message
        var msg = new ChatInfoMsg();
        msg.text = "You died and lost " + loseExp + " experience and " + loseGold + " gold.";
        netIdentity.connectionToClient.Send(ChatInfoMsg.MsgId, msg);
    }

    // inventory ///////////////////////////////////////////////////////////////
    public int InventorySlotsFree() {
        return inventory.Where(item => !item.valid).Count();
    }

    [Command]
    public void CmdSwapInventoryInventory(int fromIndex, int toIndex) {
        // note: should never send a command with complex types!
        // validate: make sure that the slots actually exist in the inventory
        // and that they are not equal
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < inventory.Count &&
            0 <= toIndex && toIndex < inventory.Count &&
            fromIndex != toIndex) {
            // swap them
            var temp = inventory[fromIndex];
            inventory[fromIndex] = inventory[toIndex];
            inventory[toIndex] = temp;
        }
    }

    [Command]
    public void CmdInventorySplit(int fromIndex, int toIndex) {
        // note: should never send a command with complex types!
        // validate: make sure that the slots actually exist in the inventory
        // and that they are not equal
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < inventory.Count &&
            0 <= toIndex && toIndex < inventory.Count &&
            fromIndex != toIndex) {
            // slotFrom has to have an entry, slotTo has to be empty
            if (inventory[fromIndex].valid && !inventory[toIndex].valid) {
                // from entry needs at least amount of 2
                if (inventory[fromIndex].amount >= 2) {
                    // split them serversided (has to work for even and odd)
                    var itemFrom = inventory[fromIndex];
                    var itemTo = inventory[fromIndex]; // copy the value
                    //inventory[toIndex] = inventory[fromIndex]; // copy value type
                    itemTo.amount = itemFrom.amount / 2;
                    itemFrom.amount -= itemTo.amount; // works for odd too

                    // put back into the list
                    inventory[fromIndex] = itemFrom;
                    inventory[toIndex] = itemTo;
                }
            }
        }
    }

    [Command]
    public void CmdInventoryMerge(int fromIndex, int toIndex) {
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= fromIndex && fromIndex < inventory.Count &&
            0 <= toIndex && toIndex < inventory.Count &&
            fromIndex != toIndex) {
            // both items have to be valid
            if (inventory[fromIndex].valid && inventory[toIndex].valid) {
                // make sure that items are the same type
                if (inventory[fromIndex].name == inventory[toIndex].name) {
                    // merge from -> to
                    var itemFrom = inventory[fromIndex];
                    var itemTo = inventory[toIndex];
                    var stack = Mathf.Min(itemFrom.amount + itemTo.amount, itemTo.maxStack);
                    var put = stack - itemFrom.amount;
                    itemFrom.amount = itemTo.amount - put;
                    itemTo.amount = stack;
                    // 'from' empty now? then clear it
                    if (itemFrom.amount == 0) itemFrom.valid = false;
                    // put back into the list
                    inventory[fromIndex] = itemFrom;
                    inventory[toIndex] = itemTo;
                }
            }
        }
    }

    [Command]
    public void CmdUseInventoryItem(int index) {
        // validate
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= index && index < inventory.Count && inventory[index].valid) {
            // what we have to do depends on the category
            //print("use item:" + index);
            var item = inventory[index];
            if (item.category.StartsWith("Potion")) {
                // use
                hp += item.usageHp;
                mp += item.usageMp;
                
                // decrease amount or destroy
                if (item.usageDestroy) {
                    --item.amount;
                    if (item.amount == 0) item.valid = false;
                    inventory[index] = item; // put new values in there
                }
            }
        }
    }

    // skills //////////////////////////////////////////////////////////////////
    public override bool CanAttackType(System.Type t) {
        return t == typeof(Monster) || t == typeof(Player) || t == typeof(Tower) || t == typeof(Barrack) || t == typeof(Base);
    }

    [Command]
    public void CmdUseSkill(int skillIndex) {
        // validate
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= skillIndex && skillIndex < skills.Count) {
            // can the skill be casted?
            if (skills[skillIndex].learned && skills[skillIndex].IsReady()) {
                // add as current or next skill, unless casting same one already
                // (some players might hammer the key multiple times, which
                //  doesn't mean that they want to cast it afterwards again)
                // => also: always set skillCur when moving or idle or whatever
                //  so that the last skill that the player tried to cast while
                //  moving is the first skill that will be casted when attacking
                //  the enemy.
                if (skillCur == -1 || state != "CASTING")
                    skillCur = skillIndex;
                else if (skillCur != skillIndex)
                    skillNext = skillIndex;
            }
        }
    }

    public int SkillpointsSpendable() {
        // calculate the amount of skill points that can still be spent
        // -> one point per level
        // -> we don't need to store the points in an extra variable, we can
        //    simply decrease the spent points from the current skills
        // -> and +1 because players should still be able to assign one point in
        //    level 1, even though they did learn Normal Attack automatically
        var spent = (from skill in skills
                     where skill.learned
                     select 1).Sum(); // TODO skill.level
        return level - spent + 1;
    }

    [Command]
    public void CmdLearnSkill(int skillIndex) {
        // validate
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= skillIndex && skillIndex < skills.Count) {
            var skill = skills[skillIndex];

            // not learned already? enough skill exp, required level?
            // note: status effects aren't learnable
            if (!skill.category.StartsWith("Status") &&
                !skill.learned &&
                level >= skill.requiredLevel &&
                SkillpointsSpendable() > 0) {
                // learn skill
                skill.learned = true;
                skills[skillIndex] = skill;
            }
        }
    }

    [Command]
    public void CmdUpgradeSkill(int skillIndex) {
        // validate
        if ((state == "IDLE" || state == "MOVING" || state == "CASTING") &&
            0 <= skillIndex && skillIndex < skills.Count) {
            var skill = skills[skillIndex];

            // already learned and required level for upgrade?
            // and can be upgraded?
            // note: status effects aren't upgradeable
            if (!skill.category.StartsWith("Status") &&
                skill.learned &&
                skill.level < skill.maxLevel &&
                level >= skill.upgradeRequiredLevel) {
                // upgrade
                ++skill.level;
                skills[skillIndex] = skill;
            }
        }
    }
    
    // npc trading /////////////////////////////////////////////////////////////
    [Command]
    public void CmdNpcBuyItem(int index, int amount) {
        // validate: close enough, npc alive and valid index?
        // use collider point(s) to also work with big entities
        if (state == "IDLE" &&
            target != null &&
            target.hp > 0 &&
            target is Npc &&
            target.team == team &&
            Utils.ClosestDistance(collider, target.collider) <= talkRange &&
            0 <= index && index < ((Npc)target).saleItems.Length)
        {
            var npc = (Npc)target;

            // valid amount?
            if (1 <= amount && amount <= npc.saleItems[index].maxStack) {
                var price = npc.saleItems[index].buyPrice * amount;

                // enough gold?
                if (gold >= price) {
                    // find free inventory slot
                    var freeIdx = inventory.FindIndex(item => !item.valid);
                    if (freeIdx != -1) {
                        // buy it
                        gold -= price;
                        var item = new Item(npc.saleItems[index], amount);
                        inventory[freeIdx] = item;
                    }
                }
            }
        }
    }

    [Command]
    public void CmdNpcSellItem(int index, int amount) {
        // validate: close enough, npc alive and valid index and valid item?
        // use collider point(s) to also work with big entities
        if (state == "IDLE" &&
            target != null && 
            target.hp > 0 &&
            target is Npc &&
            target.team == team &&
            Utils.ClosestDistance(collider, target.collider) <= talkRange &&
            0 <= index && index < inventory.Count &&
            inventory[index].valid)
        {
            var item = inventory[index];

            // valid amount?
            if (1 <= amount && amount <= item.amount) {
                // sell the amount
                var price = item.sellPrice * amount;
                gold += price;
                item.amount -= amount;
                if (item.amount == 0) item.valid = false;
                inventory[index] = item;
            }
        }
    }

    // selection handling //////////////////////////////////////////////////////
    public void SetIndicatorViaParent(Transform parent) {
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.SetParent(parent, true);
        indicator.transform.position = parent.position + Vector3.up * 0.01f;
        indicator.transform.up = Vector3.up;
    }

    public void SetIndicatorViaPosition(Vector3 pos, Vector3 normal) {
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = null;
        indicator.transform.position = pos + Vector3.up * 0.01f;
        indicator.transform.up = normal; // adjust to terrain normal
    }

    [Command]
    void CmdSetTarget(NetworkIdentity ni) {
        // validate
        if (ni != null) {
            // can directly change it, or change it after casting?
            if (state == "IDLE" || state == "MOVING")
                target = ni.GetComponent<Entity>();
            else if (state == "CASTING")
                targetNext = ni.GetComponent<Entity>();
        }
    }

    [Client]
    void SelectionHandling() {
        bool left = Input.GetMouseButtonDown(0);
        bool right = Input.GetMouseButtonDown(1);

        if ((left || right) &&
            !Utils.IsCursorOverUserInterface()) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                // left click
                if (left) {
                    // valid target?
                    var entity = hit.transform.GetComponent<Entity>();
                    if (entity) {
                        // set indicator
                        SetIndicatorViaParent(hit.transform);

                        // trying to cast a skill?
                        if (0 <= skillWanted && skillWanted < skills.Count) {
                            // what is it?
                            if (entity is Monster || entity is Tower || entity is Barrack || entity is Base) {
                                // dead or alive?
                                if (entity.hp > 0) {
                                    // cast the wanted skill (if ready)
                                    if (skills[skillWanted].IsReady()) {
                                        CmdSetTarget(entity.netIdentity);
                                        CmdUseSkill(skillWanted);
                                    // otherwise walk there if still on cooldown etc
                                    // use collider point(s) to also work with big entities
                                    } else {
                                        CmdNavigateTo(entity.collider.ClosestPointOnBounds(transform.position),
                                                      skills[skillWanted].castRange);
                                    }
                                } else {
                                    // walk there
                                    // use collider point(s) to also work with big entities
                                    CmdNavigateTo(entity.collider.ClosestPointOnBounds(transform.position), lootRange);
                                }
                            } else if (entity is Player) {
                                // cast the wanted skill, reset it
                                CmdSetTarget(entity.netIdentity);
                                CmdUseSkill(skillWanted);
                            }
                        } else {
                            // talk to / move to npc
                            if (entity is Npc && entity.team == team) {
                                // close enough to talk?
                                // use collider point(s) to also work with big entities
                                if (Utils.ClosestDistance(collider, entity.collider) <= talkRange) {
                                    CmdSetTarget(entity.netIdentity);
                                    FindObjectOfType<UINpcTrading>().Show();
                                // otherwise walk there
                                // use collider point(s) to also work with big entities
                                } else {
                                    CmdNavigateTo(entity.collider.ClosestPointOnBounds(transform.position), talkRange);
                                }
                            }
                        }
                    }
                // right click
                } else if (right) {
                    // valid target?
                    var entity = hit.transform.GetComponent<Entity>();
                    if (entity) {
                        // set indicator
                        SetIndicatorViaParent(hit.transform);

                        // is not self?
                        if (entity != this) {
                            // what is it?
                            if (entity is Monster || entity is Tower || entity is Barrack || entity is Base) {
                                // dead or alive?
                                if (entity.hp > 0) {
                                    // cast the first skill (if any, and if ready)
                                    if (skills.Count > 0 && skills[0].IsReady()) {
                                        CmdSetTarget(entity.netIdentity);
                                        CmdUseSkill(0);
                                    // otherwise walk there if still on cooldown etc
                                    // use collider point(s) to also work with big entities
                                    } else {
                                        CmdNavigateTo(entity.collider.ClosestPointOnBounds(transform.position),
                                                      skills.Count > 0 ? skills[0].castRange : 0f);
                                    }
                                } else {
                                    // walk there
                                    // use collider point(s) to also work with big entities
                                    CmdNavigateTo(entity.collider.ClosestPointOnBounds(transform.position), lootRange);
                                }
                            } else if (entity is Player) {
                                // cast the first skill (if any)
                                if (skills.Count > 0) {
                                    CmdSetTarget(entity.netIdentity);
                                    CmdUseSkill(0);
                                }
                            }
                        }
                    // otherwise it's a movement target
                    } else {
                        // set indicator and navigate
                        SetIndicatorViaPosition(hit.point, hit.normal);
                        CmdNavigateTo(hit.point, 0f);
                    }
                }

                // reset wanted skill in any case (feels best)
                skillWanted = -1;
            }
        }
    }
}
