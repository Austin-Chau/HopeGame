using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialized Variables

    [Header("Hero Stats")]

    [SerializeField]
    [Tooltip("Hero Speed")]
    private float speed = 5;

    [SerializeField]
    [Tooltip("Amount of force added when hero jumps")]
    private float jumpForce = 300;

    [SerializeField]
    [Tooltip("Speed of the bullet.")]
    private int slashSpeed = 2;


    #endregion

    #region Private Variables

    bool isGrounded = true;
    float horizontal = 0f;
    float vertical = 0f;
    bool facingRight = true;
    SpriteRenderer sr;
    Transform tr;
    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D bc2d;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetButtonDown("Slash"))
        {
            Slash(1f);
        }


        if (Input.GetButtonDown("Slash2"))
        {
            Slash(slashSpeed);
        }
    }

    /// <summary>
    /// Fires a Slash from player based on speed and direction of movement.
    /// </summary>
    void Slash(float speed)
    {
        anim.SetTrigger("Slash");
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Slash"));
        Vector2 dir = Vector2.right;
        
        if(speed == 1)
            go.transform.parent = tr;
        go.transform.position = tr.position;

        if (horizontal == 0 && vertical == 0)
        {
            if (facingRight)
                dir = Vector2.right;
            else
                dir = Vector2.left;
        }
        else if (vertical != 0)
        {
            dir = new Vector2(0,
                vertical / (vertical == 0 ? 1 : Mathf.Abs(vertical)));
        }
        else
        {
            dir = new Vector2(
                horizontal / (horizontal == 0 ? 1 : Mathf.Abs(horizontal)),
                0);
        }

        Debug.Log(dir);

        go.GetComponent<Slash>().SetRotationAndMove(dir * speed);

    }

    /// <summary>
    /// Moves player horizontally
    /// </summary>
    void Move()
    {
        Vector3 newPos = new Vector3(horizontal, 0);
        transform.position += newPos * speed * Time.deltaTime;
        
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
   

    /// <summary>
    /// Expends hope and returns the value to use for damage
    /// </summary>
    /// <returns>Amount of hope expended</returns>
    private float UseHope()
    {
        float hope = HopeManager.GetInstance().Hope;

        HopeManager.GetInstance().Hope -= 5f;

        return hope;
    }
}
