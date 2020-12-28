using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float HunterSpeed { get { return hunterSpeed; } }
    [SerializeField] float preySpeed = 45f;
    public float PreySpeed { get { return preySpeed; } }

    [Header("Other score addition configurations")]
    [SerializeField] int pacifistAward = 10;

    [Header("Respawn Configurations")]
    [SerializeField] float respawnDelay = 3f;
    [SerializeField] float freezeAfterSpawnTime = 3f;
    [SerializeField] GameObject spawnParticles = null;
    
    [Header("Intro Configurations")]
    [SerializeField] float overviewTime = 3f;
    [SerializeField] float zoomInTime = 4f;
    
    [Header("Cameras")]
    [SerializeField] GameObject introCamera = null;
    [SerializeField] GameObject introCamera2 = null;
    [SerializeField] GameObject gameCamera = null;
    [SerializeField] GameObject deathCamera = null;
    [SerializeField] GameObject killerCamera = null;
    
    [Header("Camera Walls")]
    [SerializeField] GameObject cameraWalls = null;

    [Header("Current round")]
    [SerializeField] int currentRound = 1;
    Player currentPrey = null;
    public Player CurrentPrey { get { return currentPrey; } }
    GamePlayUIController gameLoopUIController;
    [SerializeField] private List<int> preyProbability; //Serialized temporarily to make sure it works properly
    TargetGroupController targetGroupController;
    AudioController audioController;
    SpawnPoint firstSpawnPoint;
    bool isBetweenRounds = true;

    void Start()
    {
        GetReferences();
        StartCoroutine(HandleGameLoop());
    }
    private void GetReferences()
    {
        gameLoopUIController = FindObjectOfType<GamePlayUIController>();
        targetGroupController = FindObjectOfType<TargetGroupController>();
        audioController = FindObjectOfType<AudioController>();
        firstSpawnPoint = GetSpawnPoint();
    }

    IEnumerator HandleGameLoop()
    {
        preyProbability = new List<int> { 0, 1, 2, 3 }; // resets the preyProbability list everytime you play.
        while (currentRound <= numberOfRounds)
        {
            audioController.PlayMenuMusic(false);
            audioController.PlayGameMusic(false);
            audioController.TransitionToMusicOnly();
            ShowPlayers(false);
            ActivateAllPlayers(false);
            yield return StartCoroutine(gameLoopUIController.PreRoundCountdown(startCountDownDuration, players, currentRound));
            HandleRoleSetting();
            targetGroupController.UpdateTargetGroup(players);
            yield return StartCoroutine(gameLoopUIController.PreyReveal(currentPrey, preyRevealDuration));
            audioController.PlayGameMusic(true);
            if (currentRound == 1)
                yield return StartCoroutine(LevelIntro());
            audioController.TransitionToMain();
            HandleStartOfRound();
            yield return StartCoroutine(gameLoopUIController.CountRoundTime(roundDuration));
            audioController.MusicFadeOut();
            HandleEndOfRound();
            yield return StartCoroutine(gameLoopUIController.NextRoundCountdown(players, roundOverDuration, currentRound));
            currentRound++;
        }

        ShowPlayers(false);
        ActivateAllPlayers(false);
        gameLoopUIController.DisplayFinalResults(players);
    }

    private IEnumerator LevelIntro()
    {
        introCamera.SetActive(true);
        yield return new WaitForSeconds(overviewTime);
        introCamera2.GetComponent<CinemachineVirtualCamera>().Follow = firstSpawnPoint.transform;
        introCamera2.SetActive(true);
        yield return new WaitForSeconds(zoomInTime);
        introCamera.SetActive(false);
    }
    
    private void ShowPlayers(bool value)
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(value);
        }
    }

    private void ActivateAllPlayers(bool value)
    {
        foreach (var player in players)
        {
            ActivatePlayer(value, player);
        }
    }

    private static void ActivatePlayer(bool value, Player player)
    {
        player.ResetPlayer();
        player.FreezeInput = !value;
        player.gameObject.SetActive(value);
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
        players[i].SetPrey(value, speed);
        if (value)
        {
            currentPrey = players[i];
            players[i].ScoreIncreaseTimer();
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
        ShowPlayers(true);
        isBetweenRounds = false;
        ActivateAllPlayers(true);
        introCamera2.SetActive(false);
        gameCamera.SetActive(true);
        StartCoroutine(SpawnAllPlayers());
    }
    
    private void HandleEndOfRound()
    {        
        isBetweenRounds = true;
        foreach (Player player in players)
            player.Prey = false;
        ShowPlayers(false);
        StopCoroutine(HandleRespawnOfAllPlayers(null));
        gameCamera.SetActive(false);      
        gameLoopUIController.StopSpawnCountDown();         
        gameLoopUIController.SetKillScreen(null, null, false);
        ActivateAllPlayers(false);
        //CalculateScores();
    }

    IEnumerator SpawnAllPlayers()
    {
        SpawnPoint spawnPoint = currentRound == 1 ? firstSpawnPoint : GetSpawnPoint();
        int hunterSpawnCount = 1;
        foreach (var player in players)
        {
            if (isBetweenRounds)
                break;
            Vector2 spawnPosition;
            player.FreezeInput = true;
            if (player.Prey == true)
            {
                spawnPosition = spawnPoint.spawnPosition[0].transform.position;
                spawnPlayer(spawnPosition, player);
            }
            else
            {
                spawnPosition = spawnPoint.spawnPosition[hunterSpawnCount].transform.position;
                spawnPlayer(spawnPosition, player);
                hunterSpawnCount += 1;
            }
        }
        if (!isBetweenRounds)
            StartCoroutine(gameLoopUIController.SpawnCountDown(freezeAfterSpawnTime));
        yield return new WaitForSeconds(freezeAfterSpawnTime);
        cameraWalls.SetActive(true);
        foreach (var player in players)
            player.FreezeInput = false;
        StartPreyScoreIncrease(true);
    }
    
    private SpawnPoint GetSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Length - 1);
        SpawnPoint spawnPoint = spawnPoints[random].GetComponent<SpawnPoint>();
        return spawnPoint;
    }
    
    private void spawnPlayer(Vector2 spawnPosition, Player player)
    {
        player.SetNewPosition(spawnPosition);
        Instantiate(spawnParticles, new Vector3(spawnPosition.x, spawnPosition.y - 0.5f, 0), Quaternion.identity);
    }

    public void RespawnAllPlayers(Player killer)
    {
        StartCoroutine(HandleRespawnOfAllPlayers(killer));
    }

    public void RemovePushFromPrey(Player deadPlayer)
    {
        foreach (var player in players)
        {
            if(player.Prey)
            {

                if (player.PushedBy != null)
                    if (deadPlayer.name == player.PushedBy.name)
                        player.PushedBy = null;

                if(player.AssistPusher != null)
                    if (deadPlayer.name == player.AssistPusher.name)
                        player.AssistPusher = null;
            }
        }
    }

    private IEnumerator HandleRespawnOfAllPlayers(Player killer)
    {
        StartPreyScoreIncrease(false);  
        foreach (var player in players)
        {
            player.FreezeInput = true;
        }
        yield return StartCoroutine(CameraActionsAtPreyDeath(killer));
        foreach (var player in players)
        {
            player.ResetPlayer();
        }
        
        AudioController.instance.TransitionToMain();
        StartCoroutine(SpawnAllPlayers());
    }

    private void StartPreyScoreIncrease(bool value)
    {
        foreach (var player in players)
        {
            if (player.Prey)
                player.ShouldIncreaseScore = value;
        }
    }

    private void CalculateScores()
    {
        // this method will be used to calculate the score that the prey should get based on how few times they died.
        // in here all special point reward systems will be handled aswell
        GivePacifistReward(); // should we have this?
        TotalAllPlayersScore();
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
            player.ResetPlayer();
            player.TotalScore = 0;
        }

        AssignPlayers.keyboardAssigned = false;
        SceneManager.LoadScene("GardenTest 2.0");
        return;
        
/*        currentRound = 1;
        currentPrey = null;
        gameCamera.SetActive(false);
        StartCoroutine(HandleGameLoop());*/
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

    private IEnumerator CameraActionsAtPreyDeath(Player killer)
    {
        cameraWalls.SetActive(false);
        var cameraAdjustment = new Vector3(0 , 0, 4);
        gameCamera.SetActive(false);
        gameLoopUIController.SetKillScreen(currentPrey, killer, true);
        if (killer == null)
        {
            deathCamera.transform.position = currentPrey.transform.position - cameraAdjustment;
            deathCamera.SetActive(true);
        }
        else
        {
            killerCamera.transform.position = killer.transform.position - cameraAdjustment;
            killerCamera.SetActive(true);
        }
        yield return new WaitForSeconds(respawnDelay);
        deathCamera.SetActive(false);
        killerCamera.SetActive(false);
        gameCamera.SetActive(true);
        gameLoopUIController.SetKillScreen(currentPrey, killer, false);
    }
}

