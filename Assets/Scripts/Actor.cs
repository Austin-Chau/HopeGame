using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actors are any Game Object in Unity that can deal damage and recieve damage.
/// </summary>
public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount of force that occurs when hit")]
    protected int knockbackForce = 300;
    
    float hitTimePeriod = .1f;

    protected Animator anim;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// When damage is taken, actors flash red.
    /// All actors will recieve a knockback effect from the damage recieved
    /// </summary>
    /// <param name="damageVal">Amount of damage recieved</param>
    protected virtual void RegisterDamage(int damageVal)
    {
        anim.SetTrigger("Attacked");
        StartCoroutine(FlashRed());
    }

    protected virtual void Knockback(Vector2 dir)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * knockbackForce);
    }

    protected virtual IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitTimePeriod);
        sr.color = Color.white;
    }
}
