using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScreen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.LoadScene("MainScreen");
            PlayerPrefs.SetInt("NewGameUnlocked",1);
        }
        
    }
}
