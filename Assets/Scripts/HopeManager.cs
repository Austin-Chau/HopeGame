using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopeManager
{
    public delegate void OnHopeChangeDelegate(float hope);
    public static OnHopeChangeDelegate HopeChangeDelegate;

    public const int MAX_HOPE = 100;

    #region Singleton Setup

    private static HopeManager h;

    public static HopeManager GetInstance()
    {
        if (h == null) h = new HopeManager();
        return h;
    }

    #endregion

    private float hope = 100;
    public float Hope
    {
        get
        {
            return hope;
        }
        set
        {
            if (value >= 0 && value <= 100)
            {
                HopeChangeDelegate(value);
                hope = value;
            }
        }
    }
}
