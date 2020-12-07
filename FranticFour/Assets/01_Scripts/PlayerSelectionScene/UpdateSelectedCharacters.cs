using UnityEngine;
using UnityEngine.Events;

public class UpdateSelectedCharacters : MonoBehaviour
{
    [SerializeField] private Sprite[] characters = new Sprite[4];
    [SerializeField] private Sprite[] charactersSelected = new Sprite[4];
    [SerializeField] private UnityEvent onSpriteChange;

    public UnityEvent OnSpriteChange => onSpriteChange;
    public Sprite[] Characters => characters;

    public void SetSelected(int _index)
    {
        characters[(_index + 1) % 4] = charactersSelected[(_index + 1) % 4];
        onSpriteChange.Invoke();
    }
}
