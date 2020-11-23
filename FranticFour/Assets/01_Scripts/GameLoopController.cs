using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    [SerializeField] Player[] players = new Player[4];

    [SerializeField] float roundDuration = 60f;

    [SerializeField] float startCountDownDuration = 5f;
    [SerializeField] float preyRevealDuration = 5f;
    [SerializeField] float roundOverDuration = 5f;

    [SerializeField] int numberOfRounds = 5;

    Player leader = null;
    Player currentPrey = null;

    public Player CurrentPrey { get { return currentPrey; } }

    int currentRound = 0;

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

            yield return StartCoroutine(gameLoopUIController.preyCountdown(CurrentPrey,preyRevealDuration));
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
                print("Prey is: " + players[i].gameObject.name);
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
            if (player.Score == 0)
                continue;
            else if (leader == null && player.Score > 0)
                leader = player;
            else if (player.Score > leader.Score)
                leader = player;

            print("Player: " + player.PlayerNumber + "\n" + "Score: " + player.Score);
        }
        if(leader != null)  
        print("Current score leader is: " + leader.gameObject.name);
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
