using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float life = 10;
    private bool isPlat;
    private bool isObstacle;
    private Transform fallCheck;
    private Transform wallCheck;
    public LayerMask turnLayerMask;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;
    public bool availableToAttack = false;
    public float speed = 5f;
    public GameObject throwableObject;
    public bool isInvincible = false;
    private bool isHitted = false;
    private bool dead = false;
    public Transform fireStart;
    private Transform attackCheck;
    public bool doOnceDecision;

    [SerializeField] private LevelSystemInterface playerLevel;

    private void Awake()
    {
        //tao gameobject và tìm tag player
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        //lay script kinh nghiem tu gameObject gan vao playerLevel
        playerLevel = playerGameObject.GetComponentInParent<LevelSystemInterface>();

        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        doOnceDecision = true;
    }
    void Update()
    {
        Animator();
    }

    private void FixedUpdate()
    {

        RangeAttack();

        //N?u life bé h?n 0
        if (life <= 0)
        {
            //bool dead s? ???c trigger ?? ch?y 1 frame 
            if (!dead)
            {
                //Thêm kinh nghi?m
                playerLevel.SetLevelSystem(500);
                //bool dead s? ???c ch?nh thành true ?? tránh l?p l?i
                dead = true;
            }
            transform.GetComponent<Animator>().SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
        }

        //bool check n?u có m?t ??t
        isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
        //bool check n?u có t??ng
        isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

        //di chuy?n
        if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            if (isPlat && !isObstacle && !isHitted)
            {
                if (facingRight)
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    anim.SetBool("Run", true);
                }
                else
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    anim.SetBool("Run", true);
                }
            }
            else
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        // ??i m?t mà enemy ?ang ??i di?n
        facingRight = !facingRight;
        anim.SetBool("Run", true);
        // ??i x sang -1 = ??i m?t
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    //function tính damage b? dính ph?i
    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            transform.GetComponent<Animator>().SetBool("Hit", true);
            AudioManager.instance.PlaySFX("Hit");
            life -= damage;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * 500f, 100f));
            StartCoroutine(HitTime());
        }
    }
    public void RangeAttack()
    {
        if (doOnceDecision)
        {
            
            anim.SetTrigger("Attack");
            GameObject throwableProj = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, 0.5f), Quaternion.identity) as GameObject;
            throwableProj.GetComponent<ThrowableProjectile>().owner = gameObject;
            Vector2 direction = new Vector2(transform.localScale.x, 0f);
            throwableProj.GetComponent<ThrowableProjectile>().direction = direction;
            StartCoroutine(NextDecision(1f));
        }
    }
    private void Animator()
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

    

    IEnumerator NextDecision(float time)
    {
        doOnceDecision = false;
        yield return new WaitForSeconds(time);
        doOnceDecision = true;
    }

    //Neu vua bi dinh don thi doi 1 khoang thoi gian moi nhan dame tiep
    IEnumerator HitTime()
    {
        anim.SetTrigger("TakeHit");
        isHitted = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        isHitted = false;
        isInvincible = false;
    }

    //N?u ch?t thì h?y game object
    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);

        anim.SetBool("Death", true);
        AudioManager.instance.PlaySFX("MushroomDeath");
        capsule.direction = CapsuleDirection2D.Horizontal;
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
