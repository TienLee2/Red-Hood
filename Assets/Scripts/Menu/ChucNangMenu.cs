
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ChucNangMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject continueMenu;

    // start to set fullscreen
    private void Awake()
    {
        int previousWidth = PlayerPrefs.GetInt("width");
        int previousHeight = PlayerPrefs.GetInt("height");
        int previousFullScreen = PlayerPrefs.GetInt("fullScreen");

        if(previousHeight > 0 && previousFullScreen == 1)
        {
            Screen.SetResolution(previousWidth, previousHeight, true);
        }
        else if(previousHeight > 0 && previousFullScreen == 0)
        {
            Screen.SetResolution(previousWidth, previousHeight, false);
        }
        else
        {
            Screen.SetResolution(1920, 1080, false);
        }

        


        Cursor.visible = true;
        if (PlayerPrefs.GetInt("NewGameUnlocked") == 1)
        {
            continueMenu.SetActive(true);
        }
        else
        {
            continueMenu.SetActive(false);
        }
    }

    public void ChoiMoi(string sceneName)
    {
        /*SceneManager.LoadScene(1);*/
        LevelManager.Instance.LoadScene(sceneName);
        PlayerPrefs.DeleteAll();
    }

    public void Continue(string sceneName)
    {
        LevelManager.Instance.LoadScene(sceneName);
    }

    public void Thoat()
    {
        Application.Quit();
    }

    public void CaiDat()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void TroLai()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }


    List<int> widths = new List<int>() { 1920, 1200, 960, 568 };
    List<int> heights = new List<int>() { 1080, 800, 540, 329 };

    public void SetScreenSize(int index)
    {
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, Screen.fullScreen);
        PlayerPrefs.SetInt("width", width);
        PlayerPrefs.SetInt("height", height);
    }

    public void SetFullscreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
        if (_fullscreen)
        {
            PlayerPrefs.SetInt("fullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullScreen", 0);
        }
    }
}
