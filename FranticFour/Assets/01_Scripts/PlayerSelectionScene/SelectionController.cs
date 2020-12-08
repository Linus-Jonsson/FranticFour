using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)]
    [SerializeField] private int CONTROLLER_ID = 0;
    [SerializeField] private bool hasControllerJoined;
    [SerializeField] private MyPlayer myPlayer;

    [Header("Assigned controls")]
    [SerializeField] private string action1;
    [SerializeField] private string aButton;
    [SerializeField] private InputManager playerHandler;

    private float deadZoneX = 0f;
    private readonly float deadZoneAction = 0.5f;
    private readonly float deadZoneRight = 0.04f;
    private readonly float deadZoneLeft = 0.65f;
    private string rightHorizontal = null;
    private string horizontal = null;
    private bool inputBool = false;
    private bool isSelecting = false;
    private bool playerSelected = false;

    private void Start()
    {
        bool checkFailSafe = false;
        ControllerCheck(ref checkFailSafe);

        if (checkFailSafe)
        {
            Destroy(gameObject);
            return;
        }
        playerHandler = FindObjectOfType<InputManager>();
        MapController();
        myPlayer = playerHandler.GetPlayer(CONTROLLER_ID);
    }

    private void ControllerCheck(ref bool _failSafe)
    {
        //Check if controllers are connected
        string[] joystickNames = Input.GetJoystickNames();
        if (joystickNames.Length == 1 && joystickNames[0] == string.Empty)
            joystickNames = null;

        if ((joystickNames == null || CONTROLLER_ID > joystickNames.Length) && CONTROLLER_ID != 0)
            _failSafe = true;
    }

    private void Update()
    {
        if (!hasControllerJoined && (Input.GetAxis(action1) >= 1 - deadZoneAction || Input.GetButtonUp(aButton))) //For PS4, Xbox and Keyboard
            ControllerJoin();
        else if (hasControllerJoined && !isSelecting && (Input.GetAxis(action1) >= 1 - deadZoneAction || Input.GetButtonUp(aButton))) //For PS4, Xbox and Keyboard
            ControllerSelectPlayer();
        else if (isSelecting && Input.GetAxis(action1) <= 0)
            isSelecting = false;

        float m_inputX = Input.GetAxis(rightHorizontal);
        float m_inputX2 = Input.GetAxis(horizontal);
        
        if (hasControllerJoined && !playerSelected && (m_inputX != 0 || m_inputX2 != 0))
            CheckSelection(m_inputX, m_inputX2);
    }

    private void ControllerJoin()
    {
        isSelecting = true;
        hasControllerJoined = true;
        
        myPlayer.PressToJoinTmPro.text = "";
        myPlayer.PressToJoinIcon.SetActive(false);
        myPlayer.SelectedHighlight.SetActive(true);
        myPlayer.SelectedHighlightLerp.StartLerp();
        myPlayer.CharacterSelectionGameObject.SetActive(true);
    }

    private void ControllerSelectPlayer()
    {
        if (PassControllersToGame.playerOwnedBy[myPlayer.SelectedCharacterSelection.SelectedCharacterIndex] < 4) //Already assigned
            return;
        
        playerSelected = true;
        myPlayer.SelectedHighlightLerp.isLerping = false;
        myPlayer.SelectedHighlightLerp.hasSelected = true;
        myPlayer.CharacterMaskCloseSelect.OnSelected();
        playerHandler.playersSelected[CONTROLLER_ID] = true;
        
        PassControllersToGame.playerOwnedBy[myPlayer.SelectedCharacterSelection.SelectedCharacterIndex] = CONTROLLER_ID;
        myPlayer.SelectedCharacterSelection.CharacterSelected();
        
        playerHandler.SelectPlayer(CONTROLLER_ID);
    }
    
    private void MapController()
    {
        string[] controllersConnected = Input.GetJoystickNames();
        
        if (controllersConnected.Length == 0 && CONTROLLER_ID == 0)
        {
            MapKeyboard();
            return;
        }

        if (CONTROLLER_ID <= controllersConnected.Length - 1 && CONTROLLER_ID >= 0)
        {
                string controllerName = controllersConnected[CONTROLLER_ID].ToLower();

            if (controllerName.Contains(StringManager.Controllers.Xbox))
                MapXbox();
            else if (controllerName.Contains(StringManager.Controllers.PS4))
                MapPs4();
            else if (controllerName.Contains(StringManager.Controllers.Switch))
                MapSwitch();
        }
        else
        {
            if (controllersConnected.Length < playerHandler.MAX_PLAYERS && CONTROLLER_ID == controllersConnected.Length
            ) //Assign last spot to keyboard
            {
                MapKeyboard();
                return;
            }

            Destroy(gameObject); //If controller is missing or could not be recognized AND keyboard is not IN USE
            return;
        }
    }

    private void MapXbox()
    {
        action1 = StringManager.Inputs.Action1 + CONTROLLER_ID;
        aButton = StringManager.ButtonInputs.A_Button + CONTROLLER_ID;
        rightHorizontal = StringManager.Inputs.RightHorizontal + CONTROLLER_ID;
        horizontal = StringManager.Inputs.Horizontal + CONTROLLER_ID;
    }

    private void MapPs4()
    {
        action1 = StringManager.Inputs.Action1PS4 + CONTROLLER_ID;
        aButton = StringManager.ButtonInputs.X_ButtonPS4 + CONTROLLER_ID;
        rightHorizontal = StringManager.Inputs.RightHorizontalPS4 + CONTROLLER_ID;
        horizontal = StringManager.Inputs.Horizontal + CONTROLLER_ID;
    }

    private void MapSwitch()
    {
        //Todo Add switch
    }

    private void MapKeyboard()
    {
        PassControllersToGame.isKeyboardUsed = true;
        PassControllersToGame.keyBoardOwnedBy = CONTROLLER_ID;
                
        rightHorizontal = StringManager.Inputs.HorizontalKeyboard;
        horizontal = StringManager.Inputs.HorizontalKeyboard;
        action1 = StringManager.Inputs.Action1Keyboard;
        aButton = StringManager.ButtonInputs.A_ButtonKeyboard;
    }
    
    private void CheckSelection(float _inputXRight, float _inputXLeft)
    {
        if (Mathf.Abs(_inputXLeft) > Mathf.Abs(_inputXRight) && Mathf.Abs(_inputXLeft) > deadZoneLeft)
        {
            _inputXRight = _inputXLeft;
            deadZoneX = deadZoneLeft;
        }
        else
            deadZoneX = deadZoneRight;

        if (_inputXRight > deadZoneX || _inputXRight < -deadZoneX)
        {
            if (inputBool)
                return;

            if (_inputXRight > deadZoneX) _inputXRight = Mathf.CeilToInt(_inputXRight);
            else if (_inputXRight < -deadZoneX) _inputXRight = Mathf.FloorToInt(_inputXRight);

            inputBool = true;

            if (_inputXRight > 0)
                myPlayer.SelectedCharacterSelection.NextCharacter(false);
            else if (_inputXRight < 0)
                myPlayer.SelectedCharacterSelection.NextCharacter(true);
        }
        else
            inputBool = false;
    }
}