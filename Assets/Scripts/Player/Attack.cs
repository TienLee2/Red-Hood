using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public GameObject Blood;

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
    private CharacterController2D player;

	private void Start()
	{
        player = GetComponent<CharacterController2D>();
    }

	private void Update()
    {
		//Nhấn X và tấn công
		if (Input.GetKeyDown(KeyCode.X) && canAttack && player.m_Grounded)
		{
			//chỉnh bool false để ko đánh nhiều lần được
			canAttack = false;
			//set animator tấn công
			animator.SetTrigger("Attacking");
            AudioManager.instance.PlaySFX("Attack");
            DoDashDamage();
            //đếm ngược thời gian để tiếp tục tấn công
            StartCoroutine(AttackCooldown(0.25f));

        }

		//Nhấn V để bắn, có thể tạo thêm code để đếm ngược thời gian có thể bắn
		if (Input.GetKeyDown(KeyCode.V) && canAttack && player.m_Grounded)
		{
            canAttack = false;
            //Tạo object cung tên
            GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,1f), Quaternion.identity) as GameObject; 
			//điều khiển hướng di chuyển của tên
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
            StartCoroutine(AttackCooldown(1f));
        }
	}

	IEnumerator AttackCooldown(float time)
	{
		yield return new WaitForSeconds(time);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 1f);
		
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                Instantiate(Blood, attackCheck.position, Quaternion.identity);
                Debug.Log("Hit");
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}
}
