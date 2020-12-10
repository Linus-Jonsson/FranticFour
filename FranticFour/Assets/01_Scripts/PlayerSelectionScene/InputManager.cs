using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] internal readonly int MAX_PLAYERS = 4;
    [SerializeField] private string sceneToLoad = "GardenTest";
    [SerializeField] private int[] playerOwnedBy = new int[4];
    [SerializeField] public bool[] playersSelected = new bool[4]; //Todo Dod set to private or removed
    [SerializeField] public bool[] canSelect = new bool[4]; //Todo Dod set to private or removed
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private GameObject sceneLoader;
    private HighligtSet highligtSet = null;

    private void Start()
    {
        highligtSet = GetComponent<HighligtSet>();
    }

    public void CheckPlayers()
    {
        //Check if all players is assigned
        for (int i = 0; i < playersSelected.Length; i++)
        {
            if (!playersSelected[i])
                return;
        }
        
        //If assigned, the GAME SCENE will load from PassControllersToGame and not assign new values
        PassControllersToGame.playersAssigned = true;
        sceneLoader.SetActive(true);
    }

    public void SelectPlayer(int _controllerID)
    {
        playersSelected[_controllerID] = true;
        CheckPlayers();
    }
    
    public void DeselectPlayer(int _controllerID)
    {
        playersSelected[_controllerID] = false;
    }

    public MyPlayer GetPlayer(int _controllerID)
    {
        if (_controllerID >= MAX_PLAYERS)
            return null;

        playerOwnedBy[_controllerID] = _controllerID;
        return players[_controllerID].GetComponent<MyPlayer>();
    }
}