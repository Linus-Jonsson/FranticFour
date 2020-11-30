using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)]
    [SerializeField] private int CONTROLLER_ID = 0;
    [SerializeField] private bool controllerAssignedToPlayer = false;

    [Header("Assigned controls")]
    [SerializeField] private string action1;
    [SerializeField] private InputManager playerHandler;
    
    private int selected;
    private readonly float inputZone = 0.04f;
    private string rightHorizontal;
    private string rightVertical;
    private bool inputBool;
    private bool isAssigned;

    private void Start()
    {
        selected = CONTROLLER_ID;
        playerHandler = FindObjectOfType<InputManager>();
        MapController();
    }

    private void Update()
    {
        if (isAssigned) //If controller have been assigned skip loop
            return;

        if (Input.GetAxis(action1) == 1 && !controllerAssignedToPlayer) //For PS4 and Xbox
            AssignPlayerController();
        else if (Input.GetButton(action1) && !controllerAssignedToPlayer) //For keyboard
                AssignPlayerKeyboard();

        float m_inputX = Input.GetAxis(rightHorizontal);
        float m_inputY = Input.GetAxis(rightVertical);

        if (m_inputX != 0 || m_inputY != 0)
            CheckSelection(m_inputX, m_inputY);

    }
    
    private void MapController()
    {
        string[] controllersConnected = Input.GetJoystickNames();

        if (CONTROLLER_ID >= controllersConnected.Length) //If there there are more expected controllers then controllers
        {
            if (!PassControllersToGame.isKeyboardUsed
            ) //If controller is missing or could not be recognized AND keyboard is not in use
                MapKeyboard();
            else //If controller is missing or could not be recognized AND keyboard is not IN USE
                Destroy(gameObject);

            return;
        }

        string controllerName = controllersConnected[CONTROLLER_ID].ToLower();

        if (controllerName.Contains(StringManager.Controllers.Xbox))
            MapXbox();
        else if (controllerName.Contains(StringManager.Controllers.PS4))
            MapPs4();
        else if (controllerName.Contains(StringManager.Controllers.Switch))
            MapSwitch();
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

    private void AssignPlayerController()
    {
        if (playerHandler.playersSelected[selected])
            return;
        
        playerHandler.playersSelected[selected] = true;
        controllerAssignedToPlayer = true;
        isAssigned = true; 
            
        PassControllersToGame.playerOwnedBy[selected] = CONTROLLER_ID;
            
        playerHandler.SetTextAssigned(selected, CONTROLLER_ID);
        playerHandler.CheckPlayers();
            
        Debug.Log("Player "+ selected +" assigned" + Input.GetAxis(action1));
    }
    
    private void AssignPlayerKeyboard()
    {
        if (playerHandler.playersSelected[selected] || PassControllersToGame.isKeyboardUsed)
            return;
        
        playerHandler.playersSelected[selected] = true;
        controllerAssignedToPlayer = true;
        isAssigned = true;
            
        PassControllersToGame.isKeyboardUsed = true;
        PassControllersToGame.keyBoardOwnedBy = CONTROLLER_ID;
        PassControllersToGame.playerOwnedBy[selected] = CONTROLLER_ID;
            
        playerHandler.SetTextAssigned(selected, CONTROLLER_ID);
        playerHandler.CheckPlayers();
            
        Debug.Log("Player "+ selected +" assigned" + Input.GetButton(action1));
    }

    private void CheckSelection(float _inputX, float _inputY)
    {
        if (_inputX > inputZone || _inputX < -inputZone || _inputY < -inputZone || _inputY > inputZone)
        {
            if (inputBool)
                return;

            if (_inputX > inputZone) _inputX = Mathf.CeilToInt(_inputX);
            else if (_inputX < -inputZone) _inputX = Mathf.FloorToInt(_inputX);

            if (_inputY > inputZone) _inputY = Mathf.CeilToInt(_inputY);
            else if (_inputY < -inputZone) _inputY = Mathf.FloorToInt(_inputY);

            inputBool = true;
            Debug.LogWarning(_inputX);
            playerHandler.SelectNext(CONTROLLER_ID, ref selected, (int)_inputX, (int)_inputY);
        }
        else
            inputBool = false;
    }
}