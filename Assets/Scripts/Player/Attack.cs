﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject Blood;
    public Transform bloodPosition;
    //damage
    public float dmgValue = 4;
    public int combo;
    public bool Attacking;
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

    private bool Upgraded = false;


    private void Start()
    {
        combo = 1;
        player = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < player._skillUnlocked.Length; i++)
        {
            //Attack up
            if (player._skillUnlocked[3])
            {
                if (!Upgraded)
                {
                    dmgValue += 2;
                    Upgraded = true;
                }
                
            }
        }

    }

    private void Update()
    {
        Combos();
        DoDashDamage();

        //Nhấn V để bắn, có thể tạo thêm code để đếm ngược thời gian có thể bắn
        if (Input.GetKeyDown(KeyCode.V) && canAttack && player.m_Grounded)
        {
            ShootSkill();
        }


        if (player._skillUnlocked[3])
        {
            if (!Upgraded)
            {
                dmgValue += 2;
                Upgraded = true;
            }

        }
    }


    public void AttackSkill()
    {
        //chỉnh bool false để ko đánh nhiều lần được
        canAttack = false;
        animator.SetTrigger("Attacking" + combo);
        
        AudioManager.instance.PlaySFX("Attack");
        //đếm ngược thời gian để tiếp tục tấn công

        if (player._skillUnlocked[4])
        {
            if (combo == 1)
            {
                StartCoroutine(AttackCooldown(0.1f));
            }
            else if(combo == 2)
            {
                Debug.Log("Wait");
                StartCoroutine(AttackCooldown(1.5f));
            }
        }
        else if (player._skillUnlocked[5])
        {
            if (combo == 1)
            {
                StartCoroutine(AttackCooldown(0.1f));
            }
            else if (combo == 2)
            {
                StartCoroutine(AttackCooldown(0.1f));
            }
            else if (combo == 3)
            {
                StartCoroutine(AttackCooldown(1.5f));
            }
        }
        else
        {
            StartCoroutine(AttackCooldown(1f));
        }
    }

    public void MobileAttack()
    {
        if (canAttack && player.m_Grounded)
        {
            AttackSkill();
        }
    }

    public void MobileShoot()
    {
        if (canAttack && player.m_Grounded)
        {
            ShootSkill();
        }
    }

    public void ShootSkill()
    {
        player.canMove = false;
        canAttack = false;
        AudioManager.instance.PlaySFX("RangeAttack");
        animator.SetTrigger("rangeAttack");
        StartCoroutine(Shoot(0.5f));
    }

    public void StartCombo()
    {
        Attacking = false;
        if (combo < 4)
        {
            combo++;
        }
    }

    public void FinishCombo()
    {
        Attacking = false;
        animator.ResetTrigger("Attacking" + combo);
        combo = 1;
    }

    public void Combos()
    {
        if (Input.GetKeyDown(KeyCode.X) && canAttack && player.m_Grounded)
        {
            AttackSkill();
        }
    }

    IEnumerator Shoot(float time)
    {
        combo = 1;
        yield return new WaitForSeconds(time);
        //Tạo object cung tên
        GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, 1f), Quaternion.identity) as GameObject;
        throwableWeapon.transform.localScale = new Vector3(gameObject.transform.localScale.x, throwableObject.transform.localScale.y, throwableObject.transform.localScale.z);
        //điều khiển hướng di chuyển của tên
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
        throwableWeapon.name = "ThrowableWeapon";
        if (player._skillUnlocked[6])
        {
            StartCoroutine(AttackCooldown(0.7f));
        }
        else
        {
            StartCoroutine(AttackCooldown(1f));
        }
        player.canMove = true;
    }


    IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;

    }

    public void SetAttack()
    {
        Attacking = true;
    }

    public void StopAttack()
    {
        Attacking = false;
    }

    public void DoDashDamage()
    {
        if (Attacking)
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
                    Instantiate(Blood, bloodPosition.position, Quaternion.identity);
                    Attacking = false;
                    cam.GetComponent<CameraFollow>().ShakeCamera();
                }
            }

        }

    }
}
