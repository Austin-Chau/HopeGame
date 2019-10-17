using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip bgm;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = .03f;
        bgm = Resources.Load<AudioClip>("Sounds/BackgroundMusic");
        audioSource.PlayOneShot(bgm);
    }
}
