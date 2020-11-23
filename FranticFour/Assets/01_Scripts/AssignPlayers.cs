using UnityEngine;

public class AssignPlayers : MonoBehaviour
{
    [SerializeField] private AssignedController[] players = new AssignedController[4];
    public static bool keyboardAssigned = false;

    private void Awake()
    {
        if (!PassControllersToGame.playersAssigned)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].PlayerID = PassControllersToGame.playerOwnedBy[i];
        }
    }
}
