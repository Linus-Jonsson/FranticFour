using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] AudioMixerSnapshot main;
    [SerializeField] AudioMixerSnapshot musicFadeout;
    [SerializeField] float fadeOutTime = 2.0f;

    AudioSource musicPlayer;

    void Start()
    {
        musicPlayer = transform.Find("MusicPlayer").GetComponent<AudioSource>();
    }

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("master", Mathf.Log10(value) * 20);
    }
    
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("music", Mathf.Log10(value) * 20);
    }

    public void SetSfxVolume(float value)
    {
        mixer.SetFloat("sfx", Mathf.Log10(value) * 20);
    }

    public void TransitionToMain()
    {
        main.TransitionTo(0);
    }
    
    public void MusicFadeOut()
    {
        musicFadeout.TransitionTo(fadeOutTime);
    }

    public void PlayMusic(bool value)
    {
        if (musicPlayer is null)//Null check
            return;
        
        if (value)
            musicPlayer.Play();
        else
            musicPlayer.Stop();
    }
}
