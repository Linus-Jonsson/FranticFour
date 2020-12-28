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
    [SerializeField] protected GameObject tenSecondsLeftDisplay = null;
    [SerializeField] protected GameObject roundCountDownDisplay = null;

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

    [SerializeField] protected TextMeshProUGUI duckTotalScore = null;
    [SerializeField] protected TextMeshProUGUI pigTotalScore = null;
    [SerializeField] protected TextMeshProUGUI bunnyTotalScore = null;
    [SerializeField] protected TextMeshProUGUI sheepTotalScore = null;

    [SerializeField] protected SpriteRenderer duckPlacement = null;
    [SerializeField] protected SpriteRenderer pigPlacement = null;
    [SerializeField] protected SpriteRenderer bunnyPlacement = null;
    [SerializeField] protected SpriteRenderer sheepPlacement = null;
    
    [SerializeField] protected Animator duckAnimator = null;
    [SerializeField] protected Animator pigAnimator = null;
    [SerializeField] protected Animator bunnyAnimator = null;
    [SerializeField] protected Animator sheepAnimator = null;

    [SerializeField] protected Sprite[] podiums = new Sprite[3];

    [Header("Prey kill screen configuration")]
    [SerializeField] protected GameObject killScreenDisplay = null;
    [SerializeField] protected TextMeshProUGUI killedByText = null;
    [SerializeField] protected GameObject[] killerImages = null;
    [SerializeField] protected Transform[] hunterImageTransforms = null;
}
