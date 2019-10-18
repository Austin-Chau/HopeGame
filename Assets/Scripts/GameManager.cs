using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s;

    public GameObject NotificationWindow;
    public GameObject WinText;
    public GameObject PauseMenu;

    [Header("Hope Mechanics")]

    [SerializeField]
    [Tooltip("Restores 1 point every n seconds when hope is < 0")]
    private int hopeRegenSpeed = 1;

    [SerializeField]
    [Tooltip("Subtracts 1 point every n seconds when hope is > 0")]
    private int hopeDecaySpeed = 1;

    private const int LOW_HOPE_THRESHOLD = -66;
    private const int HIGH_HOPE_THRESHOLD = 66;

    HopeManager hm;
    Coroutine co;
    bool coroutineRunning = false;
    //Win condition right now.
    int killCount;
    int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        if (s != null) Destroy(gameObject);
        else s = this;
        
        hm = HopeManager.GetInstance();

        CountEnemies();

        StartCoroutine(AdjustHope());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseMenu.SetActive(true); 
        }
    }


    IEnumerator AdjustHope()
    {
        int mod = 0;

        while (true)
        {
            if(hm.Hope > 5 && hm.Hope <= 9)
            {
                mod = -hopeDecaySpeed;
            }
            if(hm.Hope < 5 && hm.Hope >= 0)
            {
                mod = hopeRegenSpeed;
            }
            hm.Hope += mod;
            mod = 0;
            yield return new WaitForSeconds(10.0f);
        }
    }

    /// <summary>
    /// Checks every enemy in a GameObject "Enemies" and stores them in a list.
    /// </summary>
    void CountEnemies()
    {
        GameObject go = GameObject.Find("Enemies");

        foreach(Transform child in go.transform)
        {
            enemyCount++;
        }
    }

    /// <summary>
    /// Jank victory condition wahooooo
    /// </summary>
    public void RaiseKillCount()
    {
        killCount++;

        if(killCount >= enemyCount)
        {
            Time.timeScale = 0;
            WinText.SetActive(true);
        }
    }

    public void PopUpRangedNotification()
    {
        if(!coroutineRunning) StartCoroutine(NotificationCoroutine());
    }

    IEnumerator NotificationCoroutine()
    {
        coroutineRunning = true;
        NotificationWindow.SetActive(true);
        yield return new WaitForSeconds(3f);
        NotificationWindow.SetActive(false);
        coroutineRunning = false;
    }
}
