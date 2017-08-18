using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour {
    [SerializeField] Text damage;
    [SerializeField] Text defense;
    [SerializeField] Text speed;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // show all stats like base(+bonus)
        damage.text = player.baseDamage + " ( + " + (player.damage-player.baseDamage) + ")";
        defense.text = player.baseDefense + " ( + " + (player.defense-player.baseDefense) + ")";
        speed.text = player.speed + " ( + 0)";
    }
}
