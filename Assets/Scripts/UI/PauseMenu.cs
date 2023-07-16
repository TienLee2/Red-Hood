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

    public bool mobileDevice;

    private void Start()
    {
        mobileDevice = true;
        if (mobileDevice)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 144;
        }
        
        Time.timeScale = 1;
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
                Cursor.visible = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!inventoryActive)
            {
                Time.timeScale = 0;
                inventory.SetActive(true);
                inventoryActive = true;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                inventory.SetActive(false);
                inventoryActive = false;
                Cursor.visible = false;
            }
            
        }
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseMenuActive = false;
        inventory.SetActive(false);
        inventoryActive = false;
    }

    public void BackToMenu(string sceneName)
    {
        Time.timeScale = 1;
        LevelManager.Instance.LoadScene(sceneName);
    }
}
