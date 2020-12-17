using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [SerializeField] AudioClip gong;
    [SerializeField] float gongVolume = 1.0f;
    [SerializeField] AudioClip fanfare;
    [SerializeField] float fanfareVolume = 1.0f;
    [SerializeField] AudioClip failure;
    [SerializeField] float failureVolume = 1.0f;
    [SerializeField] AudioClip point;
    [SerializeField] float pointVolume = 1.0f;

    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundClip)
    {
        switch (soundClip)
        {
            case "gong":
                audioSource.PlayOneShot(gong, gongVolume);
                break;
            case "fanfare":
                audioSource.PlayOneShot(fanfare, fanfareVolume);
                break;
            case "failure":
                audioSource.PlayOneShot(failure, failureVolume);
                break;
            case "point":
                audioSource.PlayOneShot(point, pointVolume);
                break;
        }
    }
}
