using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject player;
    public CharacterController2D playerController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveToJson();
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
