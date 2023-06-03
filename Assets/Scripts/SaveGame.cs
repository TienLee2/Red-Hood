using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public GameObject player;
    public CharacterController2D playerController;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveToJson();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadToJson();
        }
    }

    public void SaveToJson()
    {
        Vector3 savePlayerPos;
        savePlayerPos = player.transform.position;



        PlayerPrefs.SetFloat("PlayerPositionX", savePlayerPos.x);
        PlayerPrefs.SetFloat("PlayerPositionY", savePlayerPos.y);
        PlayerPrefs.SetFloat("PlayerHealth", playerController.life);

        PlayerPrefs.Save();

    }

    public void LoadToJson()
    {
        float playerPosX = PlayerPrefs.GetFloat("PlayerPositionX");
        float playerPosY = PlayerPrefs.GetFloat("PlayerPositionY");
        float playerHealth = PlayerPrefs.GetFloat("PlayerHealth");

        playerController.life = playerHealth;
        player.transform.position = new Vector2(playerPosX, playerPosY);
    }
}
