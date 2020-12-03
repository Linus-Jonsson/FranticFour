using System;
using UnityEngine;

public class CharacterMaskClose : MonoBehaviour
{
    [SerializeField] private float animTime = 1.2f;
    [SerializeField] private float xScaleTo = 3f;

    public void OnSelected()
    {
        LeanTween.scaleX(gameObject, xScaleTo, animTime);
    }
}
