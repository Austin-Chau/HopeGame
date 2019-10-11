using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;
using System;

/// <summary>
/// Script to track all of the sprite meshes in a single body so that we 
/// can work with them together.
/// </summary>
public class SpriteMeshContainer : MonoBehaviour
{
    SpriteMeshInstance[] SpriteMeshes;

    private void Start()
    {
        SpriteMeshes = GetComponentsInChildren<SpriteMeshInstance>();
    }

    public void FlashRed(float hitTimePeriod)
    {
        StartCoroutine(FlashMeshesRed(hitTimePeriod));
    }

    private IEnumerator FlashMeshesRed(float hitTimePeriod)
    {
        foreach(SpriteMeshInstance instance in SpriteMeshes)
        {
            instance.color = Color.red;
        }
        yield return new WaitForSeconds(hitTimePeriod);
        foreach (SpriteMeshInstance instance in SpriteMeshes)
        {
            instance.color = Color.white;
        }
    }
}
