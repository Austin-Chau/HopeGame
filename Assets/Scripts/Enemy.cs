using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Health of Enemy. Enemy is destroyed when health goes to 0.")]
    private int health = 10;

    SpriteRenderer sr;
    bool isHit;
    float startTime;
    float hitTimePeriod = .1f;

    public GameObject camera;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(isHit && Time.time > startTime + hitTimePeriod)
        {
            sr.color = Color.white;
            isHit = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;

        if (other.layer == LayerMask.NameToLayer("PlayerAttacks"))
        {
            Attack bullet = other.GetComponent<Attack>();
            if (bullet == null) Debug.LogError("Colliding Bullet does not have Bullet script", other);
            RegisterDamage(bullet.damage);
        }
    }


    public void RegisterDamage(int damageVal)
    {
        health -= damageVal;
        if (health <= 0) Destroy(gameObject);

        sr.color = Color.red;
        startTime = Time.time;
        isHit = true;
        if (damageVal >= 4) camera.GetComponent<ShakeBehaviour>().TriggerShake();
        
    }
}
