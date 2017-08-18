using UnityEngine;
using UnityEngine.UI;

public class UISkills : MonoBehaviour {
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform content;

    // helper function when client clicks on a skill or presses the hotkey
    void OnSkillClicked(Player player, int skillIndex) {
        // learned and ready?
        if (player.skills[skillIndex].learned &&
            player.skills[skillIndex].IsReady()) {
            // set skill wanted so that the skill target indicator starts to show
            // (unless for buffs, they are always casted on self)
            if (player.skills[skillIndex].category == "Buff")
                player.CmdUseSkill(skillIndex);            
            else
                player.skillWanted = skillIndex;
        }
    }

    void Update() {
        var player = Utils.ClientLocalPlayer();
        if (!player) return;

        // instantiate/destroy enough slots (except normal attack)
        UIUtils.BalancePrefabs(slotPrefab, player.skills.Count-1, content);

        // refresh all (except normal attack)
        for (int i = 1; i < player.skills.Count; ++i) {
            var entry = content.GetChild(i-1).GetChild(0); // slot entry
            entry.name = (i-1).ToString(); // for drag and drop
            var skill = player.skills[i];

            // overlay hotkey (without 'Alpha' etc.)
            var pretty = player.skillHotkeys[i].ToString().Replace("Alpha", "");
            entry.GetChild(1).GetComponentInChildren<Text>().text = pretty;

            // click event (done more than once but w/e)
            entry.GetComponent<Button>().onClick.RemoveAllListeners();
            int icopy = i;
            entry.GetComponent<Button>().onClick.SetListener(() => {
                OnSkillClicked(player, icopy);
            });
            entry.GetComponent<Button>().interactable = skill.learned;

            // hotkey pressed and not typing in any input right now?
            if (Input.GetKeyDown(player.skillHotkeys[i]) && !UIUtils.AnyInputActive())
                OnSkillClicked(player, i);
            
            // set state
            entry.GetComponent<UIShowToolTip>().enabled = true;
            entry.GetComponent<UIDragAndDropable>().dragable = skill.learned;
            // note: entries should be dropable at all times

            // image
            entry.GetComponent<Image>().sprite = skill.image;
            entry.GetComponent<UIShowToolTip>().text = skill.Tooltip();
            entry.GetComponent<Image>().color = skill.learned ? Color.white : Color.gray;

            // learn / upgrade button
            var buttonLearn = entry.transform.GetChild(2).GetComponent<Button>();
            // -> learnable?
            if (!skill.learned &&
                player.level >= skill.requiredLevel &&
                player.SkillpointsSpendable() > 0) {
                buttonLearn.gameObject.SetActive(true);
                buttonLearn.onClick.SetListener(() => { player.CmdLearnSkill(icopy); });
            // -> upgradeable?
            } else if (skill.learned &&
                       skill.level < skill.maxLevel &&
                       player.level >= skill.upgradeRequiredLevel &&
                       player.SkillpointsSpendable() > 0) {
                buttonLearn.gameObject.SetActive(true);
                buttonLearn.onClick.SetListener(() => { player.CmdUpgradeSkill(icopy); });
            // -> otherwise no button needed
            } else buttonLearn.gameObject.SetActive(false);

            // cooldown overlay
            var cd = skill.CooldownRemaining();
            if (skill.learned && cd > 0) {
                entry.transform.GetChild(0).gameObject.SetActive(true);
                entry.GetComponentInChildren<Text>().text = cd.ToString("F0");
            } else entry.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
