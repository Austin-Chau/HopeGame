using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TimeStruct
{
    public float totalTime;
    public string timeString;
}

public class Timer : MonoBehaviour
{
    public float secondsPassed { get; private set; }

    float startTime;
    Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
        secondsPassed = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        secondsPassed = Time.time - startTime;
        
        text.text = returnStruct(secondsPassed).timeString;
    }

    public TimeStruct returnStruct()
    {
        return returnStruct(secondsPassed);
    }

    public TimeStruct returnStruct(float time)
    {
        TimeStruct ret = new TimeStruct();

        ret.totalTime = time;

        int seconds = (int)(time % 60);
        string secondsString;
        if (seconds == 0) secondsString = "00";
        else if (seconds < 10) secondsString = "0" + seconds;
        else secondsString = seconds.ToString();

        int minutes = (int)(time / 60);
        string minutesString;
        if (minutes == 0) minutesString = "00";
        else if (minutes < 10) minutesString = "0" + minutes;
        else minutesString = minutes.ToString();

        ret.timeString = minutesString + ":" + secondsString +
            ":" + ((int)((time - (int)time) * 1000)).ToString();

        return ret;
    }
}
