using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : MonoBehaviour
{
    public GameObject player;
    public CharacterController2D characterController;
    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;
    Vector3 pos;

    public GameObject healthIcon;
    private HealthBar healthBar;

    private void Start()
    {
        healthIcon = GameObject.FindGameObjectWithTag("HealthBar");
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2D>();
        healthBar = healthIcon.GetComponent<HealthBar>();
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
            AudioManager.instance.PlaySFX("Pop");
            characterController.life += 2;
            healthBar.SetHealth(characterController.life);
            Destroy(gameObject);
        }
    }
}
