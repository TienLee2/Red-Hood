using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills 
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None,
        HealthUp,
        SpeedUp,
        DashDamage,
        AttackUp,
        BigAttack1,
        BigAttack2,
        BigAttack3,
        BowUp,
        BowSpeedUp,
    }

    private List<SkillType> unlockedSkillTypeList;

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    public void UnlockedSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
        
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {

        }
        return SkillType.None;
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if(IsSkillUnlocked(skillRequirement))
            {
                UnlockedSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }

    }
}
