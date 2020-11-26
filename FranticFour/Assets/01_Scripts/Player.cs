using System.Collections;
using UnityEngine;
//using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject deathParticles = null;
    [Tooltip("The amount of time in seconds that the one who recently pushed the player is saved before going back to null")]
    [SerializeField] float pushedByTime = 2f;
    [Tooltip("The value in score that the prey is worth when killing")]
    [SerializeField] int scoreValue = 3;

    int score = 0;
    public int Score { get { return score; } set { score = value; } }
    int playerNumber = 0;
    public int PlayerNumber { get { return playerNumber; } }

    //[SerializeField] string playerName = ""; // not currently in use
    Player pushedBy = null;
    PlayerActionsController playerActionController;
    MovementController movementController;
    GameLoopController gameLoopController;

    string placement = "";
    public string Placement { get{ return placement; } set{ placement = value; } }

    int placeTextSize = 0;
    public int PlaceTextSize { get { return placeTextSize; } set { placeTextSize = value; } }

    Color placementColor = new Color(0, 0, 0, 255);
    public Color PlacementColor { get { return placementColor; } set { placementColor = value; } }

    bool prey = false;
    public bool Prey { get { return prey; } set { prey = value; } }

    int numberOfDeaths = 0;
    public int NumberOfDeaths { get { return numberOfDeaths; } }

    int huntersKilled = 0;
    public int HuntersKilled { get { return huntersKilled; } set { huntersKilled = value; } }

    private void Awake()
    {
        playerNumber = GetComponent<AssignedController>().PlayerID + 1;
        playerActionController = GetComponent<PlayerActionsController>();
        movementController = GetComponent<MovementController>();
        gameLoopController = FindObjectOfType<GameLoopController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Danger"))
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (pushedBy != null && prey)
        {
            pushedBy.IncreaseScore(scoreValue);
            numberOfDeaths++;
            gameLoopController.RespawnAllPlayers(pushedBy);
        }
        else if(prey)
        {
            gameLoopController.IncreaseAllScores(Mathf.RoundToInt(scoreValue / 3));
            numberOfDeaths++;
            gameLoopController.RespawnAllPlayers(null);
        }

        if (deathParticles) //Null check
            Instantiate(deathParticles, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        if(!prey)
            gameLoopController.RespawnPlayer(this);
    }

    private void SetNewPosition()
    {
        transform.position = new Vector3(Random.Range(-7f, 1f), Random.Range(-0.38f, 4.3f), transform.position.z);
    }
    private void SetNewPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void GetPushedBy(Player pusher)
    {
        StopCoroutine(HandlePushedByTimer(pusher));
        pushedBy = null;
        StartCoroutine(HandlePushedByTimer(pusher));
    }

    IEnumerator HandlePushedByTimer(Player pusher)
    {
        pushedBy = pusher;
        yield return new WaitForSeconds(pushedByTime);
        pushedBy = null;
    }

    public void IncreaseScore(int scoreChange)
    {
        score += scoreChange;
    }

    public void ResetPlayer(Vector3 position)
    {
        StopAllCoroutines();
        playerActionController.ResetPlayerActions();
        movementController.ResetMovement();
        SetNewPosition(position);
        pushedBy = null;
    }

    // remove this once we have set respawn points for hunters.
    public void ResetPlayer()
    {
        StopAllCoroutines();
        playerActionController.ResetPlayerActions();
        movementController.ResetMovement();
        SetNewPosition();
        pushedBy = null;
    }
}