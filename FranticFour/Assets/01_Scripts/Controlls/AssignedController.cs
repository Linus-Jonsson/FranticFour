using UnityEngine;

public class AssignedController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)]
    [SerializeField] private int playerID;
    [SerializeField] private bool usesMouse;

    public int PlayerID
    {
        get => playerID;
        set
        {
            playerID = value;
            GetControllerType();
        }
    }
    
    public bool UsesMouse
    {
        get => usesMouse;
        private set => usesMouse = value;
    }
    

    [Header("Assigned controls")]
    [SerializeField] private string vertical;
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

    private void SetControllerKeyboardAndMouse()
    {
        usesMouse = true;
        
        vertical = StringManager.Inputs.verticalKeyboard;
        horizontal = StringManager.Inputs.horizontalKeyboard;

        rightVertical = vertical;
        rightHorizontal = horizontal;

        action1 = StringManager.Inputs.action1Keyboard;
        jump = StringManager.Inputs.jumpKeboard;
    }

    private void GetControllerType()
    {
        string controllerName = "";
        string[] controllersConnected = Input.GetJoystickNames();

        if (playerID < controllersConnected.Length)
            controllerName = controllersConnected[playerID];
        
        controllerName = controllerName.ToLower();
        
        if (controllerName.Contains("xbox"))
            SetControllerKeysXbox();
        else if (controllerName.Contains("wireless controller"))
            SetControllerKeysPS4();
        else if (controllerName.Contains("Switch??"))
        {
            //Implement nitendo switch controller support
        }
        else if(PassControllersToGame.isKeyboardUsed && PlayerID == PassControllersToGame.keyBoardOwnedBy)
        {
            SetControllerKeyboardAndMouse();
        }
        else if (AssignPlayers.keyboardAssigned == false && 1 == 0) //Debug
        {
            AssignPlayers.keyboardAssigned = true;
            SetControllerKeyboardAndMouse();
        }
        else
        {
            SetControllerKeysXbox();   
        }
    }
}
