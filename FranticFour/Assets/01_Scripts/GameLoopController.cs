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
    [SerializeField] float hunterSpeed = 50f;
    [SerializeField] float preySpeed = 45f;
    [SerializeField] GameObject[] onOffObjects = new GameObject[4]; // this can be removed once we implement a better way to disable cooldown bars

    [Header("Other score addition configurations")]
    [SerializeField] int preySurvivalBaseScore = 10;
    [SerializeField] int pacifistAward = 10;

    [Header("Respawn Configurations")]
    [SerializeField] float respawnDelay = 3f;
    

    int currentRound = 1;
    Player leader = null;
    Player currentPrey = null;
    GameLoopUIController gameLoopUIController;
    [SerializeField] private List<int> preyProbability; //Serialized temporarily to make sure it works properly
    TargetGroupController targetGroupController;
    
    void Start()
    {
        gameLoopUIController = FindObjectOfType<GameLoopUIController>();
        targetGroupController = FindObjectOfType<TargetGroupController>();
        StartCoroutine(HandleGameLoop());
    }

    IEnumerator HandleGameLoop()
    {
        preyProbability = new List<int> { 0, 1, 2, 3 }; // resets the preyProbability list everytime you restart the game.
        DeactivatePlayers();
        while (currentRound <= numberOfRounds)
        {
            yield return StartCoroutine(gameLoopUIController.PreRoundCountdown(startCountDownDuration, players, currentRound));
            SetPrey();
            SpawnAllPlayers();

            yield return StartCoroutine(gameLoopUIController.preyCountdown(currentPrey, preyRevealDuration));
            ActivatePlayers();

            yield return StartCoroutine(gameLoopUIController.CountRoundTime(roundDuration));
            StopCoroutine(HandleRespawnOfAllPlayers(null));
            StopCoroutine(HandlePlayerRespawn(null));
            gameLoopUIController.SetKillScreen(null, null, false);
            DeactivatePlayers();
            CalculateScores();
            DisplayScores();
            if (currentRound == numberOfRounds)
                break;
            yield return StartCoroutine(gameLoopUIController.NextRoundCountdown(players, roundOverDuration, currentRound));
            currentRound++;
        }
        gameLoopUIController.DisplayFinalResults(players);
    }

    private void SetPrey()
    {
        int random = Random.Range(0, preyProbability.Count);
        int numberOfPrey = preyProbability[random];
        //Debug.Log($"Random: {random}, Player: {numberOfPrey}"); - To check if it works properly (REMOVE later)
        for (int i = 0; i < players.Length; i++)
        {
            players[i].HuntersKilled = 0;
            if(i == numberOfPrey)
            {
                players[i].Prey = true;
                players[i].GetComponent<MovementController>().MovementSpeed = preySpeed;
                currentPrey = players[i];
            }
            else
            {
                players[i].Prey = false;
                players[i].GetComponent<MovementController>().MovementSpeed = hunterSpeed;
                // preyProbability.Add(i); - Commented for playtest!
            }
        }
        for (int i = 0; i < preyProbability.Count; i++)
            if(preyProbability[i].Equals(numberOfPrey))
                preyProbability.RemoveAt(i);
        targetGroupController.UpdateTargetGroup(players);
    }

    private void SpawnAllPlayers()
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

    public void RespawnAllPlayers(Player killer)
    {
        StartCoroutine(HandleRespawnOfAllPlayers(killer));
    }
    private IEnumerator HandleRespawnOfAllPlayers(Player killer)
    {
        foreach (var player in players)
        {
            player.GetComponent<MovementController>().FreezeInput = true;
            player.gameObject.SetActive(false);
        }
        gameLoopUIController.SetKillScreen(currentPrey, killer, true);
        yield return new WaitForSeconds(respawnDelay);
        int random = Random.Range(0, spawnPoints.Length - 1);
        SpawnPoint spawnPoint = spawnPoints[random].GetComponent<SpawnPoint>();
        int hunterSpawnCount = 1;
        foreach (var player in players)
        {
            player.gameObject.SetActive(true);
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
            player.GetComponent<MovementController>().FreezeInput = false;
        }
        gameLoopUIController.SetKillScreen(currentPrey, killer, false);
    }
    public void RespawnPlayer(Player playerToSpawn)
    {
        StartCoroutine(HandlePlayerRespawn(playerToSpawn));
    }
    private IEnumerator HandlePlayerRespawn(Player playerToSpawn)
    {
        playerToSpawn.GetComponent<MovementController>().FreezeInput = true;
        playerToSpawn.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        playerToSpawn.gameObject.SetActive(true);
        playerToSpawn.ResetPlayer();
        playerToSpawn.GetComponent<MovementController>().FreezeInput = false;
    }

    private void CalculateScores()
    {
        // this method will be used to calculate the score that the prey should get based on how few times they died.
        // in here all special point reward systems will be handled aswell

        GivePreySurvivalScore();
        GivePacifistReward();
    }

    private void GivePreySurvivalScore()
    {
        int scoreToAdd = preySurvivalBaseScore - currentPrey.NumberOfDeaths;
        if (scoreToAdd < 0)
            scoreToAdd = 0;
        currentPrey.IncreaseScore(scoreToAdd);
    }

    private void GivePacifistReward()
    {
        List<int> huntersKilledByPlayers = new List<int>();
        foreach (var player in players)
        {
            if (player.Prey)
                continue;
            huntersKilledByPlayers.Add(player.HuntersKilled);
        }
        huntersKilledByPlayers.Sort();
        int lowestAmount = huntersKilledByPlayers[0];
        List<Player> eligiblePlayers = new List<Player>();
        foreach (var player in players)
        {
            if (player.Prey)
                continue;
            if (player.HuntersKilled == lowestAmount)
            {
                eligiblePlayers.Add(player);
            }
        }
        int pacifistTrueReward = Mathf.RoundToInt(pacifistAward / eligiblePlayers.Count);
        foreach (var player in eligiblePlayers)
        {
            player.IncreaseScore(pacifistTrueReward);
        }
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
        foreach (var onOffObject in onOffObjects) // the naming sucks, will be changed once a better system is implemented
        {
            onOffObject.SetActive(false);
        }
    }

    private void ActivatePlayers()
    {
        foreach (var onOffObject in onOffObjects) // the naming sucks, will be changed once a better system is implemented
        {
            onOffObject.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        foreach (var player in players)
        {
            player.ResetPlayer();
            player.Score = 0;
        }
        currentRound = 1;
        leader = null;
        currentPrey = null;
        StartCoroutine(HandleGameLoop());
    }

    public void IncreaseAllScores(int scoreToAdd)
    {
        foreach (var player in players)
        {
            if(player.Prey) { continue; }
            player.IncreaseScore(scoreToAdd);
        }
    }
}
