using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        isCoRunning = false;
        hm = HopeManager.GetInstance();
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
}
