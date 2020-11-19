using UnityEngine;

public class ControllerInit : MonoBehaviour
{
    [SerializeField] private GameObject[] players = new GameObject[4];
    [SerializeField] private Controller[] controllers = new Controller[4];
    
    private void Start()
    {
        //Debug
        controllers[0] = new Controller("Debug1", 0, 0);
        controllers[1] = new Controller("Debug2", 1, 1);
        controllers[2] = new Controller("Debug3", 2, 2);
        //controllers[3] = new Controller("Debug4", 3, 3);
        
        string[] joystickNames = Input.GetJoystickNames();

        for (int controllerNum = 0; controllerNum < joystickNames.Length; controllerNum++)
        {
            if (string.IsNullOrEmpty(joystickNames[controllerNum]))
                continue;
            
            for (int indexID = 0; indexID < controllers.Length; indexID++)
            {
                Debug.Log("|" + string.IsNullOrEmpty(controllers[indexID].Name) + "|");//Debug
                if (!string.IsNullOrEmpty(controllers[indexID].Name))
                {
                    //Controller already assigned
                    continue;
                }
                //Assign controller
                controllers[indexID] = new Controller(joystickNames[controllerNum], indexID, controllerNum);
                indexID = controllers.Length;
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            var movement = players[i].GetComponent<Movement>();
            movement.controller = controllers[i];
        }
    }
}