using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndRespawnController : MonoBehaviour
{
    [Tooltip("The value in score that the prey is worth when killing")]
    [SerializeField] int scoreValue = 3;
    [SerializeField] GameObject deathParticles = null;
    [SerializeField] GameObject onOffObject = null;
    [SerializeField] GameObject body = null;
    [SerializeField] Color originalColor = new Color(0, 0, 0, 255);

    Player player;
    GameLoopController gameLoopController;
    PlayerActionsController playerActionController;
    MovementController movementController;
    SpriteRenderer spriteRenderer;


    void Start()
    {
        player = GetComponent<Player>();
        gameLoopController = FindObjectOfType<GameLoopController>();
        playerActionController = GetComponent<PlayerActionsController>();
        movementController = GetComponent<MovementController>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Danger"))
            HandleDeath();
    }

    private void HandleDeath()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (player.PushedBy != null && player.Prey)
            HandlePreyKilledBySomeone();
        else if (player.Prey)
            HandlePreyLapseInJudgement();

        if (deathParticles) //Null check
            Instantiate(deathParticles, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        if (!player.Prey)
            gameLoopController.RespawnPlayer(player, onOffObject);
    }
    private void HandlePreyKilledBySomeone()
    {
        player.PushedBy.IncreaseScore(scoreValue);
        player.NumberOfDeaths = player.NumberOfDeaths + 1;
        gameLoopController.RespawnAllPlayers(player.PushedBy);
    }
    private void HandlePreyLapseInJudgement()
    {
        gameLoopController.IncreaseAllScores(Mathf.RoundToInt(scoreValue / 3));
        player.NumberOfDeaths = player.NumberOfDeaths + 1;
        gameLoopController.RespawnAllPlayers(null);
    }


    public void ResetPlayer(Vector3 position)
    {
        playerActionController.ResetPlayerActions();
        movementController.ResetMovement();
        SetNewPosition(position);
        player.PushedBy = null;
    }
    private void SetNewPosition(Vector3 position)
    {
        transform.position = position;
    }

    // remove methods below this point once we have set respawn points for hunters.
    public void ResetPlayer()
    {
        spriteRenderer.color = originalColor;
        playerActionController.ResetPlayerActions();
        movementController.ResetMovement();
        SetNewPosition();
        player.PushedBy = null;
    }
    private void SetNewPosition()
    {
        transform.position = new Vector3(Random.Range(-7f, 1f), Random.Range(-0.38f, 4.3f), transform.position.z);
    }
}
