using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] GameObject deathParticles = null;

    Player player;
    InGameLoopController gameLoopController;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        player = GetComponent<Player>();
        gameLoopController = FindObjectOfType<InGameLoopController>();
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
            player.playerDead();
    }
    private void ShowDeathEffect()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(deathParticles, position, Quaternion.identity);
    }

    private void HandlePreyKilled()
    {
        if (player.PushedBy != null)
            player.PushedBy.IncreaseScore(player.ScoreValue);
        else
            gameLoopController.IncreaseAllScores(Mathf.RoundToInt(player.ScoreValue / 3));

        player.NumberOfDeaths = player.NumberOfDeaths + 1;
        gameLoopController.RespawnAllPlayers(player.PushedBy);
    }
}
