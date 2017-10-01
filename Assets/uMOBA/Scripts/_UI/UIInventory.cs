using UnityEngine;
using UnityEngine.UI;
using Bacon.GL.Util;

public class UIInventory : MonoBehaviour {
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform content;
    [SerializeField] Text goldText;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab, player.inventory.Count, content);

        // refresh all
        for (int i = 0; i < player.inventory.Count; ++i) {
            var entry = content.GetChild(i).GetChild(0); // slot entry
            entry.name = i.ToString(); // for drag and drop
            var item = player.inventory[i];

            // overlay hotkey (without 'Alpha' etc.)
            var pretty = player.inventoryHotkeys[i].ToString().Replace("Alpha", "");
            entry.GetChild(1).GetComponentInChildren<Text>().text = pretty;

            if (item.valid) {
                // click event
                int icopy = i; // needed for lambdas, otherwise i is Count
                entry.GetComponent<Button>().onClick.SetListener(() => {
                    player.CmdUseInventoryItem(icopy);
                });

                // hotkey pressed and not typing in any input right now?
                if (Input.GetKeyDown(player.inventoryHotkeys[i]) && !UIUtils.AnyInputActive())
                    player.CmdUseInventoryItem(i);
                
                // set state
                entry.GetComponent<UIShowToolTip>().enabled = true;
                entry.GetComponent<UIDragAndDropable>().dragable = true;
                // note: entries should be dropable at all times

                // image
                entry.GetComponent<Image>().color = Color.white;
                entry.GetComponent<Image>().sprite = item.image;
                entry.GetComponent<UIShowToolTip>().text = item.Tooltip();

                // amount overlay
                entry.GetChild(0).gameObject.SetActive(item.amount > 1);
                if (item.amount > 1) entry.GetComponentInChildren<Text>().text = item.amount.ToString();
            } else {
                // remove listeners
                entry.GetComponent<Button>().onClick.RemoveAllListeners();

                // set state
                entry.GetComponent<UIShowToolTip>().enabled = false;
                entry.GetComponent<UIDragAndDropable>().dragable = false;

                // image
                entry.GetComponent<Image>().color = Color.clear;
                entry.GetComponent<Image>().sprite = null;

                // amount overlay
                entry.GetChild(0).gameObject.SetActive(false);
            }
        }

        // gold
        goldText.text = player.gold.ToString();
    }
}
