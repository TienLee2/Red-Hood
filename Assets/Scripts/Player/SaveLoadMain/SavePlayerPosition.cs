using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPosition : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            SaveToJson();
        }
    }

    public void SaveToJson()
    {
        Vector3 savePlayerPos;
        savePlayerPos = player.transform.position;
        PlayerPrefs.SetInt("AlreadyPlay",1);
        PlayerPrefs.SetFloat("PlayerPositionX_MainGame", savePlayerPos.x);
        PlayerPrefs.SetFloat("PlayerPositionY_MainGame", savePlayerPos.y);

        PlayerPrefs.Save();

    }
}
