using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [Header("Sprite renderers")] [Tooltip("Should be assigned in the order [0] = left, [1] = middle, [2] = right")]
    [SerializeField] private SpriteRenderer[] spriteRenderers = new SpriteRenderer[3];
    [Header("Character sprites")]
    [SerializeField] private Sprite[] characters = new Sprite[4];

    private void Start()
    {
        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].sprite = characters[i];
    }

    public void NextCharacter(bool _right)
    {
        if (_right)
        {
            Sprite m_temp = characters[characters.Length - 1];

            for (int i = characters.Length - 2; i >= 0; i--)
                characters[i + 1] = characters[i];
            characters[0] = m_temp;
        }
        else
        {
            Sprite m_temp = characters[0];

            for (int i = 1; i < characters.Length; i++)
                characters[i - 1] = characters[i];
            characters[characters.Length - 1] = m_temp;   
        }
        UpdateRenderer();
    }
}
