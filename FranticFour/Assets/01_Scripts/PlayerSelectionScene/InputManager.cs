using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private int[] playerOwnedBy = new int[4];
    [SerializeField] private bool[] playersSelected = new bool[4];

    public void TakeNextPlayer(int controllerID)
    {
        for (int i = 0; i < playerOwnedBy.Length; i++)
        {
            if (playersSelected[i] == true)
                continue;
            
            playerOwnedBy[i] = controllerID;
            playersSelected[i] = true;
            return;
        }
    }
}
