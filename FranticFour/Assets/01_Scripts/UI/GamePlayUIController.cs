using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayUIController : GamePlayUIDisplay
{
    // add a way to discintively show who is leading in points?.
    // add scores for this round aswell as the total score

    public IEnumerator PreRoundCountdown(float duration, Player[] players, int roundNumber)
    {
        SetRoundAndPlayerDisplay(players, roundNumber);
        preeRoundDisplay.SetActive(true);
        while (duration > 0)
        {
            SetCountDownDisplayNumber(duration, "Prey revealed in: ", preRoundTime);
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preeRoundDisplay.SetActive(false);
    }
    private void SetRoundAndPlayerDisplay(Player[] players, int roundNumber)
    {
        round.text = "Round: " + roundNumber;
        foreach (var player in players)
            SetPlayerText(player);
    }
    private void SetPlayerText(Player player)
    {
        switch (player.name)
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

    public IEnumerator PreyCountdown(Player prey, float duration)
    {
        DisplayThePrey(prey);
        preyDisplay.SetActive(true);
        while (duration > 0)
        {
            SetCountDownDisplayNumber(duration, "Round starts in: ", preyCountdown);
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preyDisplay.SetActive(false);
    }

    private void DisplayThePrey(Player prey)
    {
        foreach (var image in preyImages)
            image.SetActive(false);
        TurnPreyOn(prey);
        preyNumber.text = prey.name + " Is the prey this round";
    }
    private void TurnPreyOn(Player prey)
    {
        switch (prey.name)
        {
            case "Orange":
                preyImages[0].SetActive(true);
                break;
            case "Green":
                preyImages[1].SetActive(true);
                break;
            case "Purple":
                preyImages[2].SetActive(true);
                break;
            case "Cyan":
                preyImages[3].SetActive(true);
                break;
        }
    }

    public IEnumerator CountRoundTime(float duration)
    {
        roundTime.gameObject.SetActive(true);
        while (duration > 0)
        {
            SetCountDownDisplayNumber(duration, "", roundTime);
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        roundTime.gameObject.SetActive(false);
    }

    public IEnumerator NextRoundCountdown(Player[] players, float duration, int roundNumber)
    {
        SetPlayerRoundScores(players, roundNumber);
        scoreDisplay.SetActive(true);
        while (duration > 0)
        {
            SetCountDownDisplayNumber(duration, "Next round begins in: ", nextRoundInText);
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        scoreDisplay.SetActive(false);
    }

    private void SetCountDownDisplayNumber(float duration, string message,TextMeshProUGUI countDownText)
    {
        float numberToDisplay = (float)System.Math.Round(duration, 2);
        countDownText.text = message + numberToDisplay.ToString();
    }

    private void SetPlayerRoundScores(Player[] players, int roundNumber)
    {
        roundScoreText.text = "Score standings after round: " + roundNumber;
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    SetRoundScoreTexts(player, orangeScorePlayer, orangeCurrentScore);
                    break;
                case "Green":
                    SetRoundScoreTexts(player, greenScorePlayer, greenCurrentScore);
                    break;
                case "Purple":
                    SetRoundScoreTexts(player, purpleScorePlayer, purpleCurrentScore);
                    break;
                case "Cyan":
                    SetRoundScoreTexts(player, cyanScorePlayer, cyanCurrentScore);
                    break;
            }
        }
    }
    private void SetRoundScoreTexts(Player player, TextMeshProUGUI nameText, TextMeshProUGUI scoreText)
    {
        nameText.text = "Player: " + player.PlayerNumber.ToString();
        scoreText.text = "Score: " + player.Score;
    }

    public void DisplayFinalResults(Player[] players)
    {
        finalResultDisplay.SetActive(true);
        SetPlayerPlacement(players);
        SetResultTexts(players);
        finalResultDisplay.SetActive(true);
    }

    private void SetPlayerPlacement(Player[] players)
    {
        List<int> scoreList = SortPlayerStanding(players);
        foreach (var player in players)
        {
            if (player.Score == scoreList[0])
                player.Placement = 0;
            else if (player.Score == scoreList[1])
                player.Placement = 1;
            else if (player.Score == scoreList[2])
                player.Placement = 2;
            else
                player.Placement = 3;
        }
    }
    private List<int> SortPlayerStanding(Player[] players)
    {
        List<int> playerlist = new List<int>();
        foreach (var player in players)
            if (!playerlist.Contains(player.Score))
                playerlist.Add(player.Score);
        playerlist.Sort();
        playerlist.Reverse();
        return playerlist;
    }

    private void SetResultTexts(Player[] players)
    {
        foreach (var player in players)
            switch (player.gameObject.name)
            {
                case "Orange":
                    SetPlayerResults(player, orangePlacement, resultOrangeName, orangeTotalScore);
                    break;
                case "Green":
                    SetPlayerResults(player, greenPlacement, resultGreenName, greenTotalScore);
                    break;
                case "Purple":
                    SetPlayerResults(player, purplePlacement, resultPurpleName, purpleTotalScore);
                    break;
                case "Cyan":
                    SetPlayerResults(player, cyanPlacement, resultCyanName, cyanTotalScore);
                    break;
            }
    }
    private void SetPlayerResults(Player player, TextMeshProUGUI placement, TextMeshProUGUI name, TextMeshProUGUI score )
    {
        score.text = "Score: " + player.Score;
        name.text = "Player: " + player.PlayerNumber;
        placement.fontSize = placementTextSizes[player.Placement];
        placement.color = placementColors[player.Placement];
        placement.text = placements[player.Placement];
    }

    public void SetKillScreen(Player prey, Player killer, bool value)
    {
        if (value)
        {
            DisableImagesAndCenterTransform();
            if (killer == null)
                HandlePreyMisstep(prey);
            else
                SetWhoHuntedPrey(killer);
        }
        killScreenDisplay.SetActive(value);
    }

    private void HandlePreyMisstep(Player prey)
    {
        killedByText.text = "Prey made a sudden lapse in judgement, everyone else gets a point";
        int index = 0;
        foreach (var image in killerImages)
        {
            if (image.name == prey.name)
                continue;
            image.SetActive(true);
            SetImageTransform(index, image);
            index++;
        }
    }
    private void SetImageTransform(int index, GameObject image)
    {
        switch (index)
        {
            case 0:
                image.transform.position = hunterImageTransforms[0].position;
                break;
            case 1:
                image.transform.position = hunterImageTransforms[1].position;
                break;
            case 2:
                image.transform.position = hunterImageTransforms[2].position;
                break;
        }
    }

    private void DisableImagesAndCenterTransform()
    {
        foreach (var image in killerImages)
        {
            image.transform.position = hunterImageTransforms[1].position;
            image.SetActive(false);
        }
    }

    private void SetWhoHuntedPrey(Player killer)
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

    public void PlayAgain()
    {
        finalResultDisplay.SetActive(false);
    }
}
