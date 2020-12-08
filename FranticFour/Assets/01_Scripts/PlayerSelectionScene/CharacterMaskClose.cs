using UnityEngine;

public class CharacterMaskClose : MonoBehaviour
{
    [SerializeField] private float animTime = 1.2f;
    [SerializeField] private float animTimeDeselect = 0.5f;
    [SerializeField] private float xScaleTo = 3f;
    [SerializeField] private float xStandard = 6f;

    public void OnSelected()
    {
        LeanTween.scaleX(gameObject, xScaleTo, animTime);
    }
    
    public void OnDeselected()
    {
        LeanTween.scaleX(gameObject, xStandard, animTimeDeselect);
    }
}
