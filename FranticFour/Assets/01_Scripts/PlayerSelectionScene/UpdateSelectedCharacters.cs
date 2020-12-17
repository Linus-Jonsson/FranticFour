using UnityEngine;
using UnityEngine.Events;

public class UpdateSelectedCharacters : MonoBehaviour
{
    [SerializeField] private Sprite[] characters = new Sprite[4];
    [SerializeField] private Sprite[] charactersNotSelected = new Sprite[4];
    [SerializeField] private Sprite[] charactersSelected = new Sprite[4];
    [SerializeField] private Sprite[] charactersChosed = new Sprite[4];
    [SerializeField] private Color[] characterColors = new Color[4];
    [SerializeField] CharacterSelectionAnimator[] charactersDisplayed = new CharacterSelectionAnimator[4];

    [SerializeField] private UnityEvent onSpriteChange;

    public UnityEvent OnSpriteChange => onSpriteChange;
    public Sprite[] Characters => characters;

    public void SetSelected(int _index)
    {
        characters[(_index + 1) % 4] = charactersSelected[(_index + 1) % 4];
        charactersDisplayed[(_index + 1) % 4].StopAnimations();
        onSpriteChange.Invoke();
    }
    
    public void SetDeselected(int _index)
    {
        characters[(_index + 1) % 4] = charactersNotSelected[(_index + 1) % 4];
        charactersDisplayed[(_index + 1) % 4].PlayAnimations();
        onSpriteChange.Invoke();
    }

    public Sprite GetSelected(int _index)
    {
        return charactersChosed[(_index + 1) % 4];
    }

    public Sprite GetDeselected(int _index)
    {
        return charactersNotSelected[(_index + 1) % 4];
    }

    public Color GetSelectedColor(int _index)
    {
        return characterColors[(_index + 1) % 4];
    }
}
