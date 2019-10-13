using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HopeState { Low, Normal, High };

/// <summary>
/// Class that manages the hope value. Everytime hope value changes, a delegate
/// fires any functions attached to it, so that they can do whatever they need to with
/// that information.
/// </summary>
public class HopeManager
{
    public delegate void OnHopeChangeDelegate(float hope);
    public static OnHopeChangeDelegate HopeChangeDelegate;

    public const int MAX_HOPE = 100;

    #region Singleton Setup

    private static HopeManager h;
    private static Material material;

    public static HopeManager GetInstance()
    {
        material = Resources.Load<Material>("Greyscale");

        if (h == null) h = new HopeManager();
        return h;
    }

    #endregion

    public HopeState state { get; private set; } = HopeState.Normal;
    
    private float hope = 100;
    public float Hope
    {
        get
        {
            return hope;
        }
        set
        {
            if (value >= -100 && value <= 100)
            {
                HopeChangeDelegate(value);
                hope = value;
                
                material.SetColor("_Color", new Color((value + 100f) / 200f, 1, 1, 1 ));

                if (value >= 66) state = HopeState.High;
                else if (value <= -66) state = HopeState.Low;
                else state = HopeState.Normal;
            }
        }
    }


}
