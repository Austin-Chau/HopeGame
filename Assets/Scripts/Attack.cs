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
        if(HopeManager.GetInstance().state == HopeState.High)
        {
            Camera.main.gameObject.GetComponent<CameraMovement>().ZoomCamera();
        }
        Destroy(gameObject);
    }
}
