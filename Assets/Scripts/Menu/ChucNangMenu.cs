
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ChucNangMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    // start to set fullscreen
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    public void ChoiMoi()
    {
        SceneManager.LoadScene(1);
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

    /*List<int> widths = new List<int>() { 568, 960, 1200, 1920 };
    List<int> heights = new List<int>() { 329, 540, 800, 1080 };*/

    List<int> widths = new List<int>() { 1920 , 1200, 960, 568};
    List<int> heights = new List<int>() { 1080 , 800, 540, 329};

    public void SetScreenSize (int index)
    {
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void SetFullscreen (bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }
}
