using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage { get; private set; } = 1;

    protected Transform tr;

    public virtual void SetDamage(float val)
    {
        damage = (int)val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy enem = collision.gameObject.GetComponent<Enemy>();
            enem.RegisterDamage(damage);
        }
    }
}
