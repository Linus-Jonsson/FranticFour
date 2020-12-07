using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MyPlayer : MonoBehaviour
{
    [Header("Visual stuff for player")]
    [Range(1,4)]
    [SerializeField] private int playerID = 0;
    [SerializeField] private GameObject pressToJoinGameObject = null;
    [SerializeField] private TextMeshPro pressToJoinTmPro = null;
    [SerializeField] private GameObject pressToJoinIcon = null;
    [SerializeField] private SpriteRenderer pressToJoinSpriteRenderer = null;
    [SerializeField] private GameObject selectedHighlight = null;
    [SerializeField] private Color selectedColor = new Color(0,0,0);
    [SerializeField] private int selectedColorAlpha = 70;
    [SerializeField] private HighlightLerp selectedHighlightLerp = null;
    [SerializeField] private CharacterSelection selectedCharacterSelection = null;
    [SerializeField] private GameObject characterSelectionGameObject = null;
    [SerializeField] private CharacterMaskClose characterMaskCloseSelect = null;

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
