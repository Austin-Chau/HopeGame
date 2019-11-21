using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 NewPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = NewPos;
    }
}
