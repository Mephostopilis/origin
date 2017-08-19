using UnityEngine;
using System.Collections;
using Utils = uMoba.Utils;

public class UISkillTargetIndicator : MonoBehaviour {
    public GameObject indicator;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        indicator.SetActive(player.skillWanted != -1);
        indicator.transform.position = Input.mousePosition;
    }
}
