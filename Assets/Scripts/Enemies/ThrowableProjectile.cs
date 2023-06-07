using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableProjectile : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed = 15f;
	public GameObject owner;

    
    void FixedUpdate()
    {
		if ( !hasHit)
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Neu player dinh dan thi se nhan dame
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
			Destroy(gameObject);
		}
		//Neu Enemy dinh dan thi
		//cung se nhan dame
		//Neu vat dinh dan ko phai player hay enemy thi se huy vien dan
		if (collision.gameObject.layer ==3)
		{
			Destroy(gameObject);
		}
	}
}
