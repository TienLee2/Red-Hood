using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float life = 10;
    private bool isPlat;
    private bool isObstacle;
    private Transform fallCheck;
    private Transform wallCheck;
    public LayerMask turnLayerMask;
    private Rigidbody2D rb;

    private bool facingRight = true;

    public float speed = 5f;
    public GameObject throwableObject;
    public bool isInvincible = false;
    private bool isHitted = false;
    private bool dead = false;

    public Transform fireStart;

    public bool doOnceDecision;

    [SerializeField] private LevelSystemInterface playerLevel;

    private void Awake()
    {
        //tao gameobject v� t�m tag player
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        //lay script kinh nghiem tu gameObject gan vao playerLevel
        playerLevel = playerGameObject.GetComponentInParent<LevelSystemInterface>();

        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");

        rb = GetComponent<Rigidbody2D>();
        doOnceDecision = true;
    }


    private void FixedUpdate()
    {
        RangeAttack();

        //N?u life b� h?n 0
        if (life <= 0)
        {
            //bool dead s? ???c trigger ?? ch?y 1 frame 
            if (!dead)
            {
                //Th�m kinh nghi?m
                playerLevel.SetLevelSystem(500);
                //bool dead s? ???c ch?nh th�nh true ?? tr�nh l?p l?i
                dead = true;
            }
            transform.GetComponent<Animator>().SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
        }

        //bool check n?u c� m?t ??t
        isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
        //bool check n?u c� t??ng
        isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

        //di chuy?n
        if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            if (isPlat && !isObstacle && !isHitted)
            {
                if (facingRight)
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
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
        // ??i m?t m� enemy ?ang ??i di?n
        facingRight = !facingRight;
        // ??i x sang -1 = ??i m?t
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    //function t�nh damage b? d�nh ph?i
    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            transform.GetComponent<Animator>().SetBool("Hit", true);
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
            GameObject throwableProj = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
            throwableProj.GetComponent<ThrowableProjectile>().owner = gameObject;
            Vector2 direction = new Vector2(transform.localScale.x, 0f);
            throwableProj.GetComponent<ThrowableProjectile>().direction = direction;
            StartCoroutine(NextDecision(0.5f));
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && life > 0)
        {
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
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
        isHitted = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        isHitted = false;
        isInvincible = false;
    }

    //N?u ch?t th� h?y game object
    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
