using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneLoad : MonoBehaviour
{
    public bool tutorial = false;
    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        if (tutorial)
        {
            LoadTutorialToGame();
        }
        else
        {
            LoadEndGame();
        }
    }

    public void LoadTutorialToGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadEndGame()
    {
        SceneManager.LoadScene(0);
    }
}
