using UnityEngine;

public class HighlightLerp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Color originalColor = new Color(0, 0, 0);
    [SerializeField] private float maxAlphaSelected = 0.7f;
    [SerializeField] private float lerpSpeed = 0f;
    [SerializeField] public bool isLerping = false;
    [SerializeField] public bool hasSelected = false;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
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
        //When selected
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
        //While selecting
        spriteRenderer.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            Mathf.PingPong(Time.fixedTime * lerpSpeed, maxAlphaSelected));
    }
}