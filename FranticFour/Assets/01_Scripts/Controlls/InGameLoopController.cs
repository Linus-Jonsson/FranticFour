using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InGameLoopController : MonoBehaviour
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

    [SerializeField] GameObject spawnParticles = null;


    int currentRound = 1;
    Player currentPrey = null;
    GamePlayUIController gameLoopUIController;
    [SerializeField] private List<int> preyProbability; //Serialized temporarily to make sure it works properly
    TargetGroupController targetGroupController;

    void Start()
    {
        GetReferences();
        StartCoroutine(HandleGameLoop());
    }
    private void GetReferences()
    {
        gameLoopUIController = FindObjectOfType<GamePlayUIController>();
        targetGroupController = FindObjectOfType<TargetGroupController>();
    }

    IEnumerator HandleGameLoop()
    {
        preyProbability = new List<int> { 0, 1, 2, 3 }; // resets the preyProbability list everytime you play.
        while (currentRound <= numberOfRounds)
        {
            ActivateAllPlayers(false);
            yield return StartCoroutine(gameLoopUIController.PreRoundCountdown(startCountDownDuration, players, currentRound));
            HandleRoleSetting();
            targetGroupController.UpdateTargetGroup(players);
            yield return StartCoroutine(gameLoopUIController.PreyCountdown(currentPrey, preyRevealDuration));
            HandleStartOfRound();
            yield return StartCoroutine(gameLoopUIController.CountRoundTime(roundDuration));
            HandleEndOfRound();
            if (currentRound == numberOfRounds)
                break;
            yield return StartCoroutine(gameLoopUIController.NextRoundCountdown(players, roundOverDuration, currentRound));
            currentRound++;
        }

        gameLoopUIController.DisplayFinalResults(players);
    }        
    private void ActivateAllPlayers(bool value)
    {
        foreach (var onOffObject in onOffObjects) // the naming sucks, will be changed once a better system is implemented
        {
            ActivatePlayer(value, onOffObject);
        }
    }
    private static void ActivatePlayer(bool value, GameObject onOffObject)
    {
        onOffObject.SetActive(value);
        onOffObject.GetComponentInChildren<Player>().FreezeInput = false;
    }

    private void HandleRoleSetting()
    {
        int random = Random.Range(0, preyProbability.Count);
        int numberOfPrey = preyProbability[random];
        //Debug.Log($"Random: {random}, Player: {numberOfPrey}"); - To check if it works properly (REMOVE later)
        SetPreyTrueOrFalse(numberOfPrey);
        ChangePreyProbability(numberOfPrey);
    }

    private void SetPreyTrueOrFalse(int numberOfPrey)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i == numberOfPrey)
                SetPrey(true, i, preySpeed);
            else
                SetPrey(false, i, hunterSpeed);
        }
    }
    private void SetPrey(bool value, int i, float speed)
    {
        players[i].NumberOfDeaths = 0;
        players[i].HuntersKilled = 0;
        players[i].GetComponent<MovementController>().MovementSpeed = speed;
        players[i].Prey = value;
        if (value)
        {
            currentPrey = players[i];
            FindObjectOfType<HunterRespawnHandler>().SetPrey(players[i].transform);
        }

/*        else
            preyProbability.Add(i); implement once game is done getting tested*/
    }

    private void ChangePreyProbability(int numberOfPrey)
    {
        for (int i = 0; i < preyProbability.Count; i++)
            if (preyProbability[i].Equals(numberOfPrey))
                preyProbability.RemoveAt(i);
    }

    private void HandleStartOfRound()
    {
        ActivateAllPlayers(true);
        StartCoroutine(SpawnAllPlayers());
    }
    private void HandleEndOfRound()
    {
        StopCoroutine(HandleRespawnOfAllPlayers(null));
        StopCoroutine(HandlePlayerRespawn(null, null));
        gameLoopUIController.SetKillScreen(null, null, false);
        ActivateAllPlayers(false);
        CalculateScores();
    }

    IEnumerator SpawnAllPlayers()
    {
        SpawnPoint spawnPoint = GetSpawnPoint();
        int hunterSpawnCount = 1;
        foreach (var player in players)
        {
            Vector2 spawnPosition;
            player.GetComponent<DeathAndRespawnController>().ResetPlayer();
            if (player.Prey == true)
            {
                spawnPosition = spawnPoint.spawnPosition[0].transform.position;
                spawnPlayer(spawnPosition, player);
                player.FreezeInput = true;
            }
            else
            {
                spawnPosition = spawnPoint.spawnPosition[hunterSpawnCount].transform.position;
                spawnPlayer(spawnPosition, player);
                player.FreezeInput = true;
                hunterSpawnCount += 1;
            }
        }
        yield return new WaitForSeconds(1);
        foreach (var player in players)
        {
            player.FreezeInput = false;
        }
    }
    private SpawnPoint GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Length - 1);
        SpawnPoint spawnPoint = spawnPoints[random].GetComponent<SpawnPoint>();
        return spawnPoint;
    }
    private void spawnPlayer(Vector2 spawnPosition, Player player)
    {
        player.GetComponent<DeathAndRespawnController>().ResetPlayer(spawnPosition);
        Instantiate(spawnParticles, new Vector3(spawnPosition.x, spawnPosition.y - 0.5f, 0), Quaternion.identity);
    }
    private void spawnPlayer(Player player)
    {
        player.GetComponent<DeathAndRespawnController>().ResetPlayer();
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y);
        Instantiate(spawnParticles, new Vector3(position.x, position.y - 0.5f, 0), Quaternion.identity);
    }

    public void RespawnAllPlayers(Player killer)
    {
        StartCoroutine(HandleRespawnOfAllPlayers(killer));
    }
    private IEnumerator HandleRespawnOfAllPlayers(Player killer)
    {
        ActivateAllPlayers(false);
        gameLoopUIController.SetKillScreen(currentPrey, killer, true);
        yield return new WaitForSeconds(respawnDelay);
        ActivateAllPlayers(true);
        StartCoroutine(SpawnAllPlayers());
        gameLoopUIController.SetKillScreen(currentPrey, killer, false);
    }

    public void RespawnPlayer(Player playerToSpawn, GameObject onOffObject)
    {
        StartCoroutine(HandlePlayerRespawn(playerToSpawn, onOffObject));
    }
    private IEnumerator HandlePlayerRespawn(Player playerToSpawn, GameObject onOffObject)
    {
        ActivatePlayer(false,onOffObject);
        yield return new WaitForSeconds(respawnDelay);
        ActivatePlayer(true, onOffObject);
        spawnPlayer(playerToSpawn);
    }

    private void CalculateScores()
    {
        // this method will be used to calculate the score that the prey should get based on how few times they died.
        // in here all special point reward systems will be handled aswell
        GivePreySurvivalScore();
        GivePacifistReward();
        TotalAllPlayersScore();
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
        List<Player> eligiblePlayers = GetPlayersEligibleForPacifistAward();
        int pacifistTrueReward = Mathf.RoundToInt(pacifistAward / eligiblePlayers.Count);
        foreach (var player in eligiblePlayers)
        {
            player.IncreaseScore(pacifistTrueReward);
        }
    }
    private List<Player> GetPlayersEligibleForPacifistAward()
    {
        int lowestHunterKills = GetLowestHunterKillHunterNumber();
        List<Player> eligiblePlayers = new List<Player>();
        foreach (var player in players)
        {
            if (player.Prey || player.HuntersKilled != lowestHunterKills)
                continue;
            eligiblePlayers.Add(player);
        }
        return eligiblePlayers;
    }
    private int GetLowestHunterKillHunterNumber()
    {
        List<int> HunterXHunterNumberList = new List<int>();
        foreach (var player in players)
        {
            if (player.Prey)
                continue;
            HunterXHunterNumberList.Add(player.HuntersKilled);
        }
        HunterXHunterNumberList.Sort();
        return HunterXHunterNumberList[0];
    }

    public void PlayAgain()
    {
        foreach (var player in players)
        {
            player.GetComponent<DeathAndRespawnController>().ResetPlayer();
            player.TotalScore = 0;
        }
        currentRound = 1;
        currentPrey = null;
        StartCoroutine(HandleGameLoop());
    }

    public void IncreaseAllScores(int scoreToAdd)
    {
        foreach (var player in players)
        {
            if (player.Prey)
                continue;
            player.IncreaseScore(scoreToAdd);
        }
    }

    public void TotalAllPlayersScore()
    {
        foreach (var player in players)
        {
            player.SumScore();
        }
    }

    // not in use remove if no need for it.    
  /*Player leader = null;
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
    }*/
}
