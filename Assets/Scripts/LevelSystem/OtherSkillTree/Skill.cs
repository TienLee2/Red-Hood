using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    //Make the skill name and sprite
    public string skillName;
    public Sprite skillSprite;

    [TextArea(1,3)]
    //Also the skill description
    public string skillDes;

    //Check if the skill is upgraded or not
    public bool isUpgrade;

    private void Update()
    {
        if (isUpgrade)
        {
            var currentImage = GetComponent<Image>();
            currentImage.color = new Color32(101,101,101,246);
        }   
    }
}
