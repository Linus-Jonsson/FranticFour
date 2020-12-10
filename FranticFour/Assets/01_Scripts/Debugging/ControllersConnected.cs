using TMPro;
using UnityEngine;

public class ControllersConnected : MonoBehaviour
{
    [SerializeField] private TextMeshPro displayText = null;
    private string title = "Controllers";
    private string[] controllers = null;
    private string textToDisplay = "";

    private void Update()
    {
        controllers = Input.GetJoystickNames();
        textToDisplay = title;
        
        if (controllers.Length != 0)
        {
            for (int i = 0; i < controllers.Length; i++)
                textToDisplay += $"\nID: {i} Name: {controllers[i]}";
        }
        else if (controllers.Length == 0)
            textToDisplay += "\nNo controllers connected.";
        else
            textToDisplay += "\n Null or error. Please check.";

        displayText.text = textToDisplay;
    }
}
