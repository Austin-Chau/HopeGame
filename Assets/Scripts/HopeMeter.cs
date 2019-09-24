using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HopeMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject FillArea;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        HopeManager.HopeChangeDelegate += GetHope;
    }

    public void GetHope(float hope)
    {
        slider.value = hope;
    }
}
