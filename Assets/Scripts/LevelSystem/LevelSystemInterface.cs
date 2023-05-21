using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemInterface : MonoBehaviour
{
    [SerializeField] private LevelSystem levelSystem;

    private void Awake()
    {
        levelSystem = new LevelSystem();
    }

    public void SetLevelSystem(int levelPoint)
    {
        levelSystem.AddExperience(levelPoint);
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

        Debug.Log("Current level " + levelSystem.GetLevelNumber());
        //Debug.Log("Current Experience " + levelSystem.GetExperienceNormalized());
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        Debug.Log("Level UP");
    }
}
