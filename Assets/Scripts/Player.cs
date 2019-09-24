using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialized Variables

    [Header("Hero Stats")]

    [SerializeField]
    [Tooltip("Hero Speed")]
    private float speed;

    [SerializeField]
    [Tooltip("Amount of force added when hero jumps")]
    private float jumpForce;

    [Header("Bullet Info")]

    [SerializeField]
    [Tooltip("Bullet Prefab to instantiate and fire.")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("Speed of the bullet.")]
    private int bulletSpeed = 20;

    [SerializeField]
    [Tooltip("Damage of bullets")]
    private int bulletDamage = 5;


    #endregion

    #region Private Variables

    bool isGrounded = true;
    float horizontal = 0f;
    float vertical = 0f;
    bool facingRight = true;
    SpriteRenderer sr;
    Transform tr;
    Animator anim;
    Attack slash;
    Rigidbody2D rb;
    BoxCollider2D bc2d;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        slash = transform.GetComponentInChildren<Attack>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire")){
            Fire();
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0)
        {
            Move();
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (Physics2D.Raycast(transform.position, Vector2.down)) isGrounded = true;
        else isGrounded = false;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        //TODO: Check if slash animation is occuring before calling Slash
        if (Input.GetKeyDown(KeyCode.R))
        {
            Slash();
        }
    }

    /// <summary>
    /// Fires bullet from player to mouse position.
    /// </summary>
    void Fire()
    {
        float hope = UseHope();
        
        Vector2 fireDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - tr.position;
        GameObject go = Instantiate(bullet);

        go.transform.position = tr.position;
        Rigidbody2D bRB = go.GetComponent<Rigidbody2D>();

        if (bRB == null) Debug.LogError("Bullet does not have Rigidbody", go);

        bRB.velocity = fireDirection.normalized * bulletSpeed;

        //Sets the damage of the bullet and reduces hope;
        go.GetComponent<Bullet>().SetDamage(bulletDamage * (hope / HopeManager.MAX_HOPE));
    }

    void Move()
    {
        Vector3 newPos = new Vector3(horizontal, 0, 0);
        transform.position +=  newPos * speed * Time.deltaTime;
        
        if(facingRight && horizontal < 0)
        {
            facingRight = false;
            sr.flipX = true;
        }
        else if(horizontal > 0)
        {
            sr.flipX = false;
            facingRight = true;
        }

        anim.SetBool("isMoving", true);
    }

    void Slash()
    {
        anim.SetTrigger("Slash");
        slash.SetDamage(bulletDamage * (UseHope() / HopeManager.MAX_HOPE));
    }

    private float UseHope()
    {
        float hope = HopeManager.GetInstance().Hope;

        HopeManager.GetInstance().Hope -= 5f;

        return hope;
    }
}
