using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject go = GameObject.Find("Enemies");

        foreach(Transform child in go.transform)
        {
            HealthBar hb = Instantiate(Resources.Load<HealthBar>("Prefabs/HealthBar"));
            hb.transform.SetParent(canvas.transform);
            hb.Target = child.Find("HealthBarIndicator").gameObject;
        }
    }


}
