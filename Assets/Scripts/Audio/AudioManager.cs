using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    public AudioClip victoryMusic;
    [HideInInspector]
    public AudioSource audioSource;
    private void Awake()
    {
        if (audioManager == null)
            audioManager = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic(AudioClip bGMClip)
    {
        if(bGMClip == audioSource.clip)
        {
            return;
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = bGMClip;
            audioSource.Play();
        }
    }
    public void PlayVictoryMusic()
    {
        audioSource.Stop();
        audioSource.clip = victoryMusic;
        audioSource.Play();
    }
}
