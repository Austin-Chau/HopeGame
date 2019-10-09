using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundSensor : MonoBehaviour
{
    [Tooltip("Enemy object which this is a sensor to.")]
    public Enemy Enemy;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy.flipDir();
    }
}
