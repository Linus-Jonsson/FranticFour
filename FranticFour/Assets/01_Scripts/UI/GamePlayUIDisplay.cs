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

    [SerializeField] protected TextMeshProUGUI player1 = null;
    [SerializeField] protected TextMeshProUGUI player2 = null;
    [SerializeField] protected TextMeshProUGUI player3 = null;
    [SerializeField] protected TextMeshProUGUI player4 = null;

    [Header("Prey Display Configuration")]
    [SerializeField] protected GameObject preyDisplay = null;
    [SerializeField] protected GameObject[] preyImages = null;
    [SerializeField] protected TextMeshProUGUI preyNumber = null;
    [SerializeField] protected TextMeshProUGUI preyCountdown = null; // remove later?
    
    [Header("CountDown Configuration")]
    [SerializeField] protected GameObject countDownDisplay = null;

    [Header("Score Display Configuration")]
    [SerializeField] protected GameObject scoreDisplay = null;

    [SerializeField] protected TextMeshProUGUI player1ScoreHeader = null;
    [SerializeField] protected TextMeshProUGUI player1CurrentScore = null;
    [SerializeField] protected TextMeshProUGUI player1RoundScore = null;

    [SerializeField] protected TextMeshProUGUI player2ScoreHeader= null;
    [SerializeField] protected TextMeshProUGUI player2CurrentScore = null;
    [SerializeField] protected TextMeshProUGUI player2RoundScore = null;

    [SerializeField] protected TextMeshProUGUI player3ScoreHeader = null;
    [SerializeField] protected TextMeshProUGUI player3CurrentScore = null;
    [SerializeField] protected TextMeshProUGUI player3RoundScore = null;

    [SerializeField] protected TextMeshProUGUI player4ScoreHeader = null;
    [SerializeField] protected TextMeshProUGUI player4CurrentScore = null;
    [SerializeField] protected TextMeshProUGUI player4RoundScore = null;

    [SerializeField] protected TextMeshProUGUI nextRoundInText = null; // remove later?
    [SerializeField] protected TextMeshProUGUI roundScoreText = null;

    [Header("Final result display configuration")]
    [SerializeField] protected GameObject finalResultDisplay = null;
    [SerializeField] protected int[] placementTextSizes = new int[4];
    [SerializeField] protected Color[] placementColors = new Color[4];
    [SerializeField] protected string[] placements = new string[4];

    [SerializeField] protected TextMeshProUGUI player1TotalScore = null;
    [SerializeField] protected TextMeshProUGUI player1ResultHeader = null;
    [SerializeField] protected TextMeshProUGUI player2TotalScore = null;
    [SerializeField] protected TextMeshProUGUI player2ResultHeader = null;
    [SerializeField] protected TextMeshProUGUI player3TotalScore = null;
    [SerializeField] protected TextMeshProUGUI player3ResultHeader = null;
    [SerializeField] protected TextMeshProUGUI player4TotalScore = null;
    [SerializeField] protected TextMeshProUGUI player4ResultHeader = null;

    [SerializeField] protected GameObject player1Placement = null;
    [SerializeField] protected GameObject player2Placement = null;
    [SerializeField] protected GameObject player3Placement = null;
    [SerializeField] protected GameObject player4Placement = null;
    
    [Header("Prey kill screen configuration")]
    [SerializeField] protected GameObject killScreenDisplay = null;
    [SerializeField] protected TextMeshProUGUI killedByText = null;
    [SerializeField] protected GameObject[] killerImages = null;
    [SerializeField] protected Transform[] hunterImageTransforms = null;
}
