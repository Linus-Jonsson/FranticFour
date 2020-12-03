using System;
using UnityEngine;

public class HighlightLerp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color originalColor;
    [SerializeField] private float originalAlpha;
    [SerializeField] private float lerpSpeed;
    [SerializeField] public bool isLerping;
    [SerializeField] public bool hasSelected;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
        originalAlpha = originalColor.a;
    }

    public void StartLerp()
    {
        spriteRenderer.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            originalColor.a = 0);
        isLerping = true;
    }

    private void Update()
    {
        if (isLerping)
            LerpAlpha();
        if (hasSelected)
            LerpSelect();
    }

    private void LerpSelect()
    {
        //Todo Make transition better
        if (originalColor.a >= 1f)
            hasSelected = false;

        Color.RGBToHSV(spriteRenderer.color, out float _h, out float _s, out float _v);
        _s += Time.fixedTime * 0.01f;
        Color m_temp = Color.HSVToRGB(_h, _s, _v);
        
        m_temp = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            originalColor.a += Time.fixedTime * 0.01f);

        spriteRenderer.color = m_temp;
    }

    private void LerpAlpha()
    {
        spriteRenderer.color = new Color (
            originalColor.r,
            originalColor.g,
            originalColor.b, 
            Mathf.PingPong(Time.fixedTime * lerpSpeed, originalAlpha));
    }
}
