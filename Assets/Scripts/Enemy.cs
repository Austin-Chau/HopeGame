using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Health of Enemy. Enemy is destroyed when health goes to 0.")]
    private int health = 10;
    
    [SerializeField]
    [Tooltip("Determines if enemy is attacking")]
    private bool isAttacking;

    [SerializeField]
    [Tooltip("True when vulnerable frames are showing.")]
    private bool isVulnerable;

    [SerializeField]
    [Tooltip("Speed of enemy as it patrols")]
    private float speed = 5;

    //remove this when better system is in place.
    public GameObject upHope;

    SpriteRenderer sr;
    Animator anim;
    bool isHit;
    bool facingRight = false;
    float startTime;
    float hitTimePeriod = .1f;
    float moveDir;
   
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (facingRight) moveDir = 1;
        else moveDir = -1;
        transform.Translate(moveDir * new Vector2(speed, 0) * Time.deltaTime);

        if(isHit && Time.time > startTime + hitTimePeriod)
        {
            sr.color = Color.white;
            isHit = false;
        }

        //Yup this is a terrible way to do this
        if(Random.value > .99 && !isAttacking)
        {
            anim.ResetTrigger("Attacked");
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == LayerMask.NameToLayer("PlayerAttacks"))
        {
            Attack attack = other.GetComponent<Attack>();
            if (attack == null) Debug.LogError("Colliding Object does not have Attack script", other);
            RegisterDamage(attack.damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flipDir();
    }


    public void RegisterDamage(int damageVal)
    {
        health -= damageVal;
        if (health <= 0) Destroy(gameObject);

        sr.color = Color.red;
        startTime = Time.time;
        isHit = true;
        anim.SetTrigger("Attacked");

        if (isVulnerable)
        {
            HopeManager.GetInstance().Hope += 15;

            GameObject go = Instantiate(upHope);
            go.transform.position = new Vector3(transform.position.x + (Random.value * 2), transform.position.y + (Random.value * 2),
                transform.position.z);
        }
    }

    private void flipDir()
    {
        facingRight = !facingRight;
        sr.flipX = facingRight;
    }
}
