using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HopeMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject FillArea;

    Slider slider;
    ColorBlock cb;

    private void Start()
    {
        slider = GetComponent<Slider>();
        cb = slider.colors;
        HopeManager.HopeChangeDelegate += GetHope;
    }

    public void GetHope(float hope)
    {
        slider.value = hope;
        if (hope < -50)
        {
            cb.normalColor = Color.red;
        }
        else if(hope > 50)
        {
            cb.normalColor = Color.yellow;
        }
        else
        {
            cb.normalColor = Color.cyan;
        }

        slider.colors = cb;
    }
}
