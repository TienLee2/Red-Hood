using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	public bool jump = false;
	bool dash = false;
    //bool dashAxis = false;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    void Update () {
		if(controller.life > 0)
		{
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (controller.canMove)
            {
                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (controller.canMove)
                {
                    jump = true;
                    animator.SetBool("Jumping", true);
                    AudioManager.instance.PlaySFX("Jump");
                }
                
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                dash = true;
                animator.ResetTrigger("Attacking1");
                animator.ResetTrigger("Attacking2");
                animator.ResetTrigger("Attacking3");
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Jumping", false);
            animator.SetBool("isFall", false);
            jump = false;
            dash = false;
        }
		
	}

    public void WalkingSFX()
    {
        int randomSound = Random.Range(0, 10);
        if (randomSound % 2 == 0)
        {
            AudioManager.instance.PlaySFX("Walk1");
        }
        else
        {
            AudioManager.instance.PlaySFX("Walk2");
        }
    }

	public void OnFall()
	{
		animator.SetBool("isFall", true);
        animator.SetBool("Jumping", false);
    }

	public void OnLanding()
	{
        animator.SetBool("Jumping", false);
        animator.SetBool("isFall", false);
    }

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}
