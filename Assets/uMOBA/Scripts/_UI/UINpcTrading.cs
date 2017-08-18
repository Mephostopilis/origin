using UnityEngine;
using UnityEngine.UI;

public class UINpcTrading : MonoBehaviour {
    [SerializeField] GameObject panel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform content;
    [SerializeField] UIDragAndDropable buySlot;
    [SerializeField] InputField buyAmount;
    [SerializeField] Text buyCosts;
    [SerializeField] Button buyButton;
    [SerializeField] UIDragAndDropable sellSlot;
    public InputField sellAmount;
    [SerializeField] Text sellCosts;
    [SerializeField] Button sellButton;
    int buyIndex = -1;
    [HideInInspector] public int sellIndex = -1;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // use collider point(s) to also work with big entities
        if (panel.activeSelf &&
            player.target != null && player.target is Npc &&
            Utils.ClosestDistance(player.collider, player.target.collider) <= player.talkRange) {
            var npc = (Npc)player.target;

            // items for sale            
            UIUtils.BalancePrefabs(slotPrefab, npc.saleItems.Length, content);
            for (int i = 0; i < npc.saleItems.Length; ++i) {
                var entry = content.GetChild(i).GetChild(0); // slot entry
                entry.name = i.ToString();
                var item = npc.saleItems[i];

                // set the click event
                int icopy = i;
                entry.GetComponent<Button>().onClick.SetListener(() => {
                    buyIndex = icopy;
                });
                
                // image
                entry.GetComponent<Image>().color = Color.white;
                entry.GetComponent<Image>().sprite = item.image;

                // tooltip
                entry.GetComponent<UIShowToolTip>().enabled = true;
                entry.GetComponent<UIShowToolTip>().text = new Item(item).Tooltip();
            }

            // buy
            if (buyIndex != -1 && buyIndex < npc.saleItems.Length) {
                var item = npc.saleItems[buyIndex];

                // make valid amount
                int amount = buyAmount.text.ToInt();
                amount = Mathf.Clamp(amount, 1, item.maxStack);
                buyAmount.text = amount.ToString();

                // image
                buySlot.GetComponent<Image>().color = Color.white;
                buySlot.GetComponent<Image>().sprite = item.image;

                // tooltip
                buySlot.GetComponent<UIShowToolTip>().enabled = true;
                buySlot.GetComponent<UIShowToolTip>().text = new Item(item).Tooltip();

                // price
                long price = amount * item.buyPrice;
                buyCosts.text = price.ToString();

                // button
                buyButton.interactable = amount > 0 && price <= player.gold;
                buyButton.onClick.SetListener(() => {
                    player.CmdNpcBuyItem(buyIndex, amount);
                    buyIndex = -1;
                    buyAmount.text = "1";
                });
            } else {
                // image
                buySlot.GetComponent<Image>().color = Color.clear;
                buySlot.GetComponent<Image>().sprite = null;

                // tooltip
                buySlot.GetComponent<UIShowToolTip>().enabled = false;

                // price
                buyCosts.text = "0";

                // button
                buyButton.interactable = false;
            }

            // sell
            if (sellIndex != -1 && sellIndex < player.inventory.Count &&
                player.inventory[sellIndex].valid) {
                var item = player.inventory[sellIndex];

                // make valid amount
                int amount = sellAmount.text.ToInt();
                amount = Mathf.Clamp(amount, 1, item.amount);
                sellAmount.text = amount.ToString();

                // image
                sellSlot.GetComponent<Image>().color = Color.white;
                sellSlot.GetComponent<Image>().sprite = item.image;

                // tooltip
                sellSlot.GetComponent<UIShowToolTip>().enabled = true;
                sellSlot.GetComponent<UIShowToolTip>().text = item.Tooltip();

                // price
                long price = amount * item.sellPrice;
                sellCosts.text = price.ToString();

                // button
                sellButton.interactable = amount > 0;
                sellButton.onClick.SetListener(() => {
                    player.CmdNpcSellItem(sellIndex, amount);
                    sellIndex = -1;
                    sellAmount.text = "1";
                });
            } else {
                // image
                sellSlot.GetComponent<Image>().color = Color.clear;
                sellSlot.GetComponent<Image>().sprite = null;

                // tooltip
                sellSlot.GetComponent<UIShowToolTip>().enabled = true;

                // price
                sellCosts.text = "0";

                // button
                sellButton.interactable = false;
            }
        } else panel.SetActive(false); // hide
    }

    public void Show() { panel.SetActive(true); }
}
