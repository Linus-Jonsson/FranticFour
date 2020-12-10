using TMPro;
using UnityEngine;

public class ControllersDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] displayText = new TextMeshPro[4];
    private string[] controllers = null;
    private bool isKeyboardUsed = false;

    private void Update()
    {
        controllers = Input.GetJoystickNames();

        if (controllers.Length < 4) isKeyboardUsed = true;

        for (int  i = 0; i <= controllers.Length; i++)
        {
            if (i == controllers.Length && isKeyboardUsed)
                displayText[i].text = $"ID{i}: Keyboard";
            else if (i != controllers.Length) 
                displayText[i].text = $"ID{i}: {controllers[i]}";
        }
    }
}