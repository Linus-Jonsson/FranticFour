﻿using System;
using UnityEngine;
using static StringManager;

public class AssignedController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)]
    [SerializeField] private int playerID;
    [SerializeField] private bool usesMouse;
    [SerializeField] private bool usesTwinSticks;

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
    
    public bool UsesTwinSticks
    {
        get => usesTwinSticks;
        private set { usesTwinSticks = value;
            if (usesTwinSticks)
                jump = ButtonInputs.A_Button + playerID;
            else
                jump = Inputs.Jump + playerID; }
    }

    //Control buttons
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

    public void Init()
    {
        GetControllerType();
    }

    private void SetControllerKeysXbox() //XBOX
    {
        vertical = Inputs.Vertical + playerID;
        horizontal = Inputs.Horizontal + playerID;

        rightVertical = Inputs.RightVertical + playerID;
        rightHorizontal = Inputs.RightHorizontal + playerID;

        if (!usesTwinSticks)
        {
            action1 = ButtonInputs.X_Button + playerID;
            jump = ButtonInputs.A_Button + playerID;
        }
        else
        {
            action1 = Inputs.Action1 + playerID;
            jump = Inputs.Jump + playerID;
        }
        
    }

    private void SetControllerKeysPS4() //PS4
    {
        vertical = Inputs.Vertical + playerID;
        horizontal = Inputs.Horizontal + playerID;

        rightVertical = Inputs.RightVerticalPS4 + playerID;
        rightHorizontal = Inputs.RightHorizontalPS4 + playerID;

        if (!usesTwinSticks)
        {
            action1 = ButtonInputs.Square_ButtonPS4 + playerID;
            jump = ButtonInputs.X_ButtonPS4 + playerID;
        }
        else
        {
            action1 = Inputs.Action1PS4 + playerID;
            jump = Inputs.Jump + playerID;
        }
    }
    
    private void SetControllerKeysSwitch() //Todo Switch
    {
        throw new NotImplementedException();
    }

    private void SetControllerKeyboardAndMouse() //KEYBOARD
    {
        AssignPlayers.keyboardAssigned = true;
        usesMouse = true;

        vertical = Inputs.VerticalKeyboard;
        horizontal = Inputs.HorizontalKeyboard;

        rightVertical = vertical;
        rightHorizontal = horizontal;

        action1 = Inputs.Action1Keyboard;
        jump = Inputs.JumpKeboard;
    }

    private void GetControllerType()
    {
        string controllerName = "";
        string[] controllersConnected = Input.GetJoystickNames();

        if (playerID < controllersConnected.Length)
            controllerName = controllersConnected[playerID];

        controllerName = controllerName.ToLower();

        if (controllerName.Contains(Controllers.Xbox))
            SetControllerKeysXbox();
        else if (controllerName.Contains(Controllers.PS4))
            SetControllerKeysPS4();
        else if (controllerName.Contains(Controllers.Switch)) //Todo Implement nitendo switch controller support
            SetControllerKeysSwitch();
        else if (PassControllersToGame.isKeyboardUsed && PlayerID == PassControllersToGame.keyBoardOwnedBy) //Runs if controller is missing or can't be recognized AND is not in use
            SetControllerKeyboardAndMouse();
        else if (AssignPlayers.keyboardAssigned == false && !PassControllersToGame.isKeyboardUsed) //Runs if the game starts without going through PlayerSelection
            SetControllerKeyboardAndMouse();
        else
            SetControllerKeysXbox(); //If controller name is not recognized and keyboard is taken, map the controller as xbox
    }
}