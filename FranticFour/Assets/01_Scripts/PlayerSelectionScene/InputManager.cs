using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Game";
    [SerializeField] private int[] playerOwnedBy = new int[4];
    [SerializeField] public bool[] playersSelected = new bool[4];
    [SerializeField] public bool[] canSelect = new bool[4];
    [SerializeField] private TextMeshPro[] selectedText = new TextMeshPro[4];

    public void CheckPlayers()
    {
        //Check if all players is assigned
        for (int i = 0; i < playersSelected.Length; i++)
        {
            if (!playersSelected[i])
                return;
        }
        
        PassControllersToGame.playersAssigned = true; //If assigned, the game scene will load from PassControllersToGame
        LoadGame();
    }
    
    private void LoadGame()
    {
        //Todo make transition
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SetTextAssigned(int _selected, int _id)
    {
        playerOwnedBy[_selected] = _id;
        playersSelected[_selected] = true;
        PassControllersToGame.playerOwnedBy[_selected] = _id;
        selectedText[_selected].text = $"[Player {_id}]";
    }

    public void SelectNext(int _controllerId, ref int _selected, int _inputX, int _inputY)
    {
        int m_nextSelect = _selected;
        int[][] m_nextSelection = new[] {new int[] {0,2}, new int[] {1,3}};
        int m_nextX = 0;
        int m_nextY = 0;
        _inputY *= -1;

        //Converts current selected pos to jagged array positions
        switch (_selected)
        {
            case 0:
                m_nextX = 0;
                m_nextY = 0;
                break;
            case 1:
                m_nextX = 0;
                m_nextY = 1;
                break;
            case 2:
                m_nextX = 1;
                m_nextY = 0;
                break;
            case 3:
                m_nextX = 1;
                m_nextY = 1;
                break;
        }

        switch (_inputX)
        {
            case -1:
                m_nextX = 0;
                break;
            case 0:
                break;
            case 1:
                m_nextX = 1;
                break;
        }

        switch (_inputY)
        {
            case -1:
                m_nextY = 0;
                break;
            case 0:
                break;
            case 1:
                m_nextY = 1;
                break;
        }
        
        m_nextSelect = m_nextSelection[m_nextY][m_nextX]; //Gets int value from array

        if (m_nextSelect == _selected)
            return;

        //If the spot is already in use
        if (PassControllersToGame.playerOwnedBy[m_nextSelect] < 4)
            selectedText[m_nextSelect].text =
                $"[Player {PassControllersToGame.playerOwnedBy[m_nextSelect]}] {_controllerId.ToString()}"; //Todo Remake the visual of slection
        else
            selectedText[m_nextSelect].text = _controllerId.ToString();

        //If the spot to move to is already in use
        if (PassControllersToGame.playerOwnedBy[_selected] < 4)
            selectedText[_selected].text = $"[Player {PassControllersToGame.playerOwnedBy[_selected]}]"; //Todo Remake the visual of slection
        else
            selectedText[_selected].text = "";
        
        _selected = m_nextSelect;
    }
}