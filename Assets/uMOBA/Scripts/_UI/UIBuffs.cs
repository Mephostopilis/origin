using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIBuffs : MonoBehaviour {
    [SerializeField] GameObject slotPrefab;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // instantiate/destroy enough slots
        var buffs = player.skills.Where(s => s.BuffTimeRemaining() > 0).ToList();
        UIUtils.BalancePrefabs(slotPrefab, buffs.Count, transform);

        // refresh all
        for (int i = 0; i < buffs.Count; ++i) {
            var entry = transform.GetChild(i).GetChild(0); // slot entry
            entry.name = i.ToString(); // for drag and drop

            // image
            entry.GetComponent<Image>().color = Color.white;
            entry.GetComponent<Image>().sprite = buffs[i].image;
            entry.GetComponent<UIShowToolTip>().text = buffs[i].Tooltip();

            // time bar
            var slider = entry.GetComponentInChildren<Slider>();
            slider.maxValue = buffs[i].buffTime;
            slider.value = buffs[i].BuffTimeRemaining();
        }
    }
}
