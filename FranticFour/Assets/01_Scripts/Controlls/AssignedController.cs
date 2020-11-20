using UnityEngine;

public class AssignedController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)] [SerializeField]
    private int playerID;

    public int PlayerID
    {
        get => playerID;
        set
        {
            playerID = value;
            SetControllerKeys();
        }
    }

    [Header("Assigned controls")] [SerializeField]
    private string vertical;

    [SerializeField] private string horizontal;
    [SerializeField] private string rightVertical;
    [SerializeField] private string rightHorizontal;
    [SerializeField] private string action1;
    [SerializeField] private string jump;

    public string Vertical => vertical;
    public string Horizontal => horizontal;
    public string RightVertical => rightVertical;
    public string RightHorizontal => rightHorizontal;
    public string Action1 => action1;
    public string Jump => jump;

    private void Start()
    {
        SetControllerKeys();
    }

    private void SetControllerKeys()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;
        rightVertical = StringManager.Inputs.rightVertical + playerID;
        rightHorizontal = StringManager.Inputs.rightHorizontal + playerID;
        action1 = StringManager.Inputs.action1 + playerID;
        jump = StringManager.Inputs.jump + playerID;
    }
}