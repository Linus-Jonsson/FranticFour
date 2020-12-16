﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer = null;

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("master", Mathf.Log10(value) * 20);
    }
    
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("music", Mathf.Log10(value) * 20);
    }

    public void SetPlayerFXVolume(float value)
    {
        mixer.SetFloat("playerFX", Mathf.Log10(value) * 20);
    }
    
    public void SetGameFXVolume(float value)
    {
        mixer.SetFloat("gameFX", Mathf.Log10(value) * 20);
    }
    
    public void SetEnvironmentVolume(float value)
    {
        mixer.SetFloat("environment", Mathf.Log10(value) * 20);    
    }
}
