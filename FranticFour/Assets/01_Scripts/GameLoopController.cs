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

    int currentRound = 0;
    void Start()
    {
        StartCoroutine(HandleGameLoop());
    }

    IEnumerator HandleGameLoop()
    {
        while(currentRound < numberOfRounds)
        {
            print("Current round: " + (currentRound + 1));
            yield return new WaitForSeconds(startCountDownDuration);
            SetPrey(); // add the code to handle the reveal of the player in this function.
            yield return new WaitForSeconds(preyRevealDuration);
            // add all code relevant to handling countdown of time here to display for players
            print("Starting round");
            yield return new WaitForSeconds(roundDuration);
            print("Round over sharing scores");
            // make players unable to move (disable all players?)
            DisplayScores();
            yield return new WaitForSeconds(roundOverDuration);
            SetPlayerPositions();
            print("Starting new round");
            currentRound++;
        }
        // proper display of winner and the option to start a new game or go to main menu here
        print("Game is over and the winner is: " + leader.gameObject.name);
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

        foreach (var player in players)
        {
            if (leader == null && player.Score > 0)
                leader = player;
            else if (player.Score > leader.Score)
                leader = player;

            print("Player: " + player.PlayerNumber + "\n" + "Score: " + player.Score);
        }
        if(leader != null)  
        print("Current score leader is: " + leader.gameObject.name);
    }


}
