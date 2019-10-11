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
    bool hasSpriteMesh = false;

    protected Animator anim;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected SpriteMeshContainer smc;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if(sr == null)
        {
            smc = GetComponentInChildren<SpriteMeshContainer>();
            hasSpriteMesh = true;
        }
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
        if (!hasSpriteMesh)
            StartCoroutine(FlashRed());
        else
            smc.FlashRed(hitTimePeriod);
    }

    protected virtual void Knockback(Vector2 dir)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * knockbackForce);
    }

    /// <summary>
    /// Coroutine for flashing the character red. Checks if actor has either a SpriteRenderer
    /// or a SpriteMeshContainer as its child.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitTimePeriod);
        sr.color = Color.white;
    }
}
