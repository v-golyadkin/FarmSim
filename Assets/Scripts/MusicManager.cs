using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] float timeToSwitch;

    [SerializeField] AudioClip playOnStart;

    private void Start()
    {
        Play(playOnStart, true);
    }

    public void Play(AudioClip musicPlay, bool interrupt = false)
    {
        if (musicPlay == null) { return; }
        
        
        if(interrupt == true)
        {
            audioSource.volume = 0.5f;
            audioSource.clip = musicPlay;
            audioSource.Play();
        }
        else
        {
            swithTo = musicPlay;
            StartCoroutine(SmoothSwitchMusic());
        }
    }

    AudioClip swithTo;
    float volume;

    IEnumerator SmoothSwitchMusic()
    {
        volume = 0.5f;
        while (volume > 0f)
        {
            volume -= Time.deltaTime / timeToSwitch;
            if (volume < 0f) { volume = 0f; }
            audioSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        Play(swithTo, true);
    }
}
