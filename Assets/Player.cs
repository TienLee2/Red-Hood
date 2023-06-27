using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float directionX, directionY, moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 5f;
    }

    private void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        directionY = Input.GetAxisRaw("Vertical") * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(directionX, directionY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                Score.scoreAmount += 10;
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }

    }

    public int LevelCoin
    {
        get
        {
            return Score.scoreAmount / 15;
        }
        set
        {
            Score.scoreAmount = value * 15;
        }
    }




}