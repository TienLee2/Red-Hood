using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    public event EventHandler OnLevelPointChanged;

    private int level;
    private int levelPoint;
    private int experience;
    private int experienceToNextLevel;

    public LevelSystem()
    {
        level = 0;
        levelPoint = 0;
        experience = 0;
        experienceToNextLevel = 100;
    }

    public void AddExperience(int amount)
    {
        experience += amount;

        if(experience>= experienceToNextLevel)
        {
            level++;
            levelPoint++;
            experienceToNextLevel += 20;
            experience -= experienceToNextLevel;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            if (OnLevelPointChanged != null) OnLevelPointChanged(this, EventArgs.Empty);
        }

        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);

    }

    public void SubtractSkillPoint(int amount)
    {
        if(levelPoint > 0)
        {
            levelPoint -= amount;
            if(OnLevelPointChanged!=null) OnLevelPointChanged(this, EventArgs.Empty);
        }
    }

    public int GetSkillPointNumber()
    {
        return levelPoint;
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public void SetLevelPoint(int point)
    {
        levelPoint = point;
    }

    public float GetExperienceNormalized()
    {
        return (float)experience/experienceToNextLevel;
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }


}
