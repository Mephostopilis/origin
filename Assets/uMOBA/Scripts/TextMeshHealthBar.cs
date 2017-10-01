// HealthBar via TextMesh of "_". This is a workaround for the following Unity
// bug: https://issuetracker.unity3d.com/issues/destroy-gameobject-time-causes-unity-to-crash-in-sharedgfxbuffer-unshare-sharedgfxbuffer-star
using UnityEngine;
using Bacon.GL.Util;

public class TextMeshHealthBar : MonoBehaviour {
    [SerializeField] Color teamColor = Color.green;
    [SerializeField] Color enemyColor = Color.red;
    [SerializeField] Color backgroundColor = Color.black;

    [SerializeField] Entity self; // own entity component, probably in parents

    string GenerateHealthString(int n, Color color) {
        return "<color=#" + color.ToString() + ">" +
               new string('_', n) +
               "</color>" +
               "<color=#" + backgroundColor.ToString() + ">" +
               new string('_', 10-n) +
               "</color>";
    }

    void Update() {
        // find local player
        var local = Utils.ClientLocalPlayer();
        if (!local) return;
        
        // set color based on same team or not
        var color = (local.team == self.team ? teamColor : enemyColor);

        // draw health as _ _ _
        // -> one _ per 10% in health color, the rest in black as background
        // -> "" while dead, looks best
        if (self.hp > 0) {
            var n = Mathf.RoundToInt(self.HpPercent() * 10);
            GetComponent<TextMesh>().text = GenerateHealthString(n, color);
        } else GetComponent<TextMesh>().text = "";
    }
}
