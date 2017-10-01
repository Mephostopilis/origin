// The Skill struct only contains the dynamic skill properties and a name, so
// that the static properties can be read from the scriptable object. The
// benefits are low bandwidth and easy editing in the Inspector.
//
// Skills have to be structs in order to work with SyncLists.
//
// We implemented the cooldowns in a non-traditional way. Instead of counting
// and increasing the elapsed time since the last cast, we simply set the
// 'end' Time variable to Time.time + cooldown after casting each time. This
// way we don't need an extra Update method that increases the elapsed time for
// each skill all the time.
//
//
// Note: the file can't be named "Skill.cs" because of the following UNET bug:
// http://forum.unity3d.com/threads/bug-syncliststruct-only-works-with-some-file-names.384582/
using UnityEngine;
using UnityEngine.Networking;
using Bacon.GL.Util;

[System.Serializable]
public struct Skill {
    // name used to reference the database entry (cant save template directly
    // because synclist only support simple types)
    public string name;

    // dynamic stats (cooldowns etc.)
    public bool learned;
    public int level;
    public float castTimeEnd; // server time
    public float cooldownEnd; // server time
    public float buffTimeEnd; // server time

    // constructors
    public Skill(SkillTemplate template) {
        name = template.name;

        // learned only if learned by default
        learned = template.learnDefault;
        level = 1;

        // ready immediately
        castTimeEnd = cooldownEnd = buffTimeEnd = Time.time;
    }

    // does the template still exist?
    public bool TemplateExists() {
        return SkillTemplate.dict.ContainsKey(name);
    }

    // database quest property access etc.
    public string category {
        get { return SkillTemplate.dict[name].category; }
    }
    public int damage {
        get { return SkillTemplate.dict[name].levels[level-1].damage; }
    }
    public float castTime {
        get { return SkillTemplate.dict[name].levels[level-1].castTime; }
    }
    public float cooldown {
        get { return SkillTemplate.dict[name].levels[level-1].cooldown; }
    }
    public float castRange {
        get { return SkillTemplate.dict[name].levels[level-1].castRange; }
    }
    public float aoeRadius {
        get { return SkillTemplate.dict[name].levels[level-1].aoeRadius; }
    }
    public int manaCosts {
        get { return SkillTemplate.dict[name].levels[level-1].manaCosts; }
    }
    public int healsHp {
        get { return SkillTemplate.dict[name].levels[level-1].healsHp; }
    }
    public int healsMp {
        get { return SkillTemplate.dict[name].levels[level-1].healsMp; }
    }
    public float buffTime {
        get { return SkillTemplate.dict[name].levels[level-1].buffTime; }
    }
    public int buffsHpMax {
        get { return SkillTemplate.dict[name].levels[level-1].buffsHpMax; }
    }
    public int buffsMpMax {
        get { return SkillTemplate.dict[name].levels[level-1].buffsMpMax; }
    }
    public int buffsDamage {
        get { return SkillTemplate.dict[name].levels[level-1].buffsDamage; }
    }
    public int buffsDefense {
        get { return SkillTemplate.dict[name].levels[level-1].buffsDefense; }
    }
    public float buffsHpPercentPerSecond {
        get { return SkillTemplate.dict[name].levels[level-1].buffsHpPercentPerSecond; }
    }
    public float buffsMpPercentPerSecond {
        get { return SkillTemplate.dict[name].levels[level-1].buffsMpPercentPerSecond; }
    }
    public bool followupDefaultAttack {
        get { return SkillTemplate.dict[name].followupDefaultAttack; }
    }
    public Sprite image {
        get { return SkillTemplate.dict[name].image; }
    }
    public Projectile projectile {
        get { return SkillTemplate.dict[name].levels[level-1].projectile; }
    }
    public int requiredLevel {
        get { return SkillTemplate.dict[name].levels[level-1].requiredLevel; }
    }
    public int maxLevel {
        get { return SkillTemplate.dict[name].levels.Length; }
    }
    public int upgradeRequiredLevel {
        get { return (level < maxLevel) ? SkillTemplate.dict[name].levels[level].requiredLevel : 0; }
    }

    // fill in all variables into the tooltip
    // this saves us lots of ugly string concatenation code. we can't do it in
    // SkillTemplate because some variables can only be replaced here, hence we
    // would end up with some variables not replaced in the string when calling
    // Tooltip() from the template.
    // -> note: each tooltip can have any variables, or none if needed
    // -> example usage:
    /*
    <b>{NAME} Lvl {LEVEL}</b>
    Description here...

    Damage: {DAMAGE}
    Cast Time: {CASTTIME}
    Cooldown: {COOLDOWN}
    Cast Range: {CASTRANGE}
    AoE Radius: {AOERADIUS}
    Heals Health: {HEALSHP}
    Heals Mana: {HEALSMP}
    Buff Time: {BUFFTIME}
    Buffs max Health: {BUFFSHPMAX}
    Buffs max Mana: {BUFFSMPMAX}
    Buffs damage: {BUFFSDAMAGE}
    Buffs defense: {BUFFSDEFENSE}
    Buffs Health % per Second: {BUFFSHPPERCENTPERSECOND}
    Buffs Mana % per Second: {BUFFSMPPERCENTPERSECOND}
    Mana Costs: {MANACOSTS}
    */
    public string Tooltip() {
        var tip = SkillTemplate.dict[name].tooltip;
        tip = tip.Replace("{NAME}", name);
        tip = tip.Replace("{CATEGORY}", category);
        tip = tip.Replace("{LEVEL}", level.ToString());
        tip = tip.Replace("{DAMAGE}", damage.ToString());
        tip = tip.Replace("{CASTTIME}", Utils.PrettyTime(castTime));
        tip = tip.Replace("{COOLDOWN}", Utils.PrettyTime(cooldown));
        tip = tip.Replace("{CASTRANGE}", castRange.ToString());
        tip = tip.Replace("{AOERADIUS}", aoeRadius.ToString());
        tip = tip.Replace("{HEALSHP}", healsHp.ToString());
        tip = tip.Replace("{HEALSMP}", healsMp.ToString());
        tip = tip.Replace("{BUFFTIME}", Utils.PrettyTime(buffTime));
        tip = tip.Replace("{BUFFSHPMAX}", buffsHpMax.ToString());
        tip = tip.Replace("{BUFFSMPMAX}", buffsMpMax.ToString());
        tip = tip.Replace("{BUFFSDAMAGE}", buffsDamage.ToString());
        tip = tip.Replace("{BUFFSDEFENSE}", buffsDefense.ToString());
        tip = tip.Replace("{BUFFSHPPERCENTPERSECOND}", buffsHpPercentPerSecond.ToString());
        tip = tip.Replace("{BUFFSMPPERCENTPERSECOND}", buffsMpPercentPerSecond.ToString());
        tip = tip.Replace("{MANACOSTS}", manaCosts.ToString());

        // not learned yet? then show requirements
        if (!learned)
            tip += "\n<b><i>Required Level: " + requiredLevel + "</i></b>\n";
        // upgrade? (don't show if the skill wasn't even learned yet)
        else if (learned && level < maxLevel)
            tip += "\n<b><i>Upgrade Required Level: " + upgradeRequiredLevel + "</i></b>\n";
        
        return tip;
    }

    public float CastTimeRemaining() {
        // we use the NetworkTime offset so that client cooldowns match server
        var serverTime = Time.time + NetworkTime.offset;

        // how much time remaining until the casttime ends?
        return serverTime >= castTimeEnd ? 0f : castTimeEnd - serverTime;
    }

    public bool IsCasting() {
        // we are casting a skill if the casttime remaining is > 0
        return CastTimeRemaining() > 0f;
    }

    public float CooldownRemaining() {
        // we use the NetworkTime offset so that client cooldowns match server
        var serverTime = Time.time + NetworkTime.offset;

        // how much time remaining until the cooldown ends?
        return serverTime >= cooldownEnd ? 0f : cooldownEnd - serverTime;
    }

    public float BuffTimeRemaining() {
        // we use the NetworkTime offset so that client cooldowns match server
        var serverTime = Time.time + NetworkTime.offset;

        // how much time remaining until the buff ends?
        return serverTime >= buffTimeEnd ? 0f : buffTimeEnd - serverTime;        
    }

    public bool IsReady() {
        return CooldownRemaining() == 0f;
    }    
}

public class SyncListSkill : SyncListStruct<Skill> { }
