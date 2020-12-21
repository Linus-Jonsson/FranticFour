using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] AudioMixerSnapshot main = null;
    [SerializeField] AudioMixerSnapshot musicFadeout = null;
    [SerializeField] AudioMixerSnapshot musicOnly = null;
    [SerializeField] AudioMixerSnapshot lowerMusic = null;
    [SerializeField] float musicFadeOutTime = 2.0f;
    [SerializeField] AudioSource menuMusic = null;
    [SerializeField] AudioSource gameMusic = null;
    
    public static AudioController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
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
        main.TransitionTo(1);
    }
    
    public void TransitionToMusicOnly()
    {
        musicOnly.TransitionTo(0);
    }
    
    public void MusicFadeOut()
    {
        musicFadeout.TransitionTo(musicFadeOutTime);
    }
    
    public void TransitionToLowerMusic()
    {
        lowerMusic.TransitionTo(0);
    }

    public void PlayMenuMusic(bool value)
    {
        if (menuMusic is null)
            return;
        
        if (value)
            menuMusic.Play();
        else
            menuMusic.Stop();
    }
    
    public void PlayGameMusic(bool value)
    {
        if (gameMusic is null)
            return;
        
        if (value)
            gameMusic.Play();
        else
            gameMusic.Stop();
    }
}
