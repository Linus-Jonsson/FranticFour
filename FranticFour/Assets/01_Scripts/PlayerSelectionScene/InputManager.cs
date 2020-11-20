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
        for (int i = 0; i < playerOwnedBy.Length; i++)
        {
            if (playersSelected[i] == true)
                continue;
            
            playerOwnedBy[i] = controllerID;
            selectedText[i].text = controllerID.ToString();
            playersSelected[i] = true;
            return;
        }
    }
    //Ladda scener och sätt spelar till rätt
}
