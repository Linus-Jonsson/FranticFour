using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCountDownAnimation : MonoBehaviour
{
    GameAudio gameAudio;

    void Start()
    {
        gameAudio = AudioController.instance.GetComponentInChildren<GameAudio>();
    }

    public void PlayAirHornSound()
    {
        gameAudio.PlaySound("airHorn");
    }
}
