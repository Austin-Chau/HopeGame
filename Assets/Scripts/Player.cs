using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
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

    [SerializeField]
    [Tooltip("")]
    private float fallMultiplier = 2.5f;

    [SerializeField]
    private float lowJumpMultiplier = 2f;

    [SerializeField]
    [Tooltip("Amount that speed is multiplied by when at low hope")]
    private float lowHopeSpeedMultiplier = 1.2f;

    [SerializeField]
    [Tooltip("Amount of time you can range attack without consequence.")]
    private float rangedAttackCooldown = 3f;

    [SerializeField]
    private bool canMove = true;

    [SerializeField]
    private float damageCooldown = 2f;
    #endregion

    #region Private Variables

    float horizontal = 0f;
    float vertical = 0f;
    bool facingRight = true;
    bool rangedAttackedRecently = false;
    Transform tr;
    BoxCollider2D bc2d;

    float damagedTime;
    float rangedAttackTime;
    float jumpTime;

    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        tr = transform;
        bc2d = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
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



            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {

                    rb.AddForce(Vector2.up * jumpForce);
                    AudioLibrary.Play(AudioName.LowJump);
                }
            }

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

                if (rangedAttackTime + rangedAttackCooldown < Time.time &&
                rangedAttackedRecently)
                rangedAttackedRecently = false;

            if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Slash"))
            {
                if (Input.GetButtonDown("Slash"))
                {
                    Slash(1f);
                    rangedAttackedRecently = false;
                }


                if (Input.GetButtonDown("Slash2"))
                {
                    if (rangedAttackedRecently)
                    {
                        GameManager.s.PopUpRangedNotification();
                        AudioLibrary.Play(AudioName.CrowdBoo);
                        HopeManager.GetInstance().Hope -= 1;
                    }
                    rangedAttackedRecently = true;
                    rangedAttackTime = Time.time;
                    Slash(slashSpeed);
                }
            }
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
        Slash slash = go.GetComponent<Slash>();

        if (speed == 1)
        {
            slash.slashType = SlashType.Melee;
            go.transform.parent = tr;
        }
        else
            slash.slashType = SlashType.Ranged;
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

        if (slash.slashType == SlashType.Ranged)
        {
            slash.damage = 1;
            AudioLibrary.Play(AudioName.PlayerSlash);
        }
        else
        {
            switch (HopeManager.GetInstance().state)
            {
                case HopeState.Low:
                    slash.damage = 0;
                    AudioLibrary.Play(AudioName.PlayerSlash);
                    break;
                case HopeState.Normal:
                    slash.damage = 5;
                    AudioLibrary.Play(AudioName.PlayerSlash);
                    break;
                case HopeState.High:
                    slash.damage = 20;
                    AudioLibrary.Play(AudioName.PlayerHighSlash);
                    break;

            }
        }

        slash.SetRotationAndMove(dir * speed);

    }

    /// <summary>
    /// Moves player horizontally
    /// </summary>
    void Move()
    {
        Vector3 newPos = new Vector3(horizontal, 0);

        float finalSpeed = speed;
        if (HopeManager.GetInstance().state == HopeState.Low)
            finalSpeed *= lowHopeSpeedMultiplier;
        if(canMove)
            transform.position += newPos * finalSpeed * Time.deltaTime;

        if (facingRight && horizontal < 0)
        {
            facingRight = false;
            flipX(true);
        }
        else if (horizontal > 0)
        {
            facingRight = true;
            flipX(false);
        }

        anim.SetBool("isMoving", true);
    }


    /// <summary>
    /// Checks if player is grounded or not.
    /// </summary>
    /// <returns>Returns true if grounded, false if not.</returns>
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            return true;
        }
        else return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("Enemy"))
        {
            RecieveDamage(other);
            damagedTime = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (damagedTime + damageCooldown < Time.time)
        {
            if (other.layer == LayerMask.NameToLayer("Enemy"))
            {
                RecieveDamage(other);
                damagedTime = Time.time;
            }
        }
    }

    private void RecieveDamage(GameObject other)
    {
        AnimatorStateInfo asi = other.GetComponent<Animator>().GetNextAnimatorStateInfo(0);
        if (asi.IsName("Attack") ||
            asi.IsName("Idle"))
        {
            RegisterDamage(5);
            HopeManager.GetInstance().Hope += -2;
            AudioLibrary.Play(AudioName.CrowdGasp);
        }
        else
        {
            RegisterDamage(5);
            HopeManager.GetInstance().Hope += -1;
        }
        Knockback(transform.position - other.transform.position);
    }

    //protected override void Knockback(Vector2 dir)
    //{
    //    rb.velocity = Vector3.zero;

    //    dir.y = dir.normalized.y;

    //    rb.AddForce(dir * knockbackForce);
    //}

    private void flipX(bool isFlipped)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (isFlipped ? -1 : 1),
        transform.localScale.y,
        transform.localScale.z);
    }

    protected override IEnumerator FlashRed()
    {
        yield return null;
    }

    public void PlayStep()
    {
        if(IsGrounded())
            AudioLibrary.Play(AudioName.Footstep);
    }
}
