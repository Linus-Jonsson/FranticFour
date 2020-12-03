using TMPro;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    [Header("Visual stuff for player")]
    [Range(1,4)]
    [SerializeField] private int playerID;
    [SerializeField] private GameObject pressToJoinGameObject;
    [SerializeField] private TextMeshPro pressToJoinTmPro;
    [SerializeField] private GameObject pressToJoinIcon;
    [SerializeField] private SpriteRenderer pressToJoinSpriteRenderer;
    [SerializeField] private GameObject selectedHighlight;
    [SerializeField] private Color selectedColor;
    [SerializeField] private int selectedColorAlpha = 70;
    [SerializeField] private HighlightLerp selectedHighlightLerp;
    [SerializeField] private CharacterSelection selectedCharacterSelection;
    [SerializeField] private GameObject characterSelectionGameObject;
    [SerializeField] private CharacterMaskClose characterMaskCloseSelect;

    public int PlayerID => playerID;
    public GameObject PressToJoinGameObject => pressToJoinGameObject;
    public TextMeshPro PressToJoinTmPro => pressToJoinTmPro;
    public GameObject PressToJoinIcon => pressToJoinIcon;
    public SpriteRenderer PressToJoinSpriteRenderer => pressToJoinSpriteRenderer;
    public GameObject SelectedHighlight => selectedHighlight;
    public Color SelectedColor => selectedColor;
    public int SelectedColorAlpha => selectedColorAlpha;
    public HighlightLerp SelectedHighlightLerp => selectedHighlightLerp;
    public CharacterSelection SelectedCharacterSelection => selectedCharacterSelection;
    public GameObject CharacterSelectionGameObject => characterSelectionGameObject;
    public CharacterMaskClose CharacterMaskCloseSelect => characterMaskCloseSelect;
}
