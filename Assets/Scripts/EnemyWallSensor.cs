using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallSensor : MonoBehaviour
{
    [Tooltip("Enemy object which this is a sensor to.")]
    public Enemy Enemy;

    public bool enteredWall;

    private void Start()
    {
        enteredWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredWall)
        {
            Enemy.flipDir();
            enteredWall = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        enteredWall = false;
    }

}
