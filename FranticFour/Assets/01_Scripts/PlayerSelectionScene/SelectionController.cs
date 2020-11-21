using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [Header("Player")] [Range(0, 3)] [SerializeField]
    private int CONTROLLER_ID;

    [SerializeField] private bool controllerAssignedToPlayer = false;

    [Header("Assigned controls")] [SerializeField]
    private string action1;

    [SerializeField] private InputManager playerHandler;

    private void Start()
    {
        playerHandler = FindObjectOfType<InputManager>();

        string controllerName = "";
        string[] controllersConnected = Input.GetJoystickNames();

        if (CONTROLLER_ID < controllersConnected.Length)
            controllerName = controllersConnected[CONTROLLER_ID];

        if (controllerName.Contains("XBOX"))
            action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
        else if (controllerName.Contains("Wireless Controller"))
            action1 = StringManager.Inputs.action1PS4 + CONTROLLER_ID;
        else
        {
            action1 = StringManager.Inputs.action1 + CONTROLLER_ID;
            Debug.LogWarning("Controller is missing or cant be recognized: " + controllerName);
        }
    }

    private void Update()
    {
        if (Input.GetAxis(action1) == 1 && !controllerAssignedToPlayer) //Kanske inte funkar med Xbox?
        {
            controllerAssignedToPlayer = true;
            Debug.Log("Player assigned" + Input.GetAxis(action1));
            playerHandler.TakeNextPlayer(CONTROLLER_ID);
        }
    }
}