using UnityEngine;
using System.Collections;

public class FlyingEye : MonoBehaviour
{
    public int experienceGet;
    public float dmgValue;
    public float life = 10;
    private bool isPlat;
    private bool isObstacle;
    private Transform fallCheck;
    private Transform wallCheck;
    public LayerMask turnLayerMask;
    private Rigidbody2D rb;

    private bool facingRight = true;

    public float speed = 5f;

    public bool isInvincible = false;
    private bool isHitted = false;
    private bool dead = false;

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
    }


    private void FixedUpdate()
    {

        //N?u life bé h?n 0
        if (life <= 0)
        {
            //bool dead s? ???c trigger ?? ch?y 1 frame 
            if (!dead)
            {
                AudioManager.instance.PlaySFX("FlyingEyeDeath");
                //Thêm kinh nghi?m
                playerLevel.SetLevelSystem(experienceGet);
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
        // ??i m?t mà enemy ?ang ??i di?n
        facingRight = !facingRight;
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && life > 0)
        {
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
        }
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

    //N?u ch?t thì h?y game object
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
