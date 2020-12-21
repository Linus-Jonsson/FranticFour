using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] GameObject deathParticles = null;

    Player player;
    InGameLoopController gameLoopController;
    GameAudio gameAudio;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        player = GetComponent<Player>();
        gameLoopController = FindObjectOfType<InGameLoopController>();
        gameAudio = FindObjectOfType<GameAudio>().GetComponent<GameAudio>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!player.Dead && (other.gameObject.CompareTag("Danger") || other.gameObject.layer == 13))
            HandleDeath();
    }

    private void HandleDeath()
    {
        if (gameLoopController.CurrentPrey.Dead) { return; }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.playerDead();
        if (deathParticles) //Null check
            ShowDeathEffect();
        if (player.Prey)
            HandlePreyKilled();
    }
    private void ShowDeathEffect()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(deathParticles, position, Quaternion.identity);
    }

    private void HandlePreyKilled()
    {
        AudioController.instance.TransitionToLowerMusic();
        player.SetAnimationTrigger("Death");
        if (player.PushedBy != null)
        {
            gameAudio.PlaySound("fanfare");
            player.PushedBy.SetAnimationTrigger("Win");
            player.PushedBy.IncreaseScore(player.ScoreValue);
            player.PushedBy.DisplayScoreIncrease(player.ScoreValue);
        }
        else
        {
            gameAudio.PlaySound("failure");
            gameLoopController.IncreaseAllScores(Mathf.RoundToInt(player.PreyAccidentScoreToHunters));
        }
        player.FreezeInput = true;
        player.NumberOfDeaths = player.NumberOfDeaths + 1;
        gameLoopController.RespawnAllPlayers(player.PushedBy);
    }
}
