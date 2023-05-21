using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //Paricle system quan li hieu ung
    public ParticleSystem leafParticle;

    void OnTriggerEnter2D(Collider2D col)
    {
        //tính hướng giữa vật thể và vật va chạm
        if (transform.position.x - col.transform.position.x > 0) 
        {
            GetComponent<Animator>().Play("MovingGrassL");
        }
        else 
        {
            GetComponent<Animator>().Play("MovingGrassR");
        }
    }

    public void ApplyDamage(float damage)
    {
        Instantiate(leafParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}