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
        if (!player.Dead && (other.gameObject.layer == 10 || other.gameObject.layer == 13))
        {
            player.FreezeInput = true;
            switch (other.gameObject.tag)
            {
                case "Fire":
                    print("Collided with Fire trap");
                    player.SetAnimationTrigger("FireDeath");
                    break;
                case "Saw":
                    print("Collided with Saw trap");
                    player.SetAnimationTrigger("SawDeath");
                    break;
                case "Spike":
                    print("Collided with Spike trap");
                    player.SetAnimationTrigger("SpikeDeath");
                    break;
                case "Hole":
                    print("Collided with a hole");
                    player.SetAnimationTrigger("HoleDeath");
                    break;                
                case "Plant":
                    print("Collided with Plant trap");
                    player.SetAnimationTrigger("PlantDeath");
                    break;
                default:
                    Debug.LogWarning("Hit trap whose tag is not in the switch!");
                    player.SetAnimationTrigger("Death");
                    break;
            }
            HandleDeath();
        }

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
        if (player.PushedBy != null)
        {
            gameAudio.PlaySound("fanfare");
            player.PushedBy.SetAnimationTrigger("Win");
            player.PushedBy.IncreaseScore(player.ScoreValue);
            player.PushedBy.DisplayScoreIncrease(player.ScoreValue);

            if (player.AssistPusher != null)
            {
                player.AssistPusher.IncreaseScore(player.AssistScore);
                player.AssistPusher.DisplayScoreIncrease(player.AssistScore);
            }
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
