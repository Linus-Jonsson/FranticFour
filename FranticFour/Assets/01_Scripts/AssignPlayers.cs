using UnityEngine;

//Assigns the player from playerSelect
public class AssignPlayers : MonoBehaviour
{
    [SerializeField] private AssignedController[] players = new AssignedController[4];
    public static bool keyboardAssigned = false;

    private void AssignPlayersCharacters()
    {
        if (!PassControllersToGame.playersAssigned)
        {
            foreach (var player in players)
            {
                player.Init();
            }
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < players.Length; i++)
            players[i].PlayerID = PassControllersToGame.playerOwnedBy[i];

        foreach (var player in players)
            player.Init();

        Destroy(gameObject);
    }

    private void Update()
    {
        AssignPlayersCharacters();
    }
}
