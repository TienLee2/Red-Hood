using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{

    void Start()
    {
        
        int saveGame = PlayerPrefs.GetInt("AlreadyPlay");
        if (saveGame == 1)
        {
            LoadToJson();
        }
        else
        {
            transform.position = new Vector2(13.15f, -3.82f);
        }
        
    }


    public void LoadToJson()
    {
        float playerPosX = PlayerPrefs.GetFloat("PlayerPositionX_MainGame");
        float playerPosY = PlayerPrefs.GetFloat("PlayerPositionY_MainGame");

        transform.position = new Vector2(playerPosX, playerPosY);
    }
}
