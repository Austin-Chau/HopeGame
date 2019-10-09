using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HopeMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject FillArea;

    Slider slider;
    Image fill;

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = FillArea.GetComponentInChildren<Image>();
        if (fill == null) Debug.LogError("Fill was not found in gameObject " + gameObject);
        HopeManager.HopeChangeDelegate += GetHope;
    }

    public void GetHope(float hope)
    {
        slider.value = hope;
        if (hope < -50)
        {
            fill.color = Color.red;
        }
        else if(hope > 50)
        {
            fill.color = Color.yellow;
        }
        else
        {
            fill.color = Color.cyan;
        }
    }
}
