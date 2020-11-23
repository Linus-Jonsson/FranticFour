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

    [SerializeField] TextMeshProUGUI WinnerText = null;


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

    // add a way to discintively show who is leading in points.
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
                    orangePlayerScoreText.text = "Score: " + player.Score;
                    break;
                case "Green":
                    greenScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    greenPlayerScoreText.text = "Score: " + player.Score;
                    break;
                case "Purple":
                    purpleScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    purplePlayerScoreText.text = "Score: " + player.Score;
                    break;
                case "Cyan":
                    cyanScorePlayerText.text = "Player: " + player.PlayerNumber.ToString();
                    cyanPlayerScoreText.text = "Score: " + player.Score;
                    break;
            }
        }
    }

    public void DisplayFinalResults(Player winner, Player[] players)
    {
        finalResultDisplay.SetActive(true);
        WinnerText.text = "The Winner is " + winner.gameObject.name + "\n" + "Congratulations!";
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Orange":
                    finalResultOrangePlayerScoreText.text = "Score: " + player.Score;
                    finalResultOrangePlayerText.text = "Player: " + player.PlayerNumber;
                    break;
                case "Green":
                    finalResultGreenPlayerScoreText.text = "Score: " + player.Score;
                    finalResultGreenPlayerText.text = "Player: " + player.PlayerNumber;
                    break;
                case "Purple":
                    finalResultPurplePlayerScoreText.text = "Score: " + player.Score;
                    finalResultPurplePlayerText.text = "Player: " + player.PlayerNumber;
                    break;
                case "Cyan":
                    finalResultCyanPlayerScoreText.text = "Score: " + player.Score;
                    finalResultCyanPlayerText.text = "Player: " + player.PlayerNumber;
                    break;
            }
        }
        finalResultDisplay.SetActive(true);
    }
    public void PlayAgain()
    {
        finalResultDisplay.SetActive(false);
    }
}
