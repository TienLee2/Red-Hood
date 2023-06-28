using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowDoubleJump : MonoBehaviour
{
    public GameObject player;
    public GameObject jump;
    public GameObject otherJump;
    public CharacterController2D characterController;
    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;
    Vector3 pos;

    private void Start()
    {
        characterController = player.GetComponent<CharacterController2D>();
        pos = transform.position;
    }

    void Update()
    {
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector2(transform.position.x, newY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            characterController._doubleJumpUnlocked = true;
            PlayerPrefs.SetInt("DoubleJump",1);
            jump.SetActive(true);
            Destroy(gameObject);
            otherJump.SetActive(false);
        }
    }
}
