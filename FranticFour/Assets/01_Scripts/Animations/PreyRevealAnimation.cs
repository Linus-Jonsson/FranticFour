using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyRevealAnimation : MonoBehaviour
{
    Animator animator;
    GameAudio gameAudio;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameAudio = AudioController.instance.GetComponentInChildren<GameAudio>();
    }

    public void ResetPreyTrigger()
    {
        animator.ResetTrigger("Prey");
    }

    public void ResetNotPreyTrigger()
    {
        animator.ResetTrigger("NotPrey");
    }
    
    //Adding sound later
    public void PlayScoreSound()
    {
        gameAudio.PlaySound("");
    }
}
