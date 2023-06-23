using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public GameObject player;
    public Transform afterTele;
    public SpriteRenderer rend;
    public bool isTeleporting;
    public bool finishTeleport;
    public float timeTele;
    public float hihi;

    private void Start()
    {
        rend = player.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isTeleporting)
        {
            hihi = Mathf.Lerp(rend.material.GetFloat("Vector1_A552F35F"), 0f, timeTele * Time.deltaTime);
            rend.material.SetFloat("Vector1_A552F35F", hihi);
        }

        if (finishTeleport)
        {
            hihi = Mathf.Lerp(rend.material.GetFloat("Vector1_A552F35F"), 5f, timeTele * 0.1f * Time.deltaTime);
            rend.material.SetFloat("Vector1_A552F35F", hihi);
        }
        

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isTeleporting = true;
            Invoke("Tele", 2f);
        }
    }

    public void Tele()
    {
        isTeleporting = false;
        finishTeleport = true;
        player.transform.position = new Vector2(afterTele.position.x, afterTele.position.y);
    }
}
