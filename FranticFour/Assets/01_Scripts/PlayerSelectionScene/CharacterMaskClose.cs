using UnityEngine;

public class CharacterMaskClose : MonoBehaviour
{
    [SerializeField] private float animTime = 1.2f;
    [SerializeField] private float animTimeDeselect = 0.5f;
    [SerializeField] private float xScaleTo = 3f;
    [SerializeField] private float xStandard = 6f;
    private int activeLeanTweenID = 0;

    public void OnSelected()
    {
        float m_ScTS = Mathf.Abs(xScaleTo - xStandard);
        float m_ScC = Mathf.Abs(transform.localScale.x - xScaleTo);
        
        float m_time = (animTime / m_ScTS) * m_ScC;
        
        LeanTween.cancel(activeLeanTweenID);
        activeLeanTweenID = LeanTween.scaleX(gameObject, xScaleTo, m_time).setOnComplete(ScaledUp).id;
    }
    
    public void OnDeselected()
    {
        float m_ScTS = Mathf.Abs(xScaleTo - xStandard);
        float m_ScC = Mathf.Abs(transform.localScale.x - xStandard);
        
        float m_time = (animTimeDeselect / m_ScTS) * m_ScC;
        
        
        LeanTween.cancel(activeLeanTweenID);
        activeLeanTweenID = LeanTween.scaleX(gameObject, xStandard, m_time).setOnComplete(ScaledStandard).id;
    }

    private void ScaledUp()
    {
        Vector3 m_scale = new Vector3();

        m_scale.x = xScaleTo;
        m_scale.y = transform.localScale.y;
        m_scale.z = transform.localScale.z;
    }
    private void ScaledStandard()
    {
        Vector3 m_scale = new Vector3();

        m_scale.x = xStandard;
        m_scale.y = transform.localScale.y;
        m_scale.z = transform.localScale.z;
    }
}
