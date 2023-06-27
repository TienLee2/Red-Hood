using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreAmount;

    public static int levelAmount;
    public Text levelText;


    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<Text>();
        levelAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Score: " + scoreAmount);

        Player myPlayer = new Player();

        levelAmount = myPlayer.LevelCoin;
        levelText.text = "Level: " + levelAmount;

    }


}

