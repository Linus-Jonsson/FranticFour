using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroController : MonoBehaviour
{
    [SerializeField] int timeBeforeFirstText = 1;
    [SerializeField] TMP_Text firstText = null;
    [SerializeField] Animator firstTextAnimator = null;
    [SerializeField] static int timeBetweenFirstAndSecondText = 2;
    [SerializeField] TMP_Text secondText = null;
    [SerializeField] int timeBetweenSecondAndThirdText = 2;
    [SerializeField] TMP_Text thirdText = null;
    
    IEnumerator Start()
    {
        firstText.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(timeBeforeFirstText);
        StartCoroutine(FirstText(firstText, firstTextAnimator));
        yield return new WaitForSeconds(timeBetweenFirstAndSecondText);
    }

    private static IEnumerator FirstText(TMP_Text text, Animator animator)
    {
        animator.enabled = true;
        int totalVisibleCharacters = text.textInfo.characterCount;
        int counter = 0;
        while (counter < totalVisibleCharacters + 1)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            text.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(0.1f);
            counter += 1;
        }
        yield return new WaitForSeconds(timeBetweenFirstAndSecondText);
    }
}
