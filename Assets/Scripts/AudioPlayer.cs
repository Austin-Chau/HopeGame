using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Audio Player links itself to an AudioLibrary and only guarantees one Player exists in a scene at a time.
        if (!AudioLibrary.Initialized)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioLibrary.Initialize(audioSource);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
