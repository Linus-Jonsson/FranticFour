using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip jump;
    [SerializeField] float jumpVolume = 1.0f;
    [SerializeField] AudioClip push;
    [SerializeField] float pushVolume = 1.0f;
    [SerializeField] AudioClip pushed;
    [SerializeField] float pushedVolume = 1.0f;

    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundClip)
    {
        switch (soundClip)
        {
            case "jump":
                audioSource.PlayOneShot(jump, jumpVolume);
                break;
            case "push":
                audioSource.PlayOneShot(push, pushVolume);
                break;
            case "pushed":
                audioSource.PlayOneShot(pushed, pushedVolume);
                break;
        }
    }
}
