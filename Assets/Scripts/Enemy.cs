using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField]
    [Tooltip("Health of Enemy. Enemy is destroyed when health goes to 0.")]
    private int health = 10;
    
    [SerializeField]
    [Tooltip("Determines if enemy is attacking")]
    private bool isAttacking;
    
    [Tooltip("True when vulnerable frames are showing.")]
    public bool isVulnerable = false;



    //remove this when better system is in place.
    public GameObject upHope;
    public bool facingRight = false;

    GameObject Sensors;
    GameObject Player;
    bool recentlyFlipped = false;
    float moveDir;

    protected override void Start()
    {
        base.Start();

        Player = GameObject.Find("Hero");
        Sensors = transform.Find("Sensors").gameObject;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("PlayerAttacks"))
        {
            Attack attack = other.GetComponent<Attack>();
            if (attack == null) Debug.LogError("Colliding Object does not have Attack script", other);
            RegisterDamage(attack.damage);

            Vector3 playerDirection = transform.position - Player.transform.position;

            Knockback(playerDirection);

            if (playerDirection.x < 0 && !facingRight ||
                playerDirection.x > 0 && facingRight)
            {
                flipDir();
                RaiseHope();
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null) {
            flipDir();
        }
    }

    /// <summary>
    /// Enemies have a certain amount of health and will die when health reaches 0. 
    /// Hope is also restored when enemy is hit in its vulnerable state.
    /// </summary>
    /// <param name="damageVal">Amount of Damage recieved</param>
    protected override void RegisterDamage(int damageVal)
    {
        base.RegisterDamage(damageVal);
        health -= damageVal;

        if (isVulnerable)
        {
            RaiseHope();
        }

        if (health <= 0)
        {
            //GameManager.s.RaiseKillCount();
            Destroy(gameObject);
        }
    }

    public void flipDir()
    {
        if (!recentlyFlipped)
        {
            recentlyFlipped = true;
            facingRight = !facingRight;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (facingRight ? -1 : 1),
                transform.localScale.y,
                transform.localScale.z);

            StartCoroutine(CannotFlip());
        }
    }

    /// <summary>
    /// Sensors are bugging up because of swapping sides so this is my not so elegant solution.
    /// </summary>
    /// <returns></returns>
    IEnumerator CannotFlip()
    {

        yield return new WaitForSeconds(.5f);

        recentlyFlipped = false;
    }

    protected override void Knockback(Vector2 dir)
    {
        rb.velocity = Vector3.zero;
        float dirXSign = dir.x / Mathf.Abs(dir.x);

        Vector2 newDir = new Vector2(dirXSign, .5f);
        
        rb.AddForce(newDir * knockbackForce);
    }

    private void RaiseHope()
    {
        HopeManager.GetInstance().Hope += 15;

        GameObject go = Instantiate(upHope);
        go.transform.position = new Vector3(transform.position.x + (Random.value * 2), transform.position.y + (Random.value * 2),
            transform.position.z);
    }
}
