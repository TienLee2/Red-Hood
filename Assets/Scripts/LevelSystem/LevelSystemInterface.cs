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


    private void Update()
    {
        skillPointText.text = levelSystem.GetSkillPointNumber() + " Point";
    }

    public void SetLevelSystem(int levelPoint)
    {
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        levelSystem.AddExperience(levelPoint);
        
    }

    public void SubtractSkillPoint(int point)
    {
        levelSystem.SubtractSkillPoint(point);
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        //If lEvel up 
        levelUp.Play();
        Debug.Log("Level UP");

    }

    public void SetPoint(int point)
    {
        levelSystem.SetLevelPoint(point);
    }

    public int SkillPoint()
    {
        return levelSystem.GetSkillPointNumber();
    }
}
