using UnityEngine;
using UnityEngine.UI;


public class PlayerTouchMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public Button leftButton;
    public Button rightButton;
    public Button menuButton;
    public Button dashButton;
    public Button jumpButton;
    public Button attackButton;
    public Button inventoryButton;
    public Button shotButton;
    public bool canAttack;
    private float horizontalMove = 0f;
    [SerializeField] private bool isMovingLeft = false;
    [SerializeField] private bool isMovingRight = false;
    private bool jump = false;
    private bool dash = false;

    private bool isMoving = false;
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (controller.life > 0)
        {
            if (controller.canMove)
            {
                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            }
            else
            {
                animator.SetFloat("Speed", 0);
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

    public void MoveLeft()
    {
        isMovingLeft = true;
        isMovingRight = false;
        horizontalMove = -runSpeed;
    }

    public void MoveRight()
    {
        isMovingRight = true;
        isMovingLeft = false;
        horizontalMove = runSpeed;
    }

    public void StopMovement()
    {
        if (!isMoving)
        {
            isMovingLeft = false;
            isMovingRight = false;
            horizontalMove = 0f;
        }

    }

    public void UseDash()
    {
        dash = true;
        animator.ResetTrigger("Attacking1");
        animator.ResetTrigger("Attacking2");
        animator.ResetTrigger("Attacking3");
    }

    public void Shoot()
    {
        // Xử lý khi nút bắn được nhấn
        // Gọi phương thức Shoot() trong script của bạn
        Debug.Log("Shoot"); // Ví dụ: In ra thông báo "Shoot"
    }

    public void Jump()
    {
        if (controller.canMove)
        {
            jump = true;
            animator.SetBool("Jumping", true);
            AudioManager.instance.PlaySFX("Jump");
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

    private void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }
}
