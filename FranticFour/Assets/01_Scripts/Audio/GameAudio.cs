using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [SerializeField] AudioClip startShot = null;
    [SerializeField] float startShotVolume = 1.0f;
    [SerializeField] AudioClip fanfare = null;
    [SerializeField] float fanfareVolume = 1.0f;
    [SerializeField] AudioClip failure = null;
    [SerializeField] float failureVolume = 1.0f;
    [SerializeField] AudioClip point = null;
    [SerializeField] float pointVolume = 1.0f;
    [SerializeField] AudioClip finalFanfare = null;
    [SerializeField] float finalFanfareVolume = 1.0f;
    [SerializeField] AudioClip applause = null;
    [SerializeField] float applauseVolume = 1.0f;

    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundClip)
    {
        switch (soundClip)
        {
            case "startShot":
                audioSource.PlayOneShot(startShot, startShotVolume);
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
            case "finalFanfare":
                audioSource.PlayOneShot(finalFanfare, finalFanfareVolume);
                break;
            case "applause":
                audioSource.PlayOneShot(applause, applauseVolume);
                break;
        }
    }
}
