using UnityEngine;

public class AssignedController : MonoBehaviour
{
    [Header("Player")]
    [Range(0,3)]
    [SerializeField] private int playerID;

    public int PlayerID
    {
        get => playerID;
        set { playerID = value; SetControllerKeys();}
    }

    [Header("Assigned controlls")]
    [SerializeField] private string vertical;
    [SerializeField] private string horizontal;
    [SerializeField] private string push;
    public string Vertical => vertical;
    public string Horizontal => horizontal;
    public string Push => push;

    private void Start()
    {
        SetControllerKeys();
    }

    private void SetControllerKeys()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;
        push = StringManager.Inputs.push + playerID;
    }
}
