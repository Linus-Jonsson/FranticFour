using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionAnimator : MonoBehaviour
{
    [Tooltip("The number of clips in the animationController (do not count idle)")]
    [SerializeField] int numberOfClips = 2;
    [SerializeField] Vector2 changeTime = new Vector2(1,3);

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeClip();
    }

    IEnumerator WaitForClip()
    {
        float waitTime = Random.Range(changeTime.x, changeTime.y + 1);
        yield return new WaitForSeconds(waitTime);
        ChangeClip();
    }

    private void ChangeClip()
    {
        int clipRandom = Random.Range(0, numberOfClips);
        print(clipRandom);
        switch(clipRandom)
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

        StartCoroutine(WaitForClip());
    }

}
