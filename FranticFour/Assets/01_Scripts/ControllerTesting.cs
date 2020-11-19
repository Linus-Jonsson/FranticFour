using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ControllerTesting : MonoBehaviour
{
    private Controller[] controllers = new Controller[4];
    private void Start()
    {
        string[] joystickNames = Input.GetJoystickNames();

        for (int controllerNum = 0; controllerNum < joystickNames.Length; controllerNum++)
        {
            Debug.Log("16: " + controllerNum);
            if (string.IsNullOrEmpty(joystickNames[controllerNum]))
                continue;
            else
            {
                for (int indexID = 0; indexID < controllers.Length; indexID++)
                {
                    Debug.Log("23: " + indexID);
                    if (controllers[indexID] == null)
                    {
                        controllers[indexID] = new Controller(joystickNames[controllerNum], indexID, controllerNum);
                        indexID = controllers.Length;
                    }
                }
            }
        }
    }

    private void Update()
    {
        float moveSpeed = 10;

        float horizontalInput = Input.GetAxis("Horizontal1");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (moveSpeed * Time.deltaTime));
    }
}