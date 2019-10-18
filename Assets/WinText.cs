using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    Text YourTime;
    Text BestTime;
    const string bestTime = "BestTime";

    // Start is called before the first frame update
    void OnEnable()
    {
        if (!PlayerPrefs.HasKey(bestTime)) PlayerPrefs.SetFloat(bestTime, float.MaxValue);
        BestTime = transform.Find("Best Time").GetComponent<Text>();
        YourTime = transform.Find("Your Time").GetComponent<Text>();

        TimeStruct bestTimeStruct = new TimeStruct(PlayerPrefs.GetFloat(bestTime));
        BestTime.text = bestTimeStruct.timeString;
    }

    public void UpdateYourTime(TimeStruct time)
    {
        YourTime.text = time.timeString;
        if (PlayerPrefs.GetFloat(bestTime) > time.totalTime) PlayerPrefs.SetFloat(bestTime, time.totalTime);
    }
}
