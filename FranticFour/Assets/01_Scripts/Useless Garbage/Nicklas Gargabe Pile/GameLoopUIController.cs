﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoopUIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI roundTimeText = null;

    [SerializeField] float timeDecreaseIncrement = 0.1f;


    [Header("Pree Prey Reveal Display Configuration")]
    [SerializeField] GameObject preeRoundDisplay = null;

    [SerializeField] TextMeshProUGUI preRoundTimeText = null;
    [SerializeField] TextMeshProUGUI roundText = null;

    [SerializeField] TextMeshProUGUI orangePlayerText = null;
    [SerializeField] TextMeshProUGUI greenPlayerText = null;
    [SerializeField] TextMeshProUGUI purplePlayerText = null;
    [SerializeField] TextMeshProUGUI cyanPlayerText = null;

    [Header("Prey Display Configuration")]
    [SerializeField] GameObject preyDisplay = null;
    [SerializeField] TextMeshProUGUI preyNumberText = null;
    [SerializeField] TextMeshProUGUI preyCountdownText = null;
    [SerializeField] GameObject orangePrey = null;
    [SerializeField] GameObject greenPrey = null;
    [SerializeField] GameObject purplePrey = null;
    [SerializeField] GameObject cyanPrey = null;

    [Header("Score Display Configuration")]
    [SerializeField] GameObject scoreDisplay = null;

    [SerializeField] TextMeshProUGUI orangeScorePlayerText = null;
    [SerializeField] TextMeshProUGUI orangePlayerScoreText = null;

    [SerializeField] TextMeshProUGUI greenScorePlayerText = null;
    [SerializeField] TextMeshProUGUI greenPlayerScoreText = null;

    [SerializeField] TextMeshProUGUI purpleScorePlayerText = null;
    [SerializeField] TextMeshProUGUI purplePlayerScoreText = null;

    [SerializeField] TextMeshProUGUI cyanScorePlayerText = null;
    [SerializeField] TextMeshProUGUI cyanPlayerScoreText = null;

    [SerializeField] TextMeshProUGUI nextRoundInText = null;
    [SerializeField] TextMeshProUGUI roundScoreText = null;

    [Header("Final result display configuration")]
    [SerializeField] GameObject finalResultDisplay = null;
    [SerializeField] TextMeshProUGUI finalResultOrangePlayerScoreText = null;
    [SerializeField] TextMeshProUGUI finalResultOrangePlayerText = null;
    [SerializeField] TextMeshProUGUI finalResultGreenPlayerScoreText = null;
    [SerializeField] TextMeshProUGUI finalResultGreenPlayerText = null;
    [SerializeField] TextMeshProUGUI finalResultPurplePlayerScoreText = null;
    [SerializeField] TextMeshProUGUI finalResultPurplePlayerText = null;
    [SerializeField] TextMeshProUGUI finalResultCyanPlayerScoreText = null;
    [SerializeField] TextMeshProUGUI finalResultCyanPlayerText = null;

    [SerializeField] TextMeshProUGUI player1Placement = null;
    [SerializeField] TextMeshProUGUI player2Placement = null;
    [SerializeField] TextMeshProUGUI player3Placement = null;
    [SerializeField] TextMeshProUGUI player4Placement = null;

    [SerializeField] int[] placementTextSizes = new int[4];
    [SerializeField] Color[] placementColors = new Color[4];
    [SerializeField] string[] placements = new string[4];

    [Header("Prey kill screen configuration")]
    [SerializeField] GameObject killScreenDisplay = null;
    [SerializeField] TextMeshProUGUI killedByText = null;
    [SerializeField] GameObject[] killerImages = null;
    [SerializeField] Transform hunter1Transform = null;
    [SerializeField] Transform hunter2Transform = null;
    [SerializeField] Transform hunter3Transform = null;

    public IEnumerator PreRoundCountdown(float duration, Player[] players, int roundNumber)
    {
        SetRoundAndPlayerDisplay(players, roundNumber);
        preeRoundDisplay.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            preRoundTimeText.text = "Time until prey reveal: " + numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preeRoundDisplay.SetActive(false);
    }
    private void SetRoundAndPlayerDisplay(Player[] players, int roundNumber)
    {
        roundText.text = "Round: " + roundNumber;
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    orangePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Green":
                    greenPlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Purple":
                    purplePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Cyan":
                    cyanPlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    break;
            }
        }
    }

    public IEnumerator preyCountdown(Player prey, float duration)
    {
        DisplayThePrey(prey);

        preyDisplay.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            preyCountdownText.text = "Time until round starts: " + numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preyDisplay.SetActive(false);
    }
    private void DisplayThePrey(Player prey)
    {
        orangePrey.SetActive(false);
        greenPrey.SetActive(false);
        purplePrey.SetActive(false);
        cyanPrey.SetActive(false);

        string preyPlayer = prey.gameObject.name;

        switch (preyPlayer)
        {
            case "Orange":
                orangePrey.SetActive(true);
                break;
            case "Green":
                greenPrey.SetActive(true);
                break;
            case "Purple":
                purplePrey.SetActive(true);
                break;
            case "Cyan":
                cyanPrey.SetActive(true);
                break;
        }

        preyNumberText.text = preyPlayer + " Is the prey this round";
    }

    public IEnumerator CountRoundTime(float duration)
    {
        roundTimeText.gameObject.SetActive(true);
        while(duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            roundTimeText.text = numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        roundTimeText.gameObject.SetActive(false);

    }

    // add a way to discintively show who is leading in points?.
    public IEnumerator NextRoundCountdown(Player[] players, float duration, int roundNumber)
    {
        SetPlayerRoundScores(players, roundNumber);

        scoreDisplay.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            nextRoundInText.text = "Next round begins in: " + numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        scoreDisplay.SetActive(false);
    }
    private void SetPlayerRoundScores(Player[] players, int roundNumber)
    {
        roundScoreText.text = "Score round: " + roundNumber;
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    orangeScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    orangePlayerScoreText.text = "Score: " + player.TotalScore;
                    break;
                case "Green":
                    greenScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    greenPlayerScoreText.text = "Score: " + player.TotalScore;
                    break;
                case "Purple":
                    purpleScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    purplePlayerScoreText.text = "Score: " + player.TotalScore;
                    break;
                case "Cyan":
                    cyanScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    cyanPlayerScoreText.text = "Score: " + player.TotalScore;
                    break;
            }
        }
    }

    public void DisplayFinalResults(Player[] players)
    {
        finalResultDisplay.SetActive(true);

        List<int> scoreList = SortPlayerStanding(players);
        int firstPlace = 0;
        int secondPlace = 0;
        int thirdPlace = 0;
        int fourthPlace = 0;
        foreach (var score in scoreList)
        {
            if (score >= firstPlace)
                firstPlace = score;
            else if (score >= secondPlace)
                secondPlace = score;
            else if (score >= thirdPlace)
                thirdPlace = score;
            else
                fourthPlace = score;
        }

        foreach (var player in players)
        {
            if (player.TotalScore == firstPlace)
            {
                player.Placement = 0;
            }
            else if (player.TotalScore == secondPlace)
            {
                player.Placement = 1;
            }
            else if (player.TotalScore == thirdPlace)
            {
                player.Placement = 2;
            }
            else if (player.TotalScore == fourthPlace)
            {
                player.Placement = 3;
            }
        }

        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    finalResultOrangePlayerScoreText.text = "Score: " + player.TotalScore;
                    finalResultOrangePlayerText.text = "Player: " + player.PlayerNumber;
                    player1Placement.fontSize = placementTextSizes[player.Placement];
                    player1Placement.color = placementColors[player.Placement];
                    player1Placement.text = placements[player.Placement];

                    break;
                case "Green":
                    finalResultGreenPlayerScoreText.text = "Score: " + player.TotalScore;
                    finalResultGreenPlayerText.text = "Player: " + player.PlayerNumber;
                    player2Placement.fontSize = placementTextSizes[player.Placement];
                    player2Placement.color = placementColors[player.Placement];
                    player2Placement.text = placements[player.Placement];
                    break;
                case "Purple":
                    finalResultPurplePlayerScoreText.text = "Score: " + player.TotalScore;
                    finalResultPurplePlayerText.text = "Player: " + player.PlayerNumber;
                    player3Placement.fontSize = placementTextSizes[player.Placement];
                    player3Placement.color = placementColors[player.Placement];
                    player3Placement.text = placements[player.Placement];
                    break;
                case "Cyan":
                    finalResultCyanPlayerScoreText.text = "Score: " + player.TotalScore;
                    finalResultCyanPlayerText.text = "Player: " + player.PlayerNumber;
                    player4Placement.fontSize = placementTextSizes[player.Placement];
                    player4Placement.color = placementColors[player.Placement];
                    player4Placement.text = placements[player.Placement];
                    break;
            }
        }
        finalResultDisplay.SetActive(true);
    }

    private List<int> SortPlayerStanding(Player[] players)
    {
        List<int> playerlist = new List<int>();
        foreach (var player in players)
        {
            playerlist.Add(player.TotalScore);
        }
        playerlist.Sort();
        playerlist.Reverse();
        return playerlist;
    }

    public void PlayAgain()
    {
        finalResultDisplay.SetActive(false);
    }

    public void SetKillScreen(Player prey, Player killer, bool value)
    {
        if (value)
        {
            foreach (var image in killerImages)
            {
                image.transform.position = hunter2Transform.position;
                image.SetActive(false);
            }
            if (killer == null)
            {
                killedByText.text = "Prey made a sudden lapse in judgement, everyone else gets a point";
                int index = 0;
                foreach (var image in killerImages)
                {
                    image.SetActive(true);
                    switch (prey.gameObject.name)
                    {
                        case "Orange":
                            killerImages[0].SetActive(false);
                            break;
                        case "Green":
                            killerImages[1].SetActive(false);
                            break;
                        case "Purple":
                            killerImages[2].SetActive(false);
                            break;
                        case "Cyan":
                            killerImages[3].SetActive(false);
                            break;
                    }
                    if (image.activeSelf)
                    {
                        switch(index)
                        {
                            case 0:
                                image.transform.position = hunter1Transform.position;
                                break;
                            case 1:
                                image.transform.position = hunter2Transform.position;
                                break;
                            case 2:
                                image.transform.position = hunter3Transform.position;
                                break;
                        }
                        index++;
                    }
                }
            }
            else
            {
                killedByText.text = "The prey got hunted by Player " + killer.PlayerNumber;
                switch (killer.gameObject.name)
                {
                    case "Orange":
                        killerImages[0].SetActive(true);
                        break;
                    case "Green":
                        killerImages[1].SetActive(true);
                        break;
                    case "Purple":
                        killerImages[2].SetActive(true);
                        break;
                    case "Cyan":
                        killerImages[3].SetActive(true);
                        break;
                }
            }
        }
        killScreenDisplay.SetActive(value);
    }
}