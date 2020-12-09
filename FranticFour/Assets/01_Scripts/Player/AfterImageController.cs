using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] ParticleSystem afterImage;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        ResetAfterImage();
    }

    void Update()
    {
        if(afterImage.isPlaying)
            afterImage.textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
    }

    public void TurnOnAfterImage()
    {
        afterImage.Play();
    }
    public void TurnOffAfterImage()
    {
        // might need to change to paus?
        afterImage.Stop();
    }

    public void ResetAfterImage()
    {
        // might need to change to paus?
        afterImage.Stop();
    }
}
