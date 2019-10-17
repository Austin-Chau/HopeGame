using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class AudioLibrary
{
    static bool initialized = false;
    static AudioSource audioSource;
    static Dictionary<AudioName, AudioClip> audioClips = new Dictionary<AudioName, AudioClip>();

    public static bool Initialized { get { return initialized; } }

    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;

        AddClips();
    }

    public static void Play(AudioName name)
    {
        audioSource.PlayOneShot(audioClips[name]);
    }
    public static void Stop()
    {
        audioSource.Stop();
    }
    public static bool Playing() { return audioSource.isPlaying; }


    public static AudioSource CreateNewAudioPlayer(AudioName name)
    {
        AudioSource newSource = audioSource.gameObject.AddComponent<AudioSource>();
        newSource.clip = audioClips[name];
        return newSource;
    }

    private static void AddClip(AudioName audioName, string fileName)
    {
        audioClips.Add(audioName, Resources.Load<AudioClip>("Sounds/" + fileName));
    }
}
