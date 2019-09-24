using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uphope : MonoBehaviour
{
    public float FadeDuration = 1f;

    float startTime;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startTime = Time.time;
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime + FadeDuration < Time.time)
        {
            Destroy(gameObject);
        }
        transform.position += new Vector3(0f, .01f, 0f);
        
    }

    IEnumerator Fade()
    {
        while(sr.color.a > 0)
        {
            Color color = sr.color;
            color.a -= .05f;
            sr.color = color;
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }
}
