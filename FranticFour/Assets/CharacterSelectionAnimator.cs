using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionAnimator : MonoBehaviour
{
    [Tooltip("The number of clips in the animationController (do not count idle)")]
    [SerializeField] int numberOfClips = 2;
    [SerializeField] Vector2 changeTime = new Vector2(1,3);
    [Range(0, 1)] [SerializeField] float intensity = 0.5f;

    Animator animator;
    SpriteRenderer spriteRenderer;

    bool playAnimation = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playAnimation = true;
        animator = GetComponent<Animator>();
        StartCoroutine(WaitForClip());
    }

    IEnumerator WaitForClip()
    {
        float waitTime = Random.Range(changeTime.x, changeTime.y + 1);
        yield return new WaitForSeconds(waitTime);
        ChangeClip();
    }

    private void ChangeClip()
    {
        int clipToPlay = Random.Range(1, numberOfClips); // excludes the win animation that is used when selected.
        switch(clipToPlay)
        {
            case 0:
                animator.SetTrigger("Win");
                break;
            case 1:
                animator.SetTrigger("Jump");
                break;
            default:
                animator.SetTrigger("Idle");
                break;
        }
        if (playAnimation)
            StartCoroutine(WaitForClip());
    }

    private void ChangeClip(int clipToPlay)
    {
        switch (clipToPlay)
        {
            case 0:
                animator.SetTrigger("Win");
                break;
            case 1:
                animator.SetTrigger("Jump");
                break;
            default:
                animator.SetTrigger("Idle");
                break;
        }
        if (playAnimation)
            StartCoroutine(WaitForClip());
    }

    public void StopAnimations()
    {
        spriteRenderer.color = Color.HSVToRGB(0, 0, intensity);
        StopAllCoroutines();
        playAnimation = false;
        ChangeClip(0); // will play the win animation
    }
    public void PlayAnimations()
    {
        spriteRenderer.color = Color.HSVToRGB(0, 0, 1f);
        playAnimation = true;
        StartCoroutine(WaitForClip());
    }
}
