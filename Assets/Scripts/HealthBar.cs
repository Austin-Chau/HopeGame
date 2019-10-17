using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Enemy enemy;
    private Scrollbar scrollBar;

    private GameObject target;
    public GameObject Target {
        private get {
            return target;
        }
        set {
            targetSet = true;
            target = value;
            enemy = target.transform.parent.GetComponent<Enemy>();
        }
    }
    bool targetSet = false;

    private void Start()
    {
        scrollBar = GetComponent<Scrollbar>();
    }

    private void Update()
    {
        if (targetSet)
        {
            if (target != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
                scrollBar.size = enemy.GetHealthPercentage();
            }
            else Destroy(gameObject);
            //Code not great, better to use delegate to kill health bar
        }
    }

}
