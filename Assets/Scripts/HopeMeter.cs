using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HopeMeter : MonoBehaviour
{
    Image image;
    Sprite[] gauges = new Sprite[10];

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        if (image == null) Debug.LogError("Image was not found in gameObject " + gameObject);
        HopeManager.HopeChangeDelegate += UpdateHope;

        for(int i = 0; i < 10; i++)
        {
            gauges[i] = Resources.Load<Sprite>("Sprites/HopeMeter/HopeGauge_" + i);
        }
    }

    public void UpdateHope(int hope)
    {

        image.sprite = gauges[hope];
    }
}
