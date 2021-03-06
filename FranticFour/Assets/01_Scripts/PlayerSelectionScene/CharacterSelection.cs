﻿using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [Header("Sprite renderers")] [Tooltip("Should be assigned in the order [0] = left, [1] = middle, [2] = right")]
    [SerializeField] private SpriteRenderer[] spriteRenderers = new SpriteRenderer[3];
    [Header("Character sprites")]
    [SerializeField] private Sprite[] characters = null;
    [SerializeField] private int selectedCharacterIndex = 0;
    [SerializeField] private UpdateSelectedCharacters playersSpriteHandler = null;
    [SerializeField] HighlightLerp lerpHandler = null;
    public int SelectedCharacterIndex => selectedCharacterIndex;

    private void Start()
    {
        playersSpriteHandler = GetComponentInParent<UpdateSelectedCharacters>();

        characters = new Sprite[playersSpriteHandler.Characters.Length];
        playersSpriteHandler.OnSpriteChange.AddListener(UpdateSprites);

        for (int i = 0; i < playersSpriteHandler.Characters.Length; i++)
            characters[i] = playersSpriteHandler.Characters[i];
        
        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].sprite = characters[(selectedCharacterIndex + i) % characters.Length];
    }

    private void UpdateSprites()
    {
        for (int i = 0; i < playersSpriteHandler.Characters.Length; i++)
            characters[(i + selectedCharacterIndex) % characters.Length] =
                playersSpriteHandler.Characters[(i + selectedCharacterIndex) % characters.Length];

        UpdateRenderer();
    }

    public void CharacterSelected()
    {
        // 1 = the renderer in the middle
        spriteRenderers[1].sprite = playersSpriteHandler.GetSelected(selectedCharacterIndex);
        lerpHandler.SetColor(playersSpriteHandler.GetSelectedColor(selectedCharacterIndex));
        playersSpriteHandler.OnSpriteChange.RemoveListener(UpdateSprites);
        playersSpriteHandler.SetSelected(selectedCharacterIndex);
    }
    
    public void CharacterDeselected()
    {
        // 1 = the renderer in the middle
        spriteRenderers[1].sprite = playersSpriteHandler.GetDeselected(selectedCharacterIndex);
        playersSpriteHandler.OnSpriteChange.AddListener(UpdateSprites);
        playersSpriteHandler.SetDeselected(selectedCharacterIndex);
    }

    public void NextCharacter(bool _right)
    {
        if (!_right)
        {
            selectedCharacterIndex++;
        }
        else
        {
            selectedCharacterIndex--;
            if (selectedCharacterIndex < 0)
                selectedCharacterIndex = characters.Length - 1;
            
        }

        if (selectedCharacterIndex < 0)
            selectedCharacterIndex += characters.Length;

        lerpHandler.SetColor(playersSpriteHandler.GetSelectedColor(selectedCharacterIndex));
        selectedCharacterIndex %= characters.Length;
        UpdateRenderer();
    }

    public int GetCharacterSelectedIndex()
    {
        return selectedCharacterIndex;
    }
}
