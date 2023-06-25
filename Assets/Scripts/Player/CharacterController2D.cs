using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class CharacterController2D : MonoBehaviour
{

    // Run and Jump Effect  
    public ParticleSystem dust;


    //l?c nh?y
    [SerializeField] private float m_JumpForce = 400f;
    //smooth nhân v?t khi di chuy?n
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement

    //ng??i ch?i có th? ?i?u khi?n nhân v?t trên không trung hay ko
    [SerializeField] private bool m_AirControl = false;
    //quy?t ??nh xem cái nào là m?t ??t
    [SerializeField] private LayerMask m_WhatIsGround;
    //v? trí nhân v?t ch?m ??t
    [SerializeField] private Transform m_GroundCheck;
    //v? trí nhân v?t ch?m t??ng
    [SerializeField] private Transform m_WallCheck;

    //vòng tròn nh? d??i chân nv ?? detect coi có ch?m ??t không
    const float k_GroundedRadius = .2f;
    //Bool check nv ch?m ??t
    public bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    //Quy?t ??nh nv ?ang quay m?t v? ?âu
    private bool m_FacingRight = true;
    private Vector3 velocity = Vector3.zero;
    //gi?i h?n t?c ?? r?i
    private float limitFallSpeed = 25f;

    //Cho phép double jump
    public bool canDoubleJump = true;
    //L?c dash
    [SerializeField] private float m_DashForce = 25f;
    //Bool check dash
    private bool canDash = true;
    //bool check nv có ?ang dash hay không
    private bool isDashing = false;
    //bool check nv có ?ang ??ng tr??c t??ng hay không
    private bool m_IsWall = false;
    //bool check n?u nhân v?t có ?ang l??t trên t??ng
    private bool isWallSliding = false;
    //bool check n?u nhân v?t có ?ang l??t trên t??ng ? frame tr??c không
    private bool oldWallSlidding = false;
    //bool check n?u nv có ?ang chu?n b? l??t t??ng hay không
    private bool canCheck = false;

    public float life; //Life of the player
    public float maxLife = 10f;
    public bool invincible = false; //If player can die
    public bool canMove = true; //If player can move

    private Animator animator;
    public ParticleSystem particleJumpUp; //Trail particles
    public ParticleSystem particleJumpDown; //Explosion particles

    private float jumpWallStartX = 0;
    private float jumpWallDistX = 0; //Distance between player and wall
    private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

    [Header("Events")]
    [Space]

    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [SerializeField] private int _jumpTime;
    private bool bossBattle;
    public HealthBar healthBar;

    private PlayerMovement _movement;
    private Attack attack;

    public bool[] _skillUnlocked;
    public bool _doubleJumpUnlocked;

    


    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        attack = GetComponent<Attack>();

        for(int i = 0; i < _skillUnlocked.Length; i++)
        {
            int skillUnlocked = PlayerPrefs.GetInt("SkillUnlocked" + i);
            if(skillUnlocked == 1)
            {
                _skillUnlocked[i] = true;
            }
        }

        int doubleJump = PlayerPrefs.GetInt("DoubleJump");
        if(doubleJump == 1)
        {
            _doubleJumpUnlocked = true;
        }
        else
        {
            _doubleJumpUnlocked = false;
        }

        life = maxLife;
        healthBar.SetMaxHealth(maxLife);

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void Start()
    {
        // Health up
        if (_skillUnlocked[0])
        {
            maxLife += 5f;
        }
        //Run faster
        if (_skillUnlocked[1])
        {
            _movement.runSpeed += 10f;
        }
        //Attack chain 2
        if (_skillUnlocked[2])
        {
            animator.SetInteger("Unlock", 1);
        }
        //Attack chain 3
        if (_skillUnlocked[3])
        {
            animator.SetInteger("Unlock", 2);
        }
    }

    private void Update()
    {
        if (!_doubleJumpUnlocked)
        {
            canDoubleJump = false;
        }

        //Attack chain 2
        if (_skillUnlocked[4])
        {
            animator.SetInteger("Unlock", 1);
            //Attack chain 3
            if (_skillUnlocked[5])
            {
                animator.SetInteger("Unlock", 2);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Health up
            if (_skillUnlocked[0])
            {
                maxLife += 5f;
                life = maxLife;
            }
            //Run faster
            if (_skillUnlocked[1])
            {
                _movement.runSpeed += 10f;
            }
        }
    }


    private void FixedUpdate()
    {
        if (life > 0)
        {
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    if (!m_IsWall && !isDashing)
                        particleJumpDown.Play();
                    canDoubleJump = true;
                    if (m_Rigidbody2D.velocity.y < 0f)
                        limitVelOnWallJump = false;
                }
            }

            m_IsWall = false;

            if (!m_Grounded)
            {
                OnFallEvent.Invoke();
                Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
                for (int i = 0; i < collidersWall.Length; i++)
                {
                    if (collidersWall[i].gameObject != null)
                    {
                        isDashing = false;
                        m_IsWall = true;
                    }
                }
            }

            if (limitVelOnWallJump)
            {
                if (m_Rigidbody2D.velocity.y < -0.5f)
                    limitVelOnWallJump = false;
                jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
                if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
                {
                    canMove = true;
                }
                else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
                {
                    canMove = true;
                    m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
                }
                else if (jumpWallDistX < -2f)
                {
                    limitVelOnWallJump = false;
                    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                }
                else if (jumpWallDistX > 0)
                {
                    limitVelOnWallJump = false;
                    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                }
            }
        }

    }



    public void Move(float move, bool jump, bool dash)
    {
        if (canMove)
        {
            /*if (move != 0)
            {
                CreateDust();
            }*/

            if (m_Grounded)
            {
                _jumpTime = 0;
            }

            if (dash && canDash && !isWallSliding)
            {
                /*m_Rigidbody2D.velocity = new Vector2(transform.localScale.x, 0);*/
                m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
                StartCoroutine(DashCooldown());

            }
            // If crouching, check to see if the character can stand up
            if (isDashing)
            {
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }
            //only control the player if grounded or airControl is turned on
            else if (m_Grounded || m_AirControl)
            {
                if (m_Rigidbody2D.velocity.y < -limitFallSpeed)

                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight && !isWallSliding)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight && !isWallSliding)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                _jumpTime += 1;
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                canDoubleJump = true;
                CreateDust();
                particleJumpDown.Play();
                particleJumpUp.Play();
            }
            else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
            {
                canDoubleJump = false;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
                animator.SetBool("IsDoubleJumping", true);
                CreateDust();
            }
            else if (m_IsWall && !m_Grounded)
            {
                if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
                {
                    isWallSliding = true;
                    m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
                    Flip();
                    StartCoroutine(WaitToCheck(0.1f));
                    canDoubleJump = true;
                    animator.SetBool("IsWallSliding", true);
                }
                isDashing = false;

                if (isWallSliding)
                {
                    if (move * transform.localScale.x > 0.1f)
                    {
                        StartCoroutine(WaitToEndSliding());
                    }
                    else
                    {
                        oldWallSlidding = true;
                        m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
                    }
                }

                if (jump && isWallSliding && _jumpTime != 2)
                {
                    _jumpTime += 1;
                    m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                    m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce * 1.2f, m_JumpForce));
                    jumpWallStartX = transform.position.x;
                    limitVelOnWallJump = true;
                    canDoubleJump = true;
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canMove = false;
                }
                else if (dash && canDash)
                {
                    isWallSliding = false;
                    animator.SetBool("IsWallSliding", false);
                    oldWallSlidding = false;
                    m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                    canDoubleJump = true;
                    StartCoroutine(DashCooldown());
                }
            }
            else if (isWallSliding && !m_IsWall && canCheck)
            {
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                oldWallSlidding = false;
                m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
                canDoubleJump = true;
            }
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(0, 0);
        }
    }


    private void Flip()
    {
        CreateDust();
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    private void CreateDust()
    {
        dust.Play();
    }

    public void ApplyDamage(float damage, Vector3 position)
    {
        if (!invincible)
        {
            
            life -= damage;
            healthBar.SetHealth(life);

            animator.SetTrigger("Hurt");
            animator.ResetTrigger("Attacking1");
            animator.ResetTrigger("Attacking2");
            animator.ResetTrigger("Attacking3");
            attack.combo = 1;

            _movement.jump = false;
            AudioManager.instance.PlaySFX("Hit");
            Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(damageDir * 10);

            if (life <= 0)
            {
                StartCoroutine(WaitToDead());
            }
            else
            {
                StartCoroutine(Stun(0.25f));
                StartCoroutine(MakeInvincible(1f));
            }
        }
    }

    IEnumerator DashCooldown()
    {

        animator.SetTrigger("IsDashing");
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        yield return new WaitForSeconds(2f);
        canDash = true;
    }

    IEnumerator Stun(float time)
    {
        _movement.jump = false;
        animator.SetBool("Jumping", false);
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }
    IEnumerator WaitToMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator WaitToCheck(float time)
    {
        canCheck = false;
        yield return new WaitForSeconds(time);
        canCheck = true;
    }

    IEnumerator WaitToEndSliding()
    {
        yield return new WaitForSeconds(0.1f);
        canDoubleJump = true;
        isWallSliding = false;
        animator.SetBool("IsWallSliding", false);
        oldWallSlidding = false;
        m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
    }

    IEnumerator WaitToDead()
    {
        animator.SetBool("Dead", true);
        canMove = false;
        invincible = true;
        GetComponent<Attack>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
