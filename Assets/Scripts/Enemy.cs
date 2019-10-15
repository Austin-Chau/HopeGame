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

    [SerializeField]
    [Tooltip("True when vulnerable frames are showing.")]
    private bool isVulnerable = false;

    [SerializeField]
    [Tooltip("Speed of enemy as it patrols")]
    private float speed = 5;

    //remove this when better system is in place.
    public GameObject upHope;

    GameObject Sensors;
    bool facingRight = false;
    bool recentlyFlipped = false;
    float moveDir;

    protected override void Start()
    {
        base.Start();

        Sensors = transform.Find("Sensors").gameObject;
        
    }

    private void Update()
    {
        if (facingRight) moveDir = 1;
        else moveDir = -1;
        transform.Translate(moveDir * new Vector2(speed, 0) * Time.deltaTime);



        //Yup this is a terrible way to do this
        if(Random.value > .99 && !isAttacking)
        {
            anim.ResetTrigger("Attacked");
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("PlayerAttacks"))
        {
            Attack attack = other.GetComponent<Attack>();
            if (attack == null) Debug.LogError("Colliding Object does not have Attack script", other);
            RegisterDamage(attack.damage);

            Knockback(transform.position - other.transform.position);
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
            HopeManager.GetInstance().Hope += 15;

            GameObject go = Instantiate(upHope);
            go.transform.position = new Vector3(transform.position.x + (Random.value * 2), transform.position.y + (Random.value * 2),
                transform.position.z);
        }

        if (health <= 0)
        {
            GameManager.s.RaiseKillCount();
            Destroy(gameObject);
        }
    }

    public void flipDir()
    {
        if (!recentlyFlipped)
        {
            recentlyFlipped = true;
            facingRight = !facingRight;
            flipX(facingRight);
        }
    }

    private void flipX(bool isFlipped)
    {
        
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (isFlipped ? -1 : 1),
            transform.localScale.y,
            transform.localScale.z);

            StartCoroutine(CannotFlip());
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

}
