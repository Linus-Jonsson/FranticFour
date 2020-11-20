using UnityEngine;

public class RainbowLerp : MonoBehaviour
{
    [SerializeField] public float Speed = 0.1f;
    [SerializeField] private SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float H, S, V;
        Color.RGBToHSV(renderer.color, out H, out S, out V);
        H += Time.deltaTime * Speed;
        renderer.color = Color.HSVToRGB(H, S, V);
    }
}