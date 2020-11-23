using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    [Header("Timer durations")]
    [Tooltip("The time in seconds for the round")]
    [SerializeField] float roundDuration = 60f;
    [Tooltip("The time in seconds before prey is revealed")]
    [SerializeField] float startCountDownDuration = 5f;
    [Tooltip("The time in seconds that the prey is revealed before starting round")]
    [SerializeField] float preyRevealDuration = 5f;
    [Tooltip("The time in seconds that round over summary is displayed")]
    [SerializeField] float roundOverDuration = 5f;

    [Header("Other configurations")]
    [SerializeField] Player[] players = new Player[4];
    [SerializeField] int numberOfRounds = 5;
    int currentRound = 0;


    Player leader = null;
    Player currentPrey = null;
    GameLoopUIController gameLoopUIController;

    void Start()
    {
        gameLoopUIController = FindObjectOfType<GameLoopUIController>();
        StartCoroutine(HandleGameLoop());
    }

    IEnumerator HandleGameLoop()
    {        
        SetPlayerPositions();
        DeactivatePlayers();
        while (currentRound < numberOfRounds)
        {
            yield return StartCoroutine(gameLoopUIController.PreRoundCountdown(startCountDownDuration, players, currentRound + 1));
            SetPrey();

            yield return StartCoroutine(gameLoopUIController.preyCountdown(currentPrey,preyRevealDuration));
            ActivatePlayers();
            SetPlayerPositions();

            yield return StartCoroutine(gameLoopUIController.CountRoundTime(roundDuration));
            DeactivatePlayers();
            DisplayScores();

            yield return StartCoroutine(gameLoopUIController.NextRoundCountdown(players,roundOverDuration,currentRound));
            currentRound++;
        }
        gameLoopUIController.DisplayFinalResults(leader, players);
    }

    private void SetPrey()
    {
        // add a proper calculation in here making the one who has been prey the least be the most likely to become prey
        int random = Random.Range(0, players.Length);

        for (int i = 0; i < players.Length; i++)
        {
            if(i == random)
            {
                players[i].Prey = true;
                currentPrey = players[i];
            }
            else
            {
                players[i].Prey = false;
            }
        }

    }

    public void SetPlayerPositions()
    {
        foreach (var player in players)
        {
            player.SetNewPosition();
        }
    }

    private void CalculateScores()
    {
        // this method will be used to calculate the score that the prey should get based on how few times they died.
        // in here all special point reward systems will be handled aswell
    }

    private void DisplayScores()
    {
        // make this method set each players score in the UI controller instead
        foreach (var player in players)
        {
            if (player.Score <= 0)
                continue;
            if (leader == null)
                leader = player;
            else if (player.Score > leader.Score)
                leader = player;
        }
    }

    private void DeactivatePlayers()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(false);
        }
    }

    private void ActivatePlayers()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        foreach (var player in players)
        {
            player.ResetPlayer();
        }
        currentRound = 0;
        leader = null;
        currentPrey = null;
        StartCoroutine(HandleGameLoop());
    }
}
