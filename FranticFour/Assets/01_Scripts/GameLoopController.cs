using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] GameObject[] spawnPoints = new GameObject[4];

    int currentRound = 0;
    Player leader = null;
    Player currentPrey = null;
    GameLoopUIController gameLoopUIController;
    [SerializeField] private List<int> preyProbability; //Serialized temporarily to make sure it works properly
    TargetGroupController targetGroupController;
    
    void Start()
    {
        preyProbability = new List<int> {0, 1, 2, 3};
        gameLoopUIController = FindObjectOfType<GameLoopUIController>();
        targetGroupController = FindObjectOfType<TargetGroupController>();
        StartCoroutine(HandleGameLoop());
    }

    IEnumerator HandleGameLoop()
    {
        DeactivatePlayers();
        while (currentRound < numberOfRounds)
        {
            yield return StartCoroutine(gameLoopUIController.PreRoundCountdown(startCountDownDuration, players, currentRound + 1));
            SetPrey();
            SpawnPlayers();

            yield return StartCoroutine(gameLoopUIController.preyCountdown(currentPrey,preyRevealDuration));
            ActivatePlayers();

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
        int random = Random.Range(0, preyProbability.Count);
        int numberOfPrey = preyProbability[random];
        //Debug.Log($"Random: {random}, Player: {numberOfPrey}"); - To check if it works properly (REMOVE later)
        for (int i = 0; i < players.Length; i++)
        {
            if(i == numberOfPrey)
            {
                players[i].Prey = true;
                players[i].GetComponent<MovementController>().MovementSpeed = 35f;
                currentPrey = players[i];
            }
            else
            {
                players[i].Prey = false;
                players[i].GetComponent<MovementController>().MovementSpeed = 40f;
                // preyProbability.Add(i); - Commented for playtest!
            }
        }
        for (int i = 0; i < preyProbability.Count; i++)
            if(preyProbability[i].Equals(numberOfPrey))
                preyProbability.RemoveAt(i);
        targetGroupController.UpdateTargetGroup(players);
    }

    public void SpawnPlayers()
    {
        int random = Random.Range(0, spawnPoints.Length - 1);
        SpawnPoint spawnPoint = spawnPoints[random].GetComponent<SpawnPoint>();
        int hunterSpawnCount = 1;
        foreach (var player in players)
        {
            player.GetComponent<PlayerActionsController>().ResetPlayerActions();
            if (player.Prey == true)
            {
                player.ResetPlayer(spawnPoint.spawnPosition[0].transform.position);
            }
            else
            {
                player.ResetPlayer(spawnPoint.spawnPosition[hunterSpawnCount].transform.position);
                hunterSpawnCount += 1;
            }
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
            player.Score = 0;
        }
        currentRound = 0;
        leader = null;
        currentPrey = null;
        StartCoroutine(HandleGameLoop());
    }
}
