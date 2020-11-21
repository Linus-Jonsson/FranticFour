using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField] private int[] playerOwnedBy = new int[4];
    [SerializeField] private bool[] playersSelected = new bool[4];
    [SerializeField] private TextMeshPro[] selectedText = new TextMeshPro[4];

    public void TakeNextPlayer(int controllerID)
    {
        DebugLogControllers(); //Debug
        for (int i = 0; i < playerOwnedBy.Length; i++)
        {
            if (playersSelected[i] == true)
                continue;

            playerOwnedBy[i] = controllerID;
            PassControllersToGame.playerOwnedBy[i] = controllerID;
            selectedText[i].text = controllerID.ToString();
            playersSelected[i] = true;
            break;
        }

        //Check if all players is assigned
        for (int i = 0; i < playersSelected.Length; i++)
        {
            if (!playersSelected[i])
                return;
        }

        PassControllersToGame.playersAssigned = true;
        Debug.Log("ALL PLAYERS ASSIGNED. GAME LOADING...");
        LoadGame();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
    //Ladda scener och sätt spelar till rätt

    private void DebugLogControllers() //Debug
    {
        string[] controllersConnected = Input.GetJoystickNames();
        for (int i = 0; i < controllersConnected.Length; i++)
        {
            Debug.Log(controllersConnected[i]);
        }

        Debug.Log("Controllers = " + controllersConnected.Length);
    }
}