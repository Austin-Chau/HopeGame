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

    //remove this when better system is in place.
    public GameObject upHope;


    SpriteRenderer sr;
    Animator anim;
    bool isHit;
    float startTime;
    float hitTimePeriod = .1f;
   
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Test");
        if (other.layer == LayerMask.NameToLayer("PlayerAttacks"))
        {
            Attack attack = other.GetComponent<Attack>();
            if (attack == null) Debug.LogError("Colliding Object does not have Attack script", other);
            RegisterDamage(attack.damage);
        }
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
}
