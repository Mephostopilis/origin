using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Bacon.GL.Util;

public class UIWinLose : MonoBehaviour {
    [SerializeField] GameObject panel;
    [SerializeField] Text textWinner;
    [SerializeField] Button buttonQuit;
    
    [SerializeField] Base baseGood;
    [SerializeField] Base baseEvil;

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // is there a base with 0 health?
        if (baseGood.hp == 0) {
            panel.SetActive(true);
            textWinner.text = baseEvil.team.ToString();
        } else if (baseEvil.hp == 0) {
            panel.SetActive(true);
            textWinner.text = baseGood.team.ToString();
        } else panel.SetActive(false); // hide

        // quit button in any case
        buttonQuit.onClick.SetListener(() => {
            NetworkManager.singleton.StopClient();
            Application.Quit();
        });
    }
}
