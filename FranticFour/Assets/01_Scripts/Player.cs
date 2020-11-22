using System.Collections;
using UnityEngine;
//using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject deathParticles = null;
    [Tooltip("The amount of time in seconds that the one who recently pushed the player is saved before going back to null")]
    [SerializeField] float pushedByTime = 2f;

    [SerializeField] int scoreValue = 3;
    [SerializeField] int score = 0; // remove SerializeField once score tracking works properly
    public int Score { get { return score; } }

    [SerializeField] int playerNumber = 0; // Remove serializeField once done testing.

    public int PlayerNumber { get { return playerNumber; } }

    [SerializeField] bool prey = false; // remove the SerializeField from this once gamemanager sets who is prey

    [SerializeField] string playerName = "";

    Player pushedBy = null;

    public bool Prey { get { return prey; } set { prey = value; } }

    void Start()
    {
        playerNumber = GetComponent<AssignedController>().PlayerID +1;
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
        if (pushedBy != null && prey) // increase the player that pushed score if its not null and you are the prey.
            pushedBy.ChangeScore(scoreValue);

        if (deathParticles) //Null check
            Instantiate(deathParticles, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (prey)
            FindObjectOfType<GameLoopController>().SetPlayerPositions();
        // add a penalty to score if suicide? 
        else
            SetNewPosition();
    }

    public void SetNewPosition()
    {
        transform.position = new Vector3(Random.Range(-7f, 1f), Random.Range(-0.38f, 4.3f), transform.position.z);
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

    public void ChangeScore(int scoreChange)
    {
        score += scoreChange;
    }

    public void ResetPlayer()
    {
        score = 0;
    }
}