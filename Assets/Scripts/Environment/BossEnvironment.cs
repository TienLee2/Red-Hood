using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnvironment : MonoBehaviour
{
    public Boss boss;
    public GameObject wall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.enemy = other.gameObject;
            wall.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
