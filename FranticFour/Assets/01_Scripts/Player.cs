using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    int score = 0;
    public int Score { get { return score; } set { score = value; } }

    int playerNumber = 0;
    public int PlayerNumber { get { return playerNumber; } }

    int placement = 0;
    public int Placement { get{ return placement; } set{ placement = value; } }

    bool prey = false;
    public bool Prey { get { return prey; } set { prey = value; } }

    [SerializeField] int numberOfDeaths = 0;
    public int NumberOfDeaths { get { return numberOfDeaths; } set { numberOfDeaths = value; } }

    int huntersKilled = 0;
    public int HuntersKilled { get { return huntersKilled; } set { huntersKilled = value; } }

    bool freezeInput = false;
    public bool FreezeInput { get { return freezeInput; } set { freezeInput = value; } }

    //[SerializeField] string playerName = ""; // not currently in use
    Player pushedBy = null;
    public Player PushedBy { get { return pushedBy; } set { pushedBy = value; } }

    private void Awake()
    {
        playerNumber = GetComponent<AssignedController>().PlayerID + 1;
    }

    public void IncreaseScore(int scoreChange)
    {
        score += scoreChange;
    }
}