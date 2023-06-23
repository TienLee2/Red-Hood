using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryActive = false;

    public GameObject pauseMenu;
    private bool pauseMenuActive = false;

    private void Start()
    {
        inventory.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenuActive)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pauseMenuActive = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!inventoryActive)
            {
                Time.timeScale = 0;
                inventory.SetActive(true);
                inventoryActive = true;
            }
            else
            {
                Time.timeScale = 1;
                inventory.SetActive(false);
                inventoryActive = false;
            }
            
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseMenuActive = false;
    }

    public void BackToMenu(string sceneName)
    {
        LevelManager.Instance.LoadScene(sceneName);
    }
}
