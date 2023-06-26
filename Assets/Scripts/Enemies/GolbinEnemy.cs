using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GolbinEnemy : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    //Quy?t ??nh cha
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public float life = 10;

    private bool facingRight = true;

    public float speed = 5f;

    public bool isInvincible = false;
    private bool isHitted = false;


    public GameObject enemy;
    private float distToPlayer;
    private float distToPlayerY;
    public float meleeDist = 1.5f;
    public float rangeDist = 5f;
    private bool canAttack = true;
    private Transform attackCheck;
    public float dmgValue = 4;

    private float randomDecision = 0;
    private bool doOnceDecision = true;
    private bool endDecision = false;
    private Animator anim;
    private bool dead = false;
    private bool run = false;
    public bool availableToAttack;

    [SerializeField] private LevelSystemInterface playerLevel;

    void Awake()
    {
        enemy = null;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        //lay script kinh nghiem tu gameObject gan vao playerLevel
        playerLevel = playerGameObject.GetComponentInParent<LevelSystemInterface>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        attackCheck = transform.Find("AttackCheck").transform;
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        AnimatorController();
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 5f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Player")
            {
                enemy = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //N?u life bé h?n 0 thì ch?t
        if (life <= 0)
        {
            if (!dead)
            {
                //Thêm kinh nghi?m
                playerLevel.SetLevelSystem(500);
                //bool dead s? ???c ch?nh thành true ?? tránh l?p l?i
                dead = true;
            }

            StartCoroutine(DestroyEnemy());
        }
        //N?u enemy không có
        if (enemy != null)
        {
            //N?u ch?a b? ?ánh
            if (!isHitted)
            {
                //tìm kho?ng cách gi?a k? ??ch và b?n thân
                distToPlayer = enemy.transform.position.x - transform.position.x;
                distToPlayerY = enemy.transform.position.y - transform.position.y;

                //Abs tr? v? s? nguyên d??ng, và n?u kho?ng cách bé h?n 0.25 thì nv s? ??ng yên
                if (Mathf.Abs(distToPlayer) < 0.25f)
                {
                    Idle();
                }
                //N?u kho?ng cách l?n h?n 25 và trong t?m melee thì s? t?n công t?m g?n
                else if (Mathf.Abs(distToPlayer) > 0.25f && Mathf.Abs(distToPlayer) < meleeDist && Mathf.Abs(distToPlayerY) < 2f)
                {
                    
                    if (randomDecision < 0.4f)
                    {
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                            Flip();
                    }
                    else
                    {
                        Idle();
                    }

                    if (canAttack)
                    {
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                            Flip();
                        Idle();
                        MeleeAttack();
                    }
                }
                else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
                {
                    if(randomDecision < 0.4f)
                    {
                        Run();
                        m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
                    }
                    else
                    {
                        Run();
                        m_Rigidbody2D.velocity = new Vector2(-distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
                    }
                    
                }
                else
                {
                    //ng?u nhiên quy?t ??nh hành ??ng
                    if (!endDecision)
                    {
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                            Flip();

                        if (randomDecision < 0.4f)
                            Run();
                        else
                            Idle();
                    }
                    else
                    {
                        endDecision = false;
                    }
                }
            }
            //N?u b? ?ánh
            else if (isHitted)
            {
                m_Rigidbody2D.velocity = new Vector2(-distToPlayer / Mathf.Abs(distToPlayer) * speed * 2f, m_Rigidbody2D.velocity.y);
            }
        }

        if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
        {
            //xoay nhân v?t
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void AnimatorController()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7
        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9)
        {
            if (availableToAttack)
            {
                Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
                for (int i = 0; i < collidersEnemies.Length; i++)
                {
                    if (collidersEnemies[i].gameObject.tag == "Player")
                    {
                        collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
                    }
                }
            }
        }
    }

    void Flip()
    {
        //xoay nhân v?t nên s? ??i bool 
        facingRight = !facingRight;

        //??i tr?c x ng??c v?i h? t?a ??
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    //function tính damage b? dính ph?i
    public void ApplyDamage(float damage)
    {
        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
            Flip();
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            anim.SetTrigger("TakeHit");
            AudioManager.instance.PlaySFX("Hit");
            life -= damage;
            m_Rigidbody2D.velocity = new Vector2(0, 0);
            m_Rigidbody2D.AddForce(new Vector2(direction * 300f, 100f));
            StartCoroutine(HitTime());
        }
    }

    //function ?ánh t?m g?n
    public void MeleeAttack()
    {
        m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
        //Set animator ?ánh
        anim.SetTrigger("Attack");
        AudioManager.instance.PlaySFX("Attack");
        //Ki?m ta collider k? d?ch ? g?n
        availableToAttack = true;
        //sau khi t?n công ??i 0.5 giây ?? ti?p t?c t?n công
        StartCoroutine(WaitToAttack(5f));
    }

    //ch?y
    public void Run()
    {
        run = true;
        //Set bool ??i trong animator thành false ?? có th? ch?y
        anim.SetBool("Run", true);
        //di chuy?n nhân v?t theo vector 2
        m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
        if (doOnceDecision)
            StartCoroutine(NextDecision(0.5f));
    }

    //Tr?ng thái t?nh, không làm b?t c? hành ??ng gì
    public void Idle()
    {
        anim.SetBool("Run", false);
        //gi? chi?u x c?a nhân v?t ??ng yên, còn chi?u y ?i theo h??ng c?a nhân v?t
        m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
        if (doOnceDecision)
        {
            anim.SetBool("IsWaiting", true);
            StartCoroutine(NextDecision(1f));
        }
    }

    public void EndDecision()
    {
        randomDecision = Random.Range(0.0f, 1.0f);
        endDecision = true;
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
        availableToAttack = false;
        canAttack = true;
    }


    //Quy?t ??nh hành ??ng ti?p theo nv s? làm d? vào time
    IEnumerator NextDecision(float time)
    {
        doOnceDecision = false;
        yield return new WaitForSeconds(time);
        EndDecision();
        doOnceDecision = true;
    }

    IEnumerator DestroyEnemy()
    {
        canAttack = false;
        m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);

        transform.GetComponent<Animator>().SetBool("Dead", true);
        AudioManager.instance.PlaySFX("GoblinDeath");
        yield return new WaitForSeconds(0.25f);
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
