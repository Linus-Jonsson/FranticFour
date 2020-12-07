using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndRespawnController : MonoBehaviour
{
    [Tooltip("The value in score that the prey is worth when killing")]
    [SerializeField] int scoreValue = 3;
    [SerializeField] GameObject deathParticles = null;
    [SerializeField] GameObject body = null;
    [SerializeField] Color originalColor = new Color(0, 0, 0, 255);

    Player player;
    InGameLoopController gameLoopController;
    PlayerActionsController playerActionController;
    MovementController movementController;
    SpriteRenderer spriteRenderer;
    PlayerGhostController playerGhostController;
    PushController pushController;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        player = GetComponent<Player>();
        gameLoopController = FindObjectOfType<InGameLoopController>();
        playerActionController = GetComponent<PlayerActionsController>();
        movementController = GetComponent<MovementController>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        playerGhostController = FindObjectOfType<PlayerGhostController>();
        pushController = GetComponentInChildren<PushController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Danger") || other.gameObject.layer == 13)
            HandleDeath();
    }

    private void HandleDeath()
    {       
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (deathParticles) //Null check
            ShowDeathEffect();
        if (player.Prey)
            HandlePreyKilled();
        else
            gameLoopController.spawnPlayer(player);
    }
    private void ShowDeathEffect()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(deathParticles, position, Quaternion.identity);
    }

    private void HandlePreyKilled()
    {
        if (player.PushedBy != null)
            player.PushedBy.IncreaseScore(scoreValue);
        else
            gameLoopController.IncreaseAllScores(Mathf.RoundToInt(scoreValue / 3));

        player.NumberOfDeaths = player.NumberOfDeaths + 1;
        gameLoopController.RespawnAllPlayers(player.PushedBy);

    }

    public void ResetPlayer()
    {
        ResetOtherScriptsAndColor();
        ResetPushController();
    }
    public void ResetPlayer(Vector3 position)
    {
        ResetOtherScriptsAndColor();
        ResetPushController();
        SetNewPosition(position);
    }

    private void SetNewPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void ResetOtherScriptsAndColor()
    {
        spriteRenderer.color = originalColor;
        player.Dead = false;
        playerActionController.ResetPlayerActions();
        movementController.ResetMovement();
        playerGhostController.ResetRespawn();
    }

    private void ResetPushController()
    {
        if (player.PushedBy != null)
            player.PushedBy.GetComponentInChildren<PushController>().RemoveFromPushList(transform);
        player.PushedBy = null;
        pushController.ResetPushList();
    }
}
