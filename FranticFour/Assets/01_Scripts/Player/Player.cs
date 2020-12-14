using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // [SerializeField] string playerName = ""; // not currently in use
    // public string PlayerName { get { return playerName; } } 

    [SerializeField] GameObject preySignifier = null;

    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] public Color originalColor = new Color(0, 0, 0, 0);

    [SerializeField] public Color ghostColor = new Color(0, 0, 0, 50);

    [SerializeField] int scoreValue = 3;
    public int ScoreValue { get { return scoreValue; } }

    int totalScore = 0;
    public int TotalScore { get { return totalScore; } set { totalScore = value; } }

    int roundScore = 0;
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

    Player pushedBy = null;
    public Player PushedBy { get { return pushedBy; } set { pushedBy = value; } }
    
    public UnityEvent BecamePray = new UnityEvent();

    DeathController deathController;
    MovementController movementController;
    PlayerActionsController playerActionsController;
    PlayerAnimationsController playerAnimationsController;
    RespawnController respawnController;
    PushController pushController;
    RotationController rotationController;
    AfterImageController afterImageController;

    // remove this once we are done with the print message.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
            print("Triggered a hole and is now falling to death");
    }

    private void Awake()
    {
        dead = false;
        playerNumber = GetComponent<AssignedController>().PlayerID + 1;
        GetReferences();
    }
    private void GetReferences()
    {
        deathController = GetComponent<DeathController>();
        movementController = GetComponent<MovementController>();
        playerActionsController = GetComponent<PlayerActionsController>();
        respawnController = GetComponent<RespawnController>();
        pushController = GetComponentInChildren<PushController>();
        rotationController = GetComponent<RotationController>();
        playerAnimationsController = GetComponent<PlayerAnimationsController>();
        afterImageController = GetComponent<AfterImageController>();
    }

    public void UnFreeze()
    {
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

    public void EndPush()
    {
        if(pushedBy)
        pushedBy.GetComponentInChildren<PushController>().RemoveFromPushList(this);
        freezeInput = false;
        pushedBy = null;
    }

    public void StopIsPushed()
    {
        isPushed = false;
    }

    public void ResetPlayer()
    {
        spriteRenderer.sharedMaterial.color = originalColor;
        freezeInput = false;
        isPushed = false;
        dead = false;
        respawnController.ResetRespawn();
        playerActionsController.ResetPlayerActions();
        movementController.ResetMovement();
        afterImageController.ResetAfterImage();
        if(pushedBy != null)
            pushedBy.GetComponentInChildren<PushController>().RemoveFromPushList(this);
        pushedBy = null;
        pushController.ResetPushList();
    }

    public void SetPrey(bool value, float speed)
    {
        isPrey = value;
        preySignifier.SetActive(value);
        movementController.MovementSpeed = speed;
    }

    public void SetNewPosition(Vector3 position)
    {
        transform.position = position;
    }

    // have the animation call the playerGhostController instead.
    public void playerDead()
    {
        StopIsPushed();
        afterImageController.ResetAfterImage();
        respawnController.StartGhosting();
    }
}