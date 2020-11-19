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

    [Header("Assigned controls")]
    [SerializeField] private string vertical;
    [SerializeField] private string horizontal;
    [SerializeField] private string action1;
    public string Vertical => vertical;
    public string Horizontal => horizontal;
    public string Action1 => action1;

    private void Start()
    {
        SetControllerKeys();
    }

    private void SetControllerKeys()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;
        action1 = StringManager.Inputs.action1 + playerID;
    }
}
