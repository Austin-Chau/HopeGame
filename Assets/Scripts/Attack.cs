using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage = 1;

    protected Transform tr;

    public virtual void SetDamage(float val)
    {
        damage = (int)val;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(gameObject);
    }
}
