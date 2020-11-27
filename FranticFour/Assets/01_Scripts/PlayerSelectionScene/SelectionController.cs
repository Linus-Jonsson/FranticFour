using System;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)] [SerializeField]
    private int CONTROLLER_ID = 0;

    private int selected;
    private float inputZone = 0.04f;
    private bool inputBool = false;

    [SerializeField] private bool controllerAssignedToPlayer = false;

    [Header("Assigned controls")] [SerializeField]
    private string action1;

    private string rightHorizontal;
    private string rightVertical;

    [SerializeField] private InputManager playerHandler;

    private bool isAssigned = false;

    private void Start()
    {
        selected = CONTROLLER_ID;
        playerHandler = FindObjectOfType<InputManager>();

        string controllerName = "";
        string[] controllersConnected = Input.GetJoystickNames();

        for (int i = 0; i < controllersConnected.Length; i++)
        {
            controllersConnected[i] = controllersConnected[i].ToLower();
        }

        if (CONTROLLER_ID < controllersConnected.Length)
            controllerName = controllersConnected[CONTROLLER_ID];

        if (controllerName.Contains("xbox"))
        {
            action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
            rightHorizontal = StringManager.Inputs.rightHorizontal + CONTROLLER_ID;
            rightVertical = StringManager.Inputs.rightVertical + CONTROLLER_ID;
        }
        else if (controllerName.Contains("wireless controller"))
        {
            action1 = StringManager.Inputs.action1PS4 + CONTROLLER_ID;
            rightHorizontal = StringManager.Inputs.rightHorizontalPS4 + CONTROLLER_ID;
            rightVertical = StringManager.Inputs.rightVerticalPS4 + CONTROLLER_ID;
        }
        else
        {
            if (!PassControllersToGame.isKeyboardUsed)
            {
                PassControllersToGame.isKeyboardUsed = true;
                PassControllersToGame.keyBoardOwnedBy = CONTROLLER_ID;
                
                rightHorizontal = StringManager.Inputs.horizontalKeyboard;
                rightVertical = StringManager.Inputs.verticalKeyboard;
                action1 = StringManager.Inputs.action1Keyboard;
                
                Debug.LogWarning("Controller is missing or cant be recognized: " + controllerName +
                                 " You are set to keyboard");
            }
            else
            {
                action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
                Debug.LogError("Controller is missing or cant be recognized: " + controllerName);
                isAssigned = true; //Error so skip loop in update
            }
        }
    }

    private void Update()
    {
        if (isAssigned)
            return;
            
            if (Input.GetAxis(action1) == 1 && !controllerAssignedToPlayer) //Funkar med PS4 och Xbox
        {
            if (!playerHandler.playersSelected[selected]) //Är den större än 3 är den inte vald
            {
                playerHandler.playersSelected[selected] = true;
                controllerAssignedToPlayer = true;
                PassControllersToGame.playerOwnedBy[selected] = CONTROLLER_ID;
                playerHandler.SetTextAssigned(selected, CONTROLLER_ID);
                playerHandler.CheckPlayers();
                isAssigned = true;
                Debug.Log("Player "+ selected +" assigned" + Input.GetAxis(action1));
            }
            //playerHandler.TakeNextPlayer(CONTROLLER_ID);
        }
        else if (Input.GetButton(action1) && !controllerAssignedToPlayer) //Funkar med tangentbord
        {
            if (!playerHandler.playersSelected[selected] && !PassControllersToGame.isKeyboardUsed) //Är den större än 3 är den inte vald
            {
                playerHandler.playersSelected[selected] = true;
                controllerAssignedToPlayer = true;
                PassControllersToGame.isKeyboardUsed = true;
                PassControllersToGame.keyBoardOwnedBy = CONTROLLER_ID;
                PassControllersToGame.playerOwnedBy[selected] = CONTROLLER_ID;
                playerHandler.SetTextAssigned(selected, CONTROLLER_ID);
                playerHandler.CheckPlayers();
                isAssigned = true;
                Debug.Log("Player "+ selected +" assigned" + Input.GetButton(action1));
            }
            //playerHandler.TakeNextPlayer(CONTROLLER_ID);
        }

        float m_inputX = Input.GetAxis(rightHorizontal);
        float m_inputY = Input.GetAxis(rightVertical);

        if (m_inputX > inputZone || m_inputX < -inputZone || m_inputY < -inputZone || m_inputY > inputZone)
        {
            if (inputBool)
                return;

            if (m_inputX > inputZone) m_inputX = Mathf.CeilToInt(m_inputX);
            else if (m_inputX < -inputZone) m_inputX = Mathf.FloorToInt(m_inputX);

            if (m_inputY > inputZone) m_inputY = Mathf.CeilToInt(m_inputY);
            else if (m_inputY < -inputZone) m_inputY = Mathf.FloorToInt(m_inputY);

            inputBool = true;
            Debug.LogWarning(m_inputX);
            playerHandler.SelectNext(CONTROLLER_ID, ref selected, (int)m_inputX, (int)m_inputY);
        }
        else
        {
            inputBool = false;
        }
    }
}