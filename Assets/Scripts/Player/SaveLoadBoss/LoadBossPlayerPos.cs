using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBossPlayerPos : MonoBehaviour
{
    void Start()
    {

        int saveGame = PlayerPrefs.GetInt("AlreadyPlayBoss");
        if (saveGame == 1)
        {
            LoadToJson();
        }
        else
        {
            transform.position = new Vector2(7f, 0f);
        }

    }


    public void LoadToJson()
    {
        float playerPosX = PlayerPrefs.GetFloat("PlayerPositionX_Boss");
        float playerPosY = PlayerPrefs.GetFloat("PlayerPositionY_Boss");

        transform.position = new Vector2(playerPosX, playerPosY);
    }
}
