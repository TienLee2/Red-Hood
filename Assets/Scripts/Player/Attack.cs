using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	//damage
	public float dmgValue = 4;
	//Mũi tên để bắn
	public GameObject throwableObject;

	public Transform attackCheck;

	//Cái để điều khiển animation
	public Animator animator;
	//bool 
	public bool canAttack = true;

	public bool isTimeToCheck = false;
	//Camera
	public GameObject cam;


    private void Update()
    {
		//Nhấn X và tấn công
		if (Input.GetKeyDown(KeyCode.X) && canAttack)
		{
			//chỉnh bool false để ko đánh nhiều lần được
			canAttack = false;
			//set animator tấn công
			animator.SetBool("IsAttacking", true);
			//đếm ngược thời gian để tiếp tục tấn công
			StartCoroutine(AttackCooldown());
		}

		//Nhấn V để bắn, có thể tạo thêm code để đếm ngược thời gian có thể bắn
		if (Input.GetKeyDown(KeyCode.V))
		{
			//Tạo object cung tên
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
			//điều khiển hướng di chuyển của tên
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
		}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}
}
