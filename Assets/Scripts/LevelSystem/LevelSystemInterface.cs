using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSystemInterface : MonoBehaviour
{
    public ParticleSystem levelUp;
    [SerializeField] private LevelSystem levelSystem;
    private PlayerSkills playerSkills;

    public TextMeshProUGUI skillPointText;

    private void Awake()
    {
        levelSystem = new LevelSystem();
        skillPointText.text = levelSystem.GetSkillPointNumber() + " Point";
        SetPoint(PlayerPrefs.GetInt("SkillPoint"));

        /*playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;*/

    }

    /*private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        //Debug.Log("Yes skill Unlocked");
    }*/

    private void Update()
    {
        skillPointText.text = levelSystem.GetSkillPointNumber() + " Point";
    }

    public void SetLevelSystem(int levelPoint)
    {
        levelSystem.AddExperience(levelPoint);
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    public void SubtractSkillPoint(int point)
    {
        levelSystem.SubtractSkillPoint(point);
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        //If lEvel up 
        levelUp.Play();
    }

    public void SetPoint(int point)
    {
        levelSystem.SetLevelPoint(point);
    }

    public int SkillPoint()
    {
        return levelSystem.GetSkillPointNumber();
    }



    /*
    public void HealthUp()
    {
        playerSkills.UnlockedSkill(PlayerSkills.SkillType.HealthUp);
    }

    public void DashDamage()
    {
        playerSkills.UnlockedSkill(PlayerSkills.SkillType.DashDamage);
    }*/
}
