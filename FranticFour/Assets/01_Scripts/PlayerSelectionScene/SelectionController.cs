using System;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)]
    [SerializeField] private int CONTROLLER_ID = 0;
    [SerializeField] private bool hasControllerJoined;
    [SerializeField] private MyPlayer myPlayer;

    [Header("Assigned controls")]
    [SerializeField] private string action1;
    [SerializeField] private InputManager playerHandler;

    private readonly float inputZone = 0.04f;
    private string rightHorizontal = null;
    private string rightVertical = null;
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
        if (!hasControllerJoined && Input.GetAxis(action1) == 1) //For PS4, Xbox and Keyboard
            ControllerJoin();
        else if (hasControllerJoined && !isSelecting && Input.GetAxis(action1) == 1) //For PS4, Xbox and Keyboard
            ControllerSelectPlayer();
        else if (isSelecting && Input.GetAxis(action1) <= 0)
            isSelecting = false;

        float m_inputX = Input.GetAxis(rightHorizontal);

        if (hasControllerJoined && !playerSelected && m_inputX != 0)
            CheckSelection(m_inputX);

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
        rightHorizontal = StringManager.Inputs.RightHorizontal + CONTROLLER_ID;
        rightVertical = StringManager.Inputs.RightVertical + CONTROLLER_ID;
    }

    private void MapPs4()
    {
        action1 = StringManager.Inputs.Action1PS4 + CONTROLLER_ID;
        rightHorizontal = StringManager.Inputs.RightHorizontalPS4 + CONTROLLER_ID;
        rightVertical = StringManager.Inputs.RightVerticalPS4 + CONTROLLER_ID;
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
        rightVertical = StringManager.Inputs.VerticalKeyboard;
        action1 = StringManager.Inputs.Action1Keyboard;
    }
    
    private void CheckSelection(float _inputX)
    {
        if (_inputX > inputZone || _inputX < -inputZone)
        {
            if (inputBool)
                return;

            if (_inputX > inputZone) _inputX = Mathf.CeilToInt(_inputX);
            else if (_inputX < -inputZone) _inputX = Mathf.FloorToInt(_inputX);

            inputBool = true;

            if (_inputX > 0)
                myPlayer.SelectedCharacterSelection.NextCharacter(true);
            else if (_inputX < 0)
                myPlayer.SelectedCharacterSelection.NextCharacter(false);
        }
        else
            inputBool = false;
    }
}