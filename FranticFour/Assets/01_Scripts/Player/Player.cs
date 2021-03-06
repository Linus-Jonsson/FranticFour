﻿using System.Collections;
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

    [Tooltip("The time before assist gets set to null after someone has been set to assist")]
    [SerializeField] float assistCountDownTimer = 5f;


    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] public Color originalColor = new Color(0, 0, 0, 0);

    [SerializeField] public Color ghostColor = new Color(0, 0, 0, 50);

    [SerializeField] int scoreValue = 3;
    public int ScoreValue { get { return scoreValue; } }

    [SerializeField] int assistScore = 2;
    public int AssistScore { get { return assistScore; } }
    
    [SerializeField] int preyAccidentScoreToHunters = 2;
    public int PreyAccidentScoreToHunters { get { return preyAccidentScoreToHunters; } }
    
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

    [SerializeField] Player pushedBy = null;
    public Player PushedBy { get { return pushedBy; } set { pushedBy = value; } }

    [SerializeField] Player assistPusher = null;
    public Player AssistPusher { get { return assistPusher; }  set { assistPusher = value; } }

    float assistCountdownTime = 0;

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
    GameAudio gameAudio;

    Coroutine assistTimer;
    
    static readonly int DisplayIncrease = Animator.StringToHash("DisplayIncrease");

    private void Awake()
    {
        shouldIncreaseScore = false;
        dead = false;
        playerNumber = GetComponent<AssignedController>().PlayerID + 1;
        GetReferences();
        scoreText.fontSize = 0;
        movementController.MovementSpeed = 200;
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
        gameAudio = AudioController.instance.GetComponentInChildren<GameAudio>();
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
        assistPusher = PushedBy;
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
        //What in the fuck
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
        assistPusher = null;
        pushController.ResetPushList();
    }

    public void ResetPlayerColor()
    {
        spriteRenderer.color = Color.white;
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
    
    public void SetAnimationBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
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
        gameloopController.RemovePushFromPrey(this);
    }

    public void StartGhosting()
    {
        respawnController.StartGhosting();
    }

    public void HandleResetOfScoreTextAndSurvivalStreak()
    {
        shouldIncreaseScore = false;
        scoreIncreaseAnimator.ResetTrigger(DisplayIncrease);
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
        DisplayScoreIncrease(scoreToAdd);
        IncreaseScore(scoreToAdd);
        survivalStreak++;
    }
    
    public void DisplayScoreIncrease(int score)
    {
        scoreText.text = "+" + score.ToString();
        if (scoreIncreaseAnimator != null)
            scoreIncreaseAnimator.SetTrigger(DisplayIncrease);
    }

    public void RestartAssistCounter()
    {
        if (assistTimer != null)
            StopCoroutine(assistTimer);
        assistTimer = StartCoroutine(AssistCountDown());
    }

    private IEnumerator AssistCountDown()
    {
        print("Started the assist countdown");
        assistCountdownTime = assistCountDownTimer;
        while (assistCountdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            assistCountdownTime -= 1f;
        }
        print("Ended the assist countdown");
        assistPusher = null;
    }
}