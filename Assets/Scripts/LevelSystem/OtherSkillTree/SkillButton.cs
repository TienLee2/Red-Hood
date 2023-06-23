using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public GameObject hehe;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public Button upgradeButton;

    public int skillButtonId;

    private LevelSystemInterface levelSystem;
    private CharacterController2D characterController;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(UpgradeButton);

        var player = GameObject.FindGameObjectWithTag("Player");
        levelSystem = player.GetComponent<LevelSystemInterface>();
        characterController = player.GetComponent<CharacterController2D>();
        
    }

    private void Start()
    {
        int skillUnlocked = PlayerPrefs.GetInt("SkillUnlocked" + skillButtonId);
        if (skillUnlocked == 1)
        {
            characterController._skillUnlocked[skillButtonId] = true;
            SkillManager.instance.skills[skillButtonId].isUpgrade = true;
        }
    }

    public void PressSkillButton()
    {
        SkillManager.instance.activateSkill = transform.GetComponent<Skill>();
        PlayerPrefs.SetInt("SkillPoint", levelSystem.SkillPoint());

        hehe.GetComponent<Image>().sprite = SkillManager.instance.skills[skillButtonId].skillSprite;
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
            PlayerPrefs.SetInt("SkillPoint", levelSystem.SkillPoint());


            for (int i = 0; i <= characterController._skillUnlocked.Length; i++)
            {
                if (i == skillButtonId)
                {
                    characterController._skillUnlocked[i] = true;
                    PlayerPrefs.SetInt("SkillUnlocked" + skillButtonId, 1);
                }
            }
        }
    }
}
