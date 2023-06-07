using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
    private CharacterController2D player;
    //bool dashAxis = false;

    private void Start()
    {
        player = GetComponent<CharacterController2D>();
    }

    void Update () {
		if(player.life > 0)
		{
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetKeyDown(KeyCode.Z))
            {
                jump = true;
                animator.SetBool("Jumping", true);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                dash = true;
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
