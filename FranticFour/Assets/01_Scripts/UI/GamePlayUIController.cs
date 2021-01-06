using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayUIController : GamePlayUIDisplay
{
    // add a way to discintively show who is leading in points?.
    // add scores for this round aswell as the total score
    
    static readonly int First = Animator.StringToHash("First");
    static readonly int Last = Animator.StringToHash("Last");
    static readonly int Second = Animator.StringToHash("Second");
    static readonly int Third = Animator.StringToHash("Third");
    static readonly int Reveal = Animator.StringToHash("Reveal");
    GameAudio gameAudio;

    void Start()
    {
        gameAudio = FindObjectOfType<GameAudio>();
    }
    
    public IEnumerator PreRoundCountdown(float duration, int roundNumber)
    {
        round.text = "ROUND " + roundNumber;
        preRoundDisplay.SetActive(true);
        preRevealTexts.SetActive(true);
        round.enabled = true;
        while (duration > 0)
        {
            SetCountDownDisplayNumber(duration, preRoundTime);
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        preRevealTexts.SetActive(false);
    }

    public IEnumerator PreyReveal(Player prey, float duration)
    {
        DisplayThePrey(prey);
        preyRevealColorChange.SetTrigger(Reveal);
        yield return new WaitForSeconds(duration);
        preRoundDisplay.SetActive(false);
    }

    private void DisplayThePrey(Player prey)
    {
        foreach (var animator in preyRevealAnimators)
            animator.SetTrigger(animator.name == prey.name ? "Prey" : "NotPrey");
        preyName.text = prey.name.ToUpper() + " IS THE PREY THIS ROUND!";
    }

    public IEnumerator CountRoundTime(float duration)
    {
        while (duration > 10)
        {
            yield return new WaitForSeconds(timeDecreaseIncrement);
            duration -= timeDecreaseIncrement;
        }
        yield return StartCoroutine(RoundCountDown());
    }

    private IEnumerator RoundCountDown()
    {
        gameAudio.PlaySound("tenSecGong");
        tenSecondsLeftDisplay.SetActive(true);
        yield return new WaitForSeconds(1);
        tenSecondsLeftDisplay.SetActive(false);
        yield return new WaitForSeconds(6);
        roundCountDownDisplay.SetActive(true);
        yield return new WaitForSeconds(3);
        roundCountDownDisplay.SetActive(false);
    }

    public IEnumerator NextRoundCountdown(Player[] players, float duration, int roundNumber)
    {
        SetPlayerRoundScores(players, roundNumber);
        scoreDisplay.SetActive(true);
        yield return new WaitForSeconds(duration / 2);
        AudioController.instance.PlayGameMusic(false);
        foreach (var player in players)
        {
            while (player.RoundScore > 0)
            {
                player.AddRoundScoreToTotalScore();
                gameAudio.PlaySound("point");
                SetPlayerRoundScores(players, roundNumber);
                yield return new WaitForSeconds(0.07f);
            }
        }
        yield return new WaitForSeconds(duration / 2);
        scoreDisplay.SetActive(false);
    }
    
    public IEnumerator SpawnCountDown(float freezeTime)
    {
        countDownDisplay.SetActive(true);
        yield return new WaitForSeconds(freezeTime);
        gameAudio.PlaySound("startShot");
        yield return new WaitForSeconds(freezeTime);
        countDownDisplay.SetActive(false);
    }

    private void SetCountDownDisplayNumber(float duration, TextMeshProUGUI countDownText)
    {
        countDownText.enabled = true;
        float numberToDisplay = (float)System.Math.Round(duration, 2);
        countDownText.text = numberToDisplay.ToString();
    }

    private void SetPlayerRoundScores(Player[] players, int roundNumber)
    {
        roundScoreText.text = "SCORE - ROUND " + roundNumber;
        foreach (var player in players)
        {
            switch (player.gameObject.name)
            {
                case "Duck":
                    SetRoundScoreTexts(player, player1CurrentScore, player1RoundScore);
                    break;
                case "Pig":
                    SetRoundScoreTexts(player, player2CurrentScore, player2RoundScore);
                    break;
                case "Bunny":
                    SetRoundScoreTexts(player, player3CurrentScore, player3RoundScore);
                    break;
                case "Sheep":
                    SetRoundScoreTexts(player, player4CurrentScore, player4RoundScore);
                    break;
            }
        }
    }
    private void SetRoundScoreTexts(Player player, TextMeshProUGUI totalScoreText, TextMeshProUGUI roundScoreText)
    {
        totalScoreText.text = player.TotalScore.ToString();
        roundScoreText.text = player.RoundScore.ToString();
        // player.RoundScore = 0;
    }
    

    public void DisplayFinalResults(Player[] players)
    {
        finalResultDisplay.SetActive(true);
        gameAudio.PlaySound("finalFanfare");
        gameAudio.PlaySound("applause");
        SetPlayerPlacement(players);
        SetResultTexts(players);
    }

    private void SetPlayerPlacement(Player[] players)
    {
        List<int> scoreList = SortPlayerStanding(players);
        foreach (var player in players)
        {
            if (player.TotalScore == scoreList[0])
                player.Placement = 0;
            else if (player.TotalScore == scoreList[1])
                player.Placement = 1;
            else if (player.TotalScore == scoreList[2])
                player.Placement = 2;
            else
                player.Placement = 3;
        }
    }
    private List<int> SortPlayerStanding(Player[] players)
    {
        List<int> playerlist = new List<int>();
        foreach (var player in players)
            if (!playerlist.Contains(player.TotalScore))
                playerlist.Add(player.TotalScore);
        playerlist.Sort();
        playerlist.Reverse();
        return playerlist;
    }

    private void SetResultTexts(Player[] players)
    {
        foreach (var player in players)
            switch (player.gameObject.name)
            {
                case "Duck":
                    SetPlayerResults(player, duckPlacement, duckAnimator, duckTotalScore);
                    break;
                case "Pig":
                    SetPlayerResults(player, pigPlacement, pigAnimator, pigTotalScore);
                    break;
                case "Bunny":
                    SetPlayerResults(player, bunnyPlacement, bunnyAnimator, bunnyTotalScore);
                    break;
                case "Sheep":
                    SetPlayerResults(player, sheepPlacement, sheepAnimator, sheepTotalScore);
                    break;
            }
    }
    private void SetPlayerResults(Player player, SpriteRenderer placement, Animator animator, TextMeshProUGUI score )
    {
        switch (player.Placement)
        {
            case 0:
                animator.SetTrigger(First);
                placement.sprite = podiums[0];
                Instantiate(confetti, placement.transform.position, Quaternion.identity);
                break;
            case 1:
                animator.SetTrigger(Second);
                placement.sprite = podiums[1];
                break;
            case 2:
                animator.SetTrigger(Third);
                placement.sprite = podiums[2];
                break;
            case 3:
                animator.SetTrigger(Last);
                placement.sprite = null;
                break;
        }
        //IF we want total scores to be displayed on "Final Score"-screen:
        //score.text = player.TotalScore.ToString() + " points";
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
        killedByText.text = "ACCIDENTAL DEATH - HUNTERS SCORE 2 POINTS EACH!";
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
        killedByText.text = "PREY GOT KILLED BY " + killer.gameObject.name.ToUpper() + "!";
        switch (killer.gameObject.name)
        {
            case "Duck":
                killerImages[0].SetActive(true);
                break;
            case "Pig":
                killerImages[1].SetActive(true);
                break;
            case "Bunny":
                killerImages[2].SetActive(true);
                break;
            case "Sheep":
                killerImages[3].SetActive(true);
                break;
        }
    }

    public void PlayAgain()
    {
        finalResultDisplay.SetActive(false);
    }

    public void StopSpawnCountDown()
    {
        StopCoroutine(SpawnCountDown(0));
        countDownDisplay.SetActive(false);
    }
}
