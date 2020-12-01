using UnityEngine;
using TMPro;

public class GamePlayUIDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI roundTime = null; // remove later?
    [SerializeField] protected float timeDecreaseIncrement = 0.1f;

    [Header("Pree Prey Reveal Display Configuration")]
    [SerializeField] protected GameObject preeRoundDisplay = null;

    [SerializeField] protected TextMeshProUGUI preRoundTime = null; // remove later?
    [SerializeField] protected TextMeshProUGUI round = null;

    [SerializeField] protected TextMeshProUGUI orangePlayer = null;
    [SerializeField] protected TextMeshProUGUI greenPlayer = null;
    [SerializeField] protected TextMeshProUGUI purplePlayer = null;
    [SerializeField] protected TextMeshProUGUI cyanPlayer = null;

    [Header("Prey Display Configuration")]
    [SerializeField] protected GameObject preyDisplay = null;
    [SerializeField] protected GameObject[] preyImages = null;
    [SerializeField] protected TextMeshProUGUI preyNumber = null;
    [SerializeField] protected TextMeshProUGUI preyCountdown = null; // remove later?

    [Header("Score Display Configuration")]
    [SerializeField] protected GameObject scoreDisplay = null;

    [SerializeField] protected TextMeshProUGUI orangeScorePlayer = null;
    [SerializeField] protected TextMeshProUGUI orangeCurrentScore = null;
    [SerializeField] protected TextMeshProUGUI orangeRoundScore = null;

    [SerializeField] protected TextMeshProUGUI greenScorePlayer= null;
    [SerializeField] protected TextMeshProUGUI greenCurrentScore = null;
    [SerializeField] protected TextMeshProUGUI greenRoundScore = null;

    [SerializeField] protected TextMeshProUGUI purpleScorePlayer = null;
    [SerializeField] protected TextMeshProUGUI purpleCurrentScore = null;
    [SerializeField] protected TextMeshProUGUI purpleRoundScore = null;

    [SerializeField] protected TextMeshProUGUI cyanScorePlayer = null;
    [SerializeField] protected TextMeshProUGUI cyanCurrentScore = null;
    [SerializeField] protected TextMeshProUGUI cyanRoundScore = null;

    [SerializeField] protected TextMeshProUGUI nextRoundInText = null; // remove later?
    [SerializeField] protected TextMeshProUGUI roundScoreText = null;

    [Header("Final result display configuration")]
    [SerializeField] protected GameObject finalResultDisplay = null;
    [SerializeField] protected int[] placementTextSizes = new int[4];
    [SerializeField] protected Color[] placementColors = new Color[4];
    [SerializeField] protected string[] placements = new string[4];

    [SerializeField] protected TextMeshProUGUI orangeTotalScore = null;
    [SerializeField] protected TextMeshProUGUI resultOrangeName = null;
    [SerializeField] protected TextMeshProUGUI greenTotalScore = null;
    [SerializeField] protected TextMeshProUGUI resultGreenName = null;
    [SerializeField] protected TextMeshProUGUI purpleTotalScore = null;
    [SerializeField] protected TextMeshProUGUI resultPurpleName = null;
    [SerializeField] protected TextMeshProUGUI cyanTotalScore = null;
    [SerializeField] protected TextMeshProUGUI resultCyanName = null;

    [SerializeField] protected TextMeshProUGUI orangePlacement = null;
    [SerializeField] protected TextMeshProUGUI greenPlacement = null;
    [SerializeField] protected TextMeshProUGUI purplePlacement = null;
    [SerializeField] protected TextMeshProUGUI cyanPlacement = null;


    [Header("Prey kill screen configuration")]
    [SerializeField] protected GameObject killScreenDisplay = null;
    [SerializeField] protected TextMeshProUGUI killedByText = null;
    [SerializeField] protected GameObject[] killerImages = null;
    [SerializeField] protected Transform[] hunterImageTransforms = null;
}
