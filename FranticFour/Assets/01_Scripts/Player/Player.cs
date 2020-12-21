using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour
{
    // [SerializeField] string playerName = ""; // not currently in use
    // public string PlayerName { get { return playerName; } } 

    [SerializeField] GameObject preySignifier = null;

    [Tooltip("The time (in sec) that the player has to survive as prey in order to get a point")]
    [SerializeField] float scoreIncreaseThreshold = 5f;
    [SerializeField] Animator scoreIncreaseAnimator = null;
    [SerializeField] TextMeshProUGUI scoreText = null;

    bool shouldIncreaseScore = false;
    public bool ShouldIncreaseScore { set { shouldIncreaseScore = value; } }
    [SerializeField] float scoreIncreaseTimer = 0f;
    int survivalStreak = 0;

    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] public Color originalColor = new Color(0, 0, 0, 0);

    [SerializeField] public Color ghostColor = new Color(0, 0, 0, 50);

    [SerializeField] int scoreValue = 3;
    public int ScoreValue { get { return scoreValue; } }

    int totalScore = 0;
    public int TotalScore { get { return totalScore; } set { totalScore = value; } }

    [SerializeField] int roundScore = 0;
    public int RoundScore { get { return roundScore; } set { roundScore = value; } }

    int playerNumber = 0;
    public int PlayerNumber { get { return playerNumber; } }

    int placement = 0;
    public int Placement { get{ return placement; } set{ placement = value; } }

    bool isPrey = false;
    public bool Prey { get { return isPrey; } set { isPrey = value; BecamePray.Invoke();} }

    int numberOfDeaths = 0;
    public int NumberOfDeaths { get { return numberOfDeaths; } set { numberOfDeaths = value; } }

    int huntersKilled = 0;
    public int HuntersKilled { get { return huntersKilled; } set { huntersKilled = value; } }

    bool freezeInput = false;
    public bool FreezeInput { get { return freezeInput; } set { freezeInput = value; } }

    bool dead = false;
    public bool Dead { get { return dead; } set { dead = value; } }

    bool isPushed = false;
    public bool IsPushed { get { return isPushed; } set { isPushed = value; } }

    bool pushing = false;
    public bool Pushing { get { return pushing; } set { pushing = value; } }

    Player pushedBy = null;
    public Player PushedBy { get { return pushedBy; } set { pushedBy = value; } }

    bool canPush = true;
    public bool CanPush { get { return canPush; } set { canPush = value; } }
    
    public UnityEvent BecamePray = new UnityEvent();

    DeathController deathController;
    MovementController movementController;
    PlayerActionsController playerActionsController;
    RespawnController respawnController;
    PushController pushController;
    RotationController rotationController;
    AfterImageController afterImageController;
    Animator animator;
    InGameLoopController gameloopController;

    // remove this once we are done with the print message.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
            print("Triggered a hole and is now falling to death");
    }

    private void Awake()
    {
        shouldIncreaseScore = false;
        dead = false;
        playerNumber = GetComponent<AssignedController>().PlayerID + 1;
        GetReferences();
        scoreText.fontSize = 0;
    }
    private void GetReferences()
    {
        deathController = GetComponent<DeathController>();
        movementController = GetComponent<MovementController>();
        playerActionsController = GetComponent<PlayerActionsController>();
        respawnController = GetComponent<RespawnController>();
        pushController = GetComponentInChildren<PushController>();
        rotationController = GetComponent<RotationController>();
        afterImageController = GetComponent<AfterImageController>();
        animator = GetComponent<Animator>();
        gameloopController = FindObjectOfType<InGameLoopController>();
    }

    public void UnFreeze()
    {
        if(!gameloopController.CurrentPrey.dead)
        freezeInput = false;
    }

    public void IncreaseScore(int scoreChange)
    {
        roundScore += scoreChange;
    }

    public void SumScore()
    {
        totalScore += roundScore;
    }
    
    public void AddRoundScoreToTotalScore()
    {
        if (roundScore <= 0)
            return;
        roundScore -= 1;
        totalScore += 1;
    }

    public void EndPush()
    {
        freezeInput = false;
        pushedBy = null;
    }

    public void StopIsPushed()
    {
        isPushed = false;
    }

    public void StopPushing()
    {
        pushing = false;
    }

    public void ResetCanPush()
    {
        canPush = true;
    }

    public void ResetPlayer()
    {
        HandleResetOfScoreTextAndSurvivalStreak();
        spriteRenderer.sharedMaterial.color = originalColor;
        StopPushing();
        StopIsPushed();
        ResetCanPush();
        freezeInput = false;
        dead = false;
        respawnController.ResetRespawn();
        playerActionsController.ResetPlayerActions();
        movementController.ResetMovement();
        afterImageController.ResetAfterImage();
        pushedBy = null;
        pushController.ResetPushList();
    }

    public void SetPrey(bool value, float speed)
    {
        isPrey = value;
        preySignifier.SetActive(value);
        movementController.MovementSpeed = speed;
    }

    public void SetAnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void SetNewPosition(Vector3 position)
    {
        transform.position = position;
    }

    // have the animation call the playerGhostController instead.
    public void playerDead()
    {
        dead = true;
        StopIsPushed();
        StopPushing();
        afterImageController.ResetAfterImage();
        respawnController.StartGhosting();
    }

    public void HandleResetOfScoreTextAndSurvivalStreak()
    {
        shouldIncreaseScore = false;
        scoreIncreaseAnimator.ResetTrigger("DisplayIncrease");
        scoreText.fontSize = 0;
        scoreIncreaseTimer = 0;
        survivalStreak = 0;
    }

    public async void ScoreIncreaseTimer()
    {
        while(isPrey)
        {
            await Task.Delay(100);
            if(shouldIncreaseScore)
                CheckIfIncreaseScore();
        }
    }
    private void CheckIfIncreaseScore()
    {
        scoreIncreaseTimer += 0.1f;
        if (scoreIncreaseTimer > scoreIncreaseThreshold)
            IncreasePreyScore();
    }
    private void IncreasePreyScore()
    {
        scoreIncreaseTimer -= scoreIncreaseThreshold;
        int scoreToAdd = 1 + survivalStreak;
        scoreText.text = "+" + scoreToAdd.ToString();
        scoreIncreaseAnimator.SetTrigger("DisplayIncrease");
        IncreaseScore(scoreToAdd);
        survivalStreak++;
    }
}