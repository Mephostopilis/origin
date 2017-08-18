// Saves the item info in a ScriptableObject that can be used ingame by
// referencing it from a MonoBehaviour. It only stores an item's static data.
//
// We also add each one to a dictionary automatically, so that all of them can
// be found by name without having to put them all in a database. Note that we
// have to put them all into the Resources folder and use Resources.LoadAll to
// load them. This is important because some items may not be referenced by any
// entity ingame (e.g. when a special event item isn't dropped anymore after the
// event). But all items should still be loadable from the database, even if
// they are not referenced by anyone.
//
// An Item can be created by right clicking the Resources folder and selecting
// Create -> uMOBA Item. Existing items can be found in the Resources folder.
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName="New Item", menuName="uMOBA Item", order=999)]
public class ItemTemplate : ScriptableObject {
    // base stats
    public string category;
    public int maxStack;
    public int buyPrice;
    public int sellPrice;
    [TextArea(1, 30)] public string tooltip;
    public Sprite image;

    // one time usage boosts
    public bool usageDestroy;
    public int usageHp;
    public int usageMp;
    
    // equipment boosts (quick and dirty, to avoid itemequipment class etc.)
    public int equipHpBonus;
    public int equipMpBonus;
    public int equipDamageBonus;
    public int equipDefenseBonus;

    // caching /////////////////////////////////////////////////////////////////
    // we can only use Resources.Load in the main thread. we can't use it when
    // declaring static variables. so we have to use it as soon as 'dict' is
    // accessed for the first time from the main thread.
    static Dictionary<string, ItemTemplate> cache = null;
    public static Dictionary<string, ItemTemplate> dict {
        get {
            // load if not loaded yet
            if (cache == null)
                cache = Resources.LoadAll<ItemTemplate>("").ToDictionary(
                    item => item.name, item => item
                );
            return cache;
        }
    }

    // inspector validation ////////////////////////////////////////////////////
    void OnValidate() {
        // make sure that the sell price <= buy price to avoid exploitation
        // (people should never buy an item for 1 gold and sell it for 2 gold)
        sellPrice = Mathf.Min(sellPrice, buyPrice);
    }
}
