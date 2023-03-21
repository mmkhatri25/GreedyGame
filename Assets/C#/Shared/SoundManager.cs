using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private AudioClip[] _audioClips;
    [SerializeField]private AudioClip backgroundMusic;
    [SerializeField]private AudioClip countdonwnMusic;
    [SerializeField]private AudioSource _audioSource;
    [SerializeField]private AudioSource backgroundAudio;
    [SerializeField]private AudioSource countdownAudio;
    public static SoundManager instance;
    public bool canPlaySound;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        canPlaySound = true;
    }

    public void PlayBackgroundMusic()
    {
        backgroundAudio.clip = backgroundMusic;
        backgroundAudio.Play();
    } 
    public void PlayCountdown()
    {
        countdownAudio.clip = countdonwnMusic;
        countdownAudio.Play();
    }
    public void StopBackgroundMusic()
    {
        backgroundAudio.Stop();
    }
    public void PlayClip(string clipName)
    {
        if (!canPlaySound) return;
        int clipIndex = 0;
        for (int i = 0; i < _audioClips.Length ; i++)
        {
            if(_audioClips[i].name==clipName)
            {
                clipIndex = i;
                break;
            }
        }
        _audioSource.clip = _audioClips[clipIndex];
        _audioSource.Play();
    }  
    public void StopClip(string clipName)
    {
        int clipIndex = 0;
        for (int i = 0; i < _audioClips.Length ; i++)
        {
            if(_audioClips[i].name==clipName)
            {
                clipIndex = i;
                break;
            }
        }
        _audioSource.clip = _audioClips[clipIndex];
        _audioSource.Stop();
    }
}
