using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScoreAnimation : MonoBehaviour
{
    Animator animator;
    GameAudio gameAudio;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        gameAudio = AudioController.instance.GetComponentInChildren<GameAudio>();
    }

    public void ResetScoreTrigger()
    {
        animator.ResetTrigger("DisplayIncrease");
    }

    public void PlayScoreSound()
    {
        gameAudio.PlaySound("point");
    }
}
