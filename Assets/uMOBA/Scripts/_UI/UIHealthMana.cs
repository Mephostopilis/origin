using UnityEngine;
using UnityEngine.UI;
using Bacon.GL.Util;

public class UIHealthMana : MonoBehaviour {
    [SerializeField] Slider hpBar;
    [SerializeField] Text hpStatus;
    [SerializeField] Slider mpBar;
    [SerializeField] Text mpStatus;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        hpBar.value = player.HpPercent();
        hpStatus.text = player.hp + " / " + player.hpMax;

        mpBar.value = player.MpPercent();
        mpStatus.text = player.mp + " / " + player.mpMax;
    }
}
