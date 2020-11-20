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
            SetControllerKeysXbox();
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
        GetControllerType();
    }

    private void SetControllerKeysXbox()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;

        rightVertical = StringManager.Inputs.rightVertical + playerID; //5th
        rightHorizontal = StringManager.Inputs.rightHorizontal + playerID; //4th

        action1 = StringManager.Inputs.action1 + playerID;
        jump = StringManager.Inputs.jump + playerID;
    }

    private void SetControllerKeysPS4()
    {
        vertical = StringManager.Inputs.vertical + playerID;
        horizontal = StringManager.Inputs.horizontal + playerID;

        rightVertical = StringManager.Inputs.rightVerticalPS4 + playerID; //4th
        rightHorizontal = StringManager.Inputs.rightHorizontalPS4 + playerID; //3th

        action1 = StringManager.Inputs.action1PS4 + playerID;
        jump = StringManager.Inputs.jump + playerID;
    }

    private void GetControllerType()
    {
        string controllerName = "";
        string[] controllersConnected = Input.GetJoystickNames();

        if (playerID < controllersConnected.Length) controllerName = controllersConnected[playerID];

        if (controllerName.Contains("XBOX"))
            SetControllerKeysXbox();
        else if (controllerName.Contains("Wireless Controller"))
            SetControllerKeysPS4();
        else if (controllerName.Contains("Switch??"))
        {
            //Implement nitendo switch controller support
        }
        else
            SetControllerKeysXbox();
    }
}
