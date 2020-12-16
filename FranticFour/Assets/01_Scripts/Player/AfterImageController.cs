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
        afterImage.textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
    }

    public void TurnOnAfterImage()
    {
        afterImage.Play();
    }
    public void TurnOffAfterImage()
    {
        afterImage.Stop();
    }

    public void ResetAfterImage()
    {
        afterImage.Stop();
    }
}   
