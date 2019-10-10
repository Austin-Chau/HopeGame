using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s;

    public GameObject WinText;

    [Header("Hope Mechanics")]

    [SerializeField]
    [Tooltip("Restores 1 point every n seconds when hope is < 0")]
    private float hopeRegenSpeed = 1.0f;

    [SerializeField]
    [Tooltip("Subtracts 1 point every n seconds when hope is > 0")]
    private float hopeDecaySpeed = 1.0f;


    HopeManager hm;
    Coroutine co;
    bool isCoRunning;

    //Win condition right now.
    int killCount;
    int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        if (s != null) Destroy(gameObject);
        else s = this;

        isCoRunning = false;
        hm = HopeManager.GetInstance();

        CountEnemies();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoRunning)
        {
            if (hm.Hope > 0)
            {
                StartCoroutine(AdjustHope(hopeDecaySpeed, -1));
            }
            if (hm.Hope < 0)
            {
                StartCoroutine(AdjustHope(hopeRegenSpeed, 1));
            }
        }
    }

    IEnumerator AdjustHope(float timeInterval, int hopeMod)
    {
        isCoRunning = true;
        while (hm.Hope != 0)
        {
            hm.Hope += hopeMod;
            yield return new WaitForSeconds(timeInterval);
        }
        isCoRunning = false;
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
}
