using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUIController : GamePlayUIDisplay
{
    public IEnumerator PreRoundCountdown(float duration, Player[] players, int roundNumber)
    {
        SetRoundAndPlayerDisplay(players, roundNumber);
        preeRoundDisplay.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            preRoundTime.text = "Time until prey reveal: " + numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preeRoundDisplay.SetActive(false);
    }
    private void SetRoundAndPlayerDisplay(Player[] players, int roundNumber)
    {
        round.text = "Round: " + roundNumber;
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    orangePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Green":
                    greenPlayer.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Purple":
                    purplePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    break;
                case "Cyan":
                    cyanPlayer.text = "Player: " + player.PlayerNumber.ToString();
                    break;
            }
        }
    }

    public IEnumerator PreyCountdown(Player prey, float duration)
    {
        DisplayThePrey(prey);

        preyDisplay.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            base.preyCountdown.text = "Time until round starts: " + numberToDisplay.ToString();
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

        preyNumber.text = preyPlayer + " Is the prey this round";
    }

    public IEnumerator CountRoundTime(float duration)
    {
        roundTime.gameObject.SetActive(true);
        while (duration > 0)
        {
            float numberToDisplay = (float)System.Math.Round(duration, 2);
            roundTime.text = numberToDisplay.ToString();
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        roundTime.gameObject.SetActive(false);

    }

    // add a way to discintively show who is leading in points?.
    // add scores for this round aswell as the total score
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
                    orangeScorePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    orangeCurrentScore.text = "Score: " + player.Score;
                    break;
                case "Green":
                    greenScorePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    greenCurrentScore.text = "Score: " + player.Score;
                    break;
                case "Purple":
                    purpleScorePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    purpleCurrentScore.text = "Score: " + player.Score;
                    break;
                case "Cyan":
                    cyanScorePlayer.text = "Player: " + player.PlayerNumber.ToString();
                    cyanCurrentScore.text = "Score: " + player.Score;
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
            if (player.Score == firstPlace)
            {
                player.Placement = 0;
            }
            else if (player.Score == secondPlace)
            {
                player.Placement = 1;
            }
            else if (player.Score == thirdPlace)
            {
                player.Placement = 2;
            }
            else if (player.Score == fourthPlace)
            {
                player.Placement = 3;
            }
        }

        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    orangeTotalScore.text = "Score: " + player.Score;
                    resultOrangeName.text = "Player: " + player.PlayerNumber;
                    player1Placement.fontSize = placementTextSizes[player.Placement];
                    player1Placement.color = placementColors[player.Placement];
                    player1Placement.text = placements[player.Placement];

                    break;
                case "Green":
                    greenTotalScore.text = "Score: " + player.Score;
                    resultOrangeName.text = "Player: " + player.PlayerNumber;
                    player2Placement.fontSize = placementTextSizes[player.Placement];
                    player2Placement.color = placementColors[player.Placement];
                    player2Placement.text = placements[player.Placement];
                    break;
                case "Purple":
                    purpleTotalScore.text = "Score: " + player.Score;
                    resultPurpleName.text = "Player: " + player.PlayerNumber;
                    player3Placement.fontSize = placementTextSizes[player.Placement];
                    player3Placement.color = placementColors[player.Placement];
                    player3Placement.text = placements[player.Placement];
                    break;
                case "Cyan":
                    cyanTotalScore.text = "Score: " + player.Score;
                    resultCyanName.text = "Player: " + player.PlayerNumber;
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
            playerlist.Add(player.Score);
        }
        playerlist.Sort();
        playerlist.Reverse();
        return playerlist;
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
                        switch (index)
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

    public void PlayAgain()
    {
        finalResultDisplay.SetActive(false);
    }
}
