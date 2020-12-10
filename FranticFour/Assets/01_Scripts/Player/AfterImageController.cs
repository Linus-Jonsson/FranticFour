using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] ParticleSystem afterImage;
    [SerializeField] SpriteRenderer spriteRenderer;

    ParticleSystem.MainModule mainModule;
    private void Start()
    {
        mainModule = afterImage.main;
        ResetAfterImage();
    }

    void Update()
    {
        afterImage.textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
    }

    public void TurnOnAfterImage()
    {
        //afterImage.Clear();
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
