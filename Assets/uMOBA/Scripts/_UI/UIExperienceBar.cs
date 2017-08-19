using UnityEngine;
using UnityEngine.UI;
using Utils = uMoba.Utils;

public class UIExperienceBar : MonoBehaviour {
    [SerializeField] Slider bar;
    [SerializeField] Text status;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        bar.value = player.ExpPercent();
        status.text = "Lvl. " + player.level;
    }
}
