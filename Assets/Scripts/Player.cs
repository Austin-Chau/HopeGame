﻿using System.Collections;
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
    [Tooltip("Time between press and release for high jump.")]
    private float jumpTimeThreshold = .5f;

    [SerializeField]
    [Tooltip("Amount of force jump is multiplied by.")]
    private float longJumpMultiplier = 1.5f;

    [SerializeField]
    [Tooltip("Amount that speed is multiplied by when at low hope")]
    private float lowHopeSpeedMultiplier = 1.2f;
    #endregion

    #region Private Variables

    float horizontal = 0f;
    float vertical = 0f;
    bool facingRight = true;
    Transform tr;
    BoxCollider2D bc2d;

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
                jumpTime = Time.time;

        }

        if (Input.GetButtonUp("Jump"))
        {
            if (IsGrounded())
            {
                if (Time.time - jumpTime <= jumpTimeThreshold)
                    rb.AddForce(Vector2.up * jumpForce);
                else
                    rb.AddForce(Vector2.up * jumpForce * longJumpMultiplier);
                jumpTime = float.MaxValue;
            }
        }

        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Slash"))
        {
            if (Input.GetButtonDown("Slash"))
            {
                Slash(1f);
            }


            if (Input.GetButtonDown("Slash2"))
            {
                Slash(slashSpeed);
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

        if (speed == 1)
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

        Slash slash = go.GetComponent<Slash>();
        

        switch (HopeManager.GetInstance().state)
        {
            case HopeState.Low:
                slash.damage = 0;
                break;
            case HopeState.Normal:
                slash.damage = 5;
                break;
            case HopeState.High:
                slash.damage = 15;
                break;

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
            //Does flat damage for now.
            RegisterDamage(5);
            HopeManager.GetInstance().Hope += -5;
            Knockback(transform.position - other.transform.position);
        }
    }

    protected override void Knockback(Vector2 dir){
        rb.velocity = Vector3.zero;

        dir.y = dir.normalized.y;

        rb.AddForce(dir * knockbackForce);
    }

    private void flipX(bool isFlipped)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (isFlipped? -1 : 1),
        transform.localScale.y,
        transform.localScale.z);
    }

    protected override IEnumerator FlashRed()
    {
        yield return null;
    }
}
