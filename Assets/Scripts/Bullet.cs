using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Attack
{
    public override void SetDamage(float val)
    {
        base.SetDamage(val);
        tr = transform;
        Vector2 scale = tr.localScale;
        tr = transform;
        scale = new Vector2(val, val);
        tr.localScale = scale;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
