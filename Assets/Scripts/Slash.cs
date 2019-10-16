using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Attack
{
    [SerializeField]
    [Tooltip("How long the slash lasts in seconds.")]
    private float lifetime = .5f;

    SpriteRenderer sr;
    Rigidbody2D rb;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Move(Vector2 dir)
    {
        rb.velocity += dir;
        StartCoroutine(MoveAndFade());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.GetComponent<Enemy>() != null)
        {
            if(Mathf.Abs(rb.velocity.x) == 1)
            {
                if(HopeManager.GetInstance().state == HopeState.High)
                {
                    Camera.main.gameObject.GetComponent<CameraMovement>().ZoomCamera();
                }
            }
        }
    }

    public void SetRotation(Vector2 dir)
    {
        if (dir.x < 0 || dir.y < 0) sr.flipX = true;
        if (dir.y != 0) transform.Rotate(Vector3.forward, 90);
    }

    public void SetRotationAndMove(Vector2 dir)
    {
        Vector3 startPos = new Vector3(dir.normalized.x, dir.normalized.y * 1.5f, 0);
        transform.position += startPos;
        SetRotation(dir);
        Move(dir);
    }

    IEnumerator MoveAndFade()
    {
        yield return new WaitForSeconds(lifetime);

        Color temp;
        while(sr.color.a > 0)
        {
            temp = sr.color;
            temp.a -= .1f;
            sr.color = temp;
            yield return new WaitForSeconds(.01f);
        }
        Destroy(gameObject);
    }



}
