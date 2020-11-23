using UnityEngine;

public class RainbowLerp : MonoBehaviour
{
    [SerializeField] public float Speed = 0.1f;
    private SpriteRenderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float H, S, V;
        Color.RGBToHSV(myRenderer.color, out H, out S, out V);
        H += Time.deltaTime * Speed;
        myRenderer.color = Color.HSVToRGB(H, S, V);
    }
}