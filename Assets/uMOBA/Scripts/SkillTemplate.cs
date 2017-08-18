// Saves the skill info in a ScriptableObject that can be used ingame by
// referencing it from a MonoBehaviour. It only stores an skill's static data.
//
// We also add each one to a dictionary automatically, so that all of them can
// be found by name without having to put them all in a database. Note that we
// have to put them all into the Resources folder and use Resources.LoadAll to
// load them. This is important because some skills may not be referenced by any
// entity ingame (e.g. after a special event). But all skills should still be
// loadable from the database, even if they are not referenced by anyone.
//
// Skills can have different stats for each skill level. This is what the
// 'levels' list is for. If you only need one level, then only add one entry to
// it in the Inspector.
//
// A Skill can be created by right clicking the Resources folder and selecting
// Create -> uMOBA Skill. Existing skills can be found in the Resources folder.
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName="New Skill", menuName="uMOBA Skill", order=999)]
public class SkillTemplate : ScriptableObject {
    // general stats
    // we can use the category to decide what to do on use. example categories:
    // Attack, Stun, Buff, Heal, ...
    public string category;
    public bool followupDefaultAttack;
    [TextArea(1, 30)] public string tooltip;
    public Sprite image;

    // skill levels
    [System.Serializable]
    public struct SkillLevel {
        // level dependent stats
        public int damage;
        public float castTime;
        public float cooldown;
        public float castRange;
        public float aoeRadius;
        public int manaCosts;
        public int healsHp;
        public int healsMp;
        public float buffTime;
        public int buffsHpMax;
        public int buffsMpMax;
        public int buffsDamage;
        public int buffsDefense;
        public float buffsHpPercentPerSecond; // can be negative too
        public float buffsMpPercentPerSecond; // can be negative too
        public Projectile projectile; // Arrows, Bullets, Fireballs, ...

        // learn requirements
        public int requiredLevel; // level needed to learn it
    }
    public SkillLevel[] levels = new SkillLevel[]{new SkillLevel()}; // default
    public bool learnDefault; // normal attack etc.
    
    // caching /////////////////////////////////////////////////////////////////
    // we can only use Resources.Load in the main thread. we can't use it when
    // declaring static variables. so we have to use it as soon as 'dict' is
    // accessed for the first time from the main thread.
    static Dictionary<string, SkillTemplate> cache = null;
    public static Dictionary<string, SkillTemplate> dict {
        get {
            // load if not loaded yet
            if (cache == null)
                cache = Resources.LoadAll<SkillTemplate>("").ToDictionary(
                    item => item.name, item => item
                );
            return cache;
        }
    }
}
