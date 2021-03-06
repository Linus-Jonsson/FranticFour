﻿using System;
using UnityEngine;
using static StringManager;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] 
    [Range(0, 3)] [SerializeField] private int CONTROLLER_ID = 0;
    [SerializeField] private bool hasControllerJoined;
    [SerializeField] private MyPlayer myPlayer;

    [Header("Assigned controls")]
    [SerializeField] private string action1; //Debug
    [SerializeField] private string aButton; //Debug
    [SerializeField] private string bButton; //Debug
    [SerializeField] private InputManager playerHandler;

    //Dead-zones
    private float deadZoneX = 0f;
    private readonly float deadZoneAction = 0.5f;
    private readonly float deadZoneRight = 0.04f;
    private readonly float deadZoneLeft = 0.65f;
    //Axis
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
        //Error check
        if (action1 == String.Empty || action1 is null)
        {
            Destroy(gameObject);
            return;
        }
        
        if (!hasControllerJoined && (Input.GetAxis(action1) >= 1 - deadZoneAction || Input.GetButtonUp(aButton))) //For PS4, Xbox and Keyboard
            ControllerJoin();
        else if (hasControllerJoined && !isSelecting && (Input.GetAxis(action1) >= 1 - deadZoneAction || Input.GetButtonUp(aButton))) //For PS4, Xbox and Keyboard
            ControllerSelectPlayer();
        else if (isSelecting && Input.GetAxis(action1) <= 0)
            isSelecting = false;

        if (playerSelected && Input.GetButtonUp(bButton))
            DeselectPlayer();

        float m_inputX = Input.GetAxis(rightHorizontal);
        float m_inputX2 = Input.GetAxis(horizontal);
        
        if (hasControllerJoined && !playerSelected && (m_inputX != 0 || m_inputX2 != 0))
            CheckSelection(m_inputX, m_inputX2);
    }

    private void ControllerJoin()
    {
        isSelecting = true;
        hasControllerJoined = true;
        
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

    private void DeselectPlayer()
    {
        playerSelected = false;
        myPlayer.SelectedHighlightLerp.isLerping = true;
        myPlayer.SelectedHighlightLerp.hasSelected = false;
        
        myPlayer.CharacterMaskCloseSelect.OnDeselected();
        playerHandler.playersSelected[CONTROLLER_ID] = false;
        
        PassControllersToGame.playerOwnedBy[myPlayer.SelectedCharacterSelection.SelectedCharacterIndex] = 99;
        myPlayer.SelectedCharacterSelection.CharacterDeselected();
        
        playerHandler.DeselectPlayer(CONTROLLER_ID);
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

            if (controllerName.Contains(Controllers.Xbox))
                MapXbox();
            else if (controllerName.Contains(Controllers.PS4))
                MapPs4();
            else if (controllerName.Contains(Controllers.Switch))
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
        //Buttons Xbox
        action1 = Inputs.Action1 + CONTROLLER_ID;
        aButton = ButtonInputs.A_Button + CONTROLLER_ID;
        bButton = ButtonInputs.B_Button + CONTROLLER_ID;
        
        //Axis Xbox
        rightHorizontal = Inputs.RightHorizontal + CONTROLLER_ID;
        horizontal = Inputs.Horizontal + CONTROLLER_ID;
    }

    private void MapPs4()
    {
        //Buttons PS4
        action1 = Inputs.Action1PS4 + CONTROLLER_ID;
        aButton = ButtonInputs.X_ButtonPS4 + CONTROLLER_ID;
        bButton = ButtonInputs.Circle_ButtonPS4 + CONTROLLER_ID;
        
        //Axis PS4
        rightHorizontal = Inputs.RightHorizontalPS4 + CONTROLLER_ID;
        horizontal = Inputs.Horizontal + CONTROLLER_ID;
    }

    private void MapSwitch()
    {
        //Todo Add switch
    }

    private void MapKeyboard()
    {
        //Keyboard
        PassControllersToGame.isKeyboardUsed = true;
        PassControllersToGame.keyBoardOwnedBy = CONTROLLER_ID;
                
        rightHorizontal = Inputs.HorizontalKeyboard;
        horizontal = Inputs.HorizontalKeyboard;
        
        action1 = Inputs.Action1Keyboard;
        aButton = ButtonInputs.A_ButtonKeyboard;
        bButton = ButtonInputs.B_ButtonKeyboard;
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