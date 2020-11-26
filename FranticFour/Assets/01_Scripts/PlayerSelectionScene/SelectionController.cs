using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] 
    [Range(0, 3)] [SerializeField] private int CONTROLLER_ID = 0;

    [SerializeField] private bool controllerAssignedToPlayer = false;

    [Header("Assigned controls")]
    [SerializeField] private string action1;

    [SerializeField] private InputManager playerHandler;
    
    [SerializeField]

    private void Start()
    {
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
            action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
        else if (controllerName.Contains("wireless controller"))
            action1 = StringManager.Inputs.action1PS4 + CONTROLLER_ID;
        else
        {
            if (!PassControllersToGame.isKeyboardUsed)
            {
                PassControllersToGame.isKeyboardUsed = true;
                action1 = StringManager.Inputs.action1Keyboard;
                Debug.LogWarning("Controller is missing or cant be recognized: " + controllerName + " You are set to keyboard");
            }
            else
            {
                action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
                Debug.LogWarning("Controller is missing or cant be recognized: " + controllerName);
            }
        }
    }

    private void Update()
    {
        if (Input.GetAxis(action1) == 1 && !controllerAssignedToPlayer) //Funkar med PS4 och Xbox
        {
            controllerAssignedToPlayer = true;
            Debug.Log("Player assigned" + Input.GetAxis(action1));
            playerHandler.TakeNextPlayer(CONTROLLER_ID);
        }
        else if (Input.GetButton(action1) && !controllerAssignedToPlayer) //Funkar med tangentbord
        {
            controllerAssignedToPlayer = true;
            Debug.Log("Player assigned" + Input.GetButton(action1));
            playerHandler.TakeNextPlayer(CONTROLLER_ID);
        }
    }
}