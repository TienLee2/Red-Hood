using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public GameObject hehe;
    private Sprite skillImage;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public Button upgradeButton;

    public int skillButtonId;

    private LevelSystemInterface levelSystem;

    private void Awake()
    {
        skillImage = hehe.GetComponent<Image>().sprite;
        upgradeButton.onClick.AddListener(UpgradeButton);

        var player = GameObject.FindGameObjectWithTag("Player");
        levelSystem = player.GetComponent<LevelSystemInterface>();
    }

    public void PressSkillButton()
    {
        SkillManager.instance.activateSkill = transform.GetComponent<Skill>();

        skillImage = SkillManager.instance.skills[skillButtonId].skillSprite;
        skillNameText.text = SkillManager.instance.skills[skillButtonId].skillName;
        skillDescriptionText.text = SkillManager.instance.skills[skillButtonId].skillDes;

        //Set the skill ID to the skill manager
        SkillManager.instance.activateSkillID = skillButtonId;
    }

    public void UpgradeButton()
    {
        if (!SkillManager.instance.skills[skillButtonId].isUpgrade
            && skillButtonId == SkillManager.instance.activateSkillID
            && levelSystem.SkillPoint() > 0)
        {
            levelSystem.SubtractSkillPoint(1);
            SkillManager.instance.skills[skillButtonId].isUpgrade = true;
        }
    }
}
