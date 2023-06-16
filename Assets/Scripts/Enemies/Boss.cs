using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    [Header("Health")]
    [SerializeField] private float life;
    public float maxLife;

    public float speed = 5f;
    [SerializeField] private float m_DashForce = 25f;

    public GameObject enemy;
    private float distToPlayer;
    public float meleeDist = 1.5f;
    public float meleeDistState2 = 4f;

    [Header("Attack")]
    private bool state2 = false;
    [SerializeField] private Transform attackCheck1;
    [SerializeField] private Transform attackCheck3;
    [SerializeField] private Transform attackCheck4;
    private int randomAttack;
    public float dmgValue = 4;

    [SerializeField]private float randomDecision = 0;
    private bool doOnceDecision = true;
    [SerializeField] private bool endDecision = false;

    [SerializeField] private Animator anim;
    [SerializeField] private LevelSystemInterface playerLevel;

    private bool isDashing = false;
    private bool canAttack = true;
    private bool m_FacingRight = true;
    private bool facingRight = true;
    private bool isRunning;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool availableToAttack;

    private bool dead = false;
    public bool isInvincible = false;
    private bool isHitted = false;


    //Camera
    [Header("Shake The Camera")]
    public float shakeDuration = 0f;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    [SerializeField] private Transform camTransform;
    Vector3 originalPos;

    private CapsuleCollider2D capsule;



    private void Awake()
    {
        enemy = null;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        //lay script kinh nghiem tu gameObject gan vao playerLevel
        playerLevel = playerGameObject.GetComponentInParent<LevelSystemInterface>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        life = maxLife;
        capsule = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        InvokeRepeating("RandomD", 1f, 1f);
    }

    private void Update()
    {
        ReviveToState2();
        CameraShake();
        AnimatorController();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //N?u life b� h?n 0 th� ch?t
        if (life <= 0)
        {
            if (!dead)
            {
                //Th�m kinh nghi?m
                playerLevel.SetLevelSystem(500);
                //bool dead s? ???c ch?nh th�nh true ?? tr�nh l?p l?i
                dead = true;
            }

            StartCoroutine(DestroyEnemy());
        }
        //N?u enemy kh�ng c�
        if (enemy != null)
        {
            //N?u c?n dash
            if (isDashing)
            {
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }
            //N?u ch?a b? ?�nh

            //t�m kho?ng c�ch gi?a k? ??ch v� b?n th�n
            distToPlayer = enemy.transform.position.x - transform.position.x;

            if (randomAttack == 6 || randomAttack ==5)
            {
                if (state2 == true && isRunning == false)
                {
                    if (Mathf.Abs(distToPlayer) > meleeDistState2)
                    {
                        JumpAttack();
                    }
                }
            }

            if (Mathf.Abs(distToPlayer) < meleeDist)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
                if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                {
                    if (!isAttacking)
                    {
                        Flip();
                    }
                }
                else
                {
                    if (canAttack)
                    {
                        MeleeAttack();
                    }

                }
            }
            else
            {
                //ng?u nhi�n quy?t ??nh h�nh ??ng
                if (!endDecision)
                {
                    if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                    {
                        if (!isAttacking)
                        {
                            Flip();
                        }
                    }
                    if (randomDecision <= 6f)
                        Run();
                    else if (randomDecision > 6f)
                        Idle();
                    else
                        Run();
                }
                else
                {
                    endDecision = false;
                }
            }


            if (isHitted)
            {

            }
        }

        if (!isAttacking)
        {
            if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
            {
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
            {
                Flip();
            }
        }

    }


    private void AnimatorController()
    {
        if (isRunning)
        {
            anim.SetBool("Run", true);
            anim.SetInteger("Attack", 0);
        }
        else
        {
            anim.SetBool("Run", false);
        }



        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
        {
            capsule.isTrigger = true;
            isInvincible = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack4")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
        {
            isRunning = true;
            m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * 4, m_Rigidbody2D.velocity.y);
            if (doOnceDecision)
                StartCoroutine(NextDecision(0.5f));
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7)
        {
            if (availableToAttack)
            {
                DealDamage();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.45)
        {
            if (availableToAttack)
            {
                DealDamage();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9)
        {
            if (availableToAttack)
            {
                JumpAttackDamaged();
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack4")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9)
        {
            if (availableToAttack)
            {
                SpinAttackDamaged();
            }
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack4") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
        {
            isInvincible = false;
            capsule.isTrigger = false;
            isAttacking = false;
            anim.SetInteger("Action", 0);
        }
    }

    private void CameraShake()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    void Flip()
    {
        //xoay nh�n v?t n�n s? ??i bool 
        facingRight = !facingRight;

        //??i tr?c x ng??c v?i h? t?a ??
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //function t�nh damage b? d�nh ph?i
    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            damage = Mathf.Abs(damage);
            anim.SetBool("Hit", true);
            life -= damage;

            StartCoroutine(HitTime());
        }
    }

    //function ?�nh t?m g?n
    public void MeleeAttack()
    {
        isRunning = false;
        isAttacking = true;

        if (!state2)
        {
            switch (randomAttack)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    anim.SetInteger("Attack", 1);
                    break;

                case 4:
                case 5:
                case 6:
                case 7:
                    anim.SetInteger("Attack", 2);
                    break;
            }
        }
        else
        {
            switch (randomAttack)
            {
                case 0:
                case 1:
                case 2:
                    anim.SetInteger("Attack", 1);
                    break;
                case 3:
                case 4:
                    anim.SetInteger("Attack", 2);
                    break;
                case 5:
                case 6:
                case 7:
                    anim.SetInteger("Attack", 4);
                    break;
            }
        }


        availableToAttack = true;

        //sau khi t?n c�ng ??i 0.5 gi�y ?? ti?p t?c t?n c�ng
        StartCoroutine(WaitToAttack(4f));
    }

    public void JumpAttack()
    {
        isRunning = false;
        isAttacking = true;
        anim.SetInteger("Attack", 3);
        availableToAttack = true;
        StartCoroutine(WaitToAttack(4f));
    }

    //?�nh t?m xa
    public void DealDamage()
    {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck1.position, 1f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Player")
            {
                collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
                ShakeCamera(0.5f);
            }
        }
        availableToAttack = false;
        StartCoroutine(WaitToAttack(1f));
    }

    public void JumpAttackDamaged()
    {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck3.position, 2f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Player")
            {
                collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
                ShakeCamera(1f);

            }
        }
        availableToAttack = false;
        StartCoroutine(WaitToAttack(1f));

    }

    public void SpinAttackDamaged()
    {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck4.position, 3f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Player")
            {
                collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
                ShakeCamera(0.7f);

            }
        }
        availableToAttack = false;
        StartCoroutine(WaitToAttack(1f));

    }

    public void ShakeCamera(float time)
    {
        originalPos = camTransform.localPosition;
        shakeDuration = time;
    }

    //ch?y
    public void Run()
    {
        if (!isAttacking)
        {
            distToPlayer = enemy.transform.position.x - transform.position.x;
            if (Mathf.Abs(distToPlayer) > meleeDist)
            {
                isRunning = true;
                m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
                if (doOnceDecision)
                    StartCoroutine(NextDecision(0.5f));
            }
            else
            {
                StartCoroutine(NextDecision(0.1f));
            }
        }

    }

    //Tr?ng th�i t?nh, kh�ng l�m b?t c? h�nh ??ng g�
    public void Idle()
    {
        anim.SetInteger("Attack", 0);
        isRunning = false;
        //gi? chi?u x c?a nh�n v?t ??ng y�n, c�n chi?u y ?i theo h??ng c?a nh�n v?t
        m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
        if (doOnceDecision)
        {
            anim.SetBool("IsWaiting", true);
            StartCoroutine(NextDecision(1f));
        }
    }

    private void ReviveToState2()
    {
        if (!state2)
        {
            float n = (life / maxLife) * 100;
            if (n <= 20f)
            {
                anim.SetInteger("Attack", 0);
                anim.SetTrigger("Revive");
                life = maxLife;
                state2 = true;
            }

        }
    }




    IEnumerator HitTime()
    {
        isInvincible = true;
        isHitted = true;
        yield return new WaitForSeconds(0.1f);
        isHitted = false;
        isInvincible = false;
    }

    IEnumerator WaitToAttack(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    IEnumerator Dash()
    {
        //isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        NextDecision(1f);
    }

    public void RandomD()
    {
        randomDecision = Random.Range(1, 10);
        randomAttack = Random.Range(1, 7);
    }

    //Quy?t ??nh h�nh ??ng ti?p theo nv s? l�m d? v�o time
    IEnumerator NextDecision(float time)
    {
        /*randomDecision = Random.Range(1, 10);
        randomAttack = Random.Range(1, 7);*/
        doOnceDecision = false;
        yield return new WaitForSeconds(time);
        endDecision = true;
        doOnceDecision = true;

    }

    IEnumerator DestroyEnemy()
    {
        capsule.isTrigger = true;
        anim.SetBool("Death", true);
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
