using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBossPlayerPos : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveToJson();
        }
    }

    public void SaveToJson()
    {
        Vector3 savePlayerPos;
        savePlayerPos = player.transform.position;
        PlayerPrefs.SetInt("AlreadyPlayBoss", 1);
        PlayerPrefs.SetFloat("PlayerPositionX_Boss", savePlayerPos.x);
        PlayerPrefs.SetFloat("PlayerPositionY_Boss", savePlayerPos.y);

        PlayerPrefs.Save();

    }
}
