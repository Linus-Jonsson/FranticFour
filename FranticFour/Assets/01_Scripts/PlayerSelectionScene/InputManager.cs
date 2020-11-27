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
    }

    public void CheckPlayers()
    {
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

    public void SetTextAssigned(int _selected, int _id)
    {
        playerOwnedBy[_selected] = _id;
        playersSelected[_selected] = true;
        PassControllersToGame.playerOwnedBy[_selected] = _id;
        selectedText[_selected].text = $"[Player {_id}]";
    }

    public void SelectNext(int _controllerId, ref int _selected, int ompaLompaBompa, int ompaLompaKonka)
    {
        int m_nextSelect = _selected;
        int[,] ompaLompaaDompa = new int[2,2] {{0,2}, {1,3}};
        int ompaLompaRompa = 0;
        int ompaLompaSompa = 0;
        int ompaLompaOompa = 0;
        int ompaLompaDompa = 0;
        ompaLompaKonka *= -1;

        switch (_selected)
        {
            //Robin du får fixa detta på måndag, trevlig helg och lycka till nästa vecka!
            //Mvh Robin på en fredags morgon
            case 0:
                ompaLompaRompa = 0; //Fuck u rider
                ompaLompaSompa = 0;
                break;
            case 1:
                ompaLompaRompa = 0; //Fuck u rider
                ompaLompaSompa = 1;
                break;
            case 2:
                ompaLompaRompa = 1; //Fuck u rider
                ompaLompaSompa = 0;
                break;
            case 3:
                ompaLompaRompa = 1; //Fuck u rider
                ompaLompaSompa = 1;
                break;
        }

        switch (ompaLompaBompa)
        {
            case -1:
                ompaLompaRompa = 0;
                break;
            case 0:
                break;
            case 1:
                ompaLompaRompa = 1;
                break;
        }

        switch (ompaLompaKonka)
        {
            case -1:
                ompaLompaSompa = 0;
                break;
            case 0:
                break;
            case 1:
                ompaLompaSompa = 1;
                break;
        }
        
        m_nextSelect = ompaLompaaDompa[ompaLompaSompa, ompaLompaRompa]; //Fuck u rider

        if (m_nextSelect == _selected)
            return;
        //Debug.Log(m_nextSelect + "|Y" + ompaLompaSompa + "|X" + ompaLompaRompa + "|" + ompaLompaaDompa[ompaLompaRompa, ompaLompaSompa]);

        //Om platsen redan används
        if (PassControllersToGame.playerOwnedBy[m_nextSelect] < 4)
            selectedText[m_nextSelect].text =
                $"[Player {PassControllersToGame.playerOwnedBy[m_nextSelect]}] {_controllerId.ToString()}";
        else
            selectedText[m_nextSelect].text = _controllerId.ToString();

        //Om platsen redan används flytt
        if (PassControllersToGame.playerOwnedBy[_selected] < 4)
            selectedText[_selected].text = $"[Player {PassControllersToGame.playerOwnedBy[_selected]}]";
        else
            selectedText[_selected].text = "";
        
        _selected = m_nextSelect;
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