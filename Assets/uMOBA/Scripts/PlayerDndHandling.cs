// Takes care of Drag and Drop events for the player.
// Works with UI and OnGUI. Simply do:
//   FindObjectOfType<PlayerDndHandling>().OnDragAndDrop(...);
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDndHandling : MonoBehaviour {
    // cache
    Player player;

    // remove self if not local player
    void Start() {
        player = GetComponent<Player>();
        if (!player.isLocalPlayer) Destroy(this);
    }

    // message sent from UI
    public void OnDragAndDrop(string fromType, int from, string toType, int to) {
        // call OnDnd_From_To dynamically
        print("OnDragAndDrop from: " + fromType + " " + from + " to: " + toType + " " + to);
        SendMessage("OnDnd_" + fromType + "_" + toType, new int[]{from, to},
                    SendMessageOptions.DontRequireReceiver);
    }

    public void OnDragAndClear(string type, int from) {
        // nothing to clear yet
    }

    ////////////////////////////////////////////////////////////////////////////
    void OnDnd_InventorySlot_InventorySlot(int[] slotIndices) {
        // slotIndices[0] = slotFrom; slotIndices[1] = slotTo

        // merge? (just check the name, rest is done server sided)
        if (player.inventory[slotIndices[0]].valid && player.inventory[slotIndices[1]].valid &&
            player.inventory[slotIndices[0]].name == player.inventory[slotIndices[1]].name) {
            player.CmdInventoryMerge(slotIndices[0], slotIndices[1]);
        // split?
        } else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            player.CmdInventorySplit(slotIndices[0], slotIndices[1]);
        // swap?
        } else {
            player.CmdSwapInventoryInventory(slotIndices[0], slotIndices[1]);
        }
    }

    void OnDnd_InventorySlot_NpcSellSlot(int[] slotIndices) {
        // slotIndices[0] = slotFrom; slotIndices[1] = slotTo
        FindObjectOfType<UINpcTrading>().sellIndex = slotIndices[0];
        FindObjectOfType<UINpcTrading>().sellAmount.text = player.inventory[slotIndices[0]].amount.ToString();
    }
}
