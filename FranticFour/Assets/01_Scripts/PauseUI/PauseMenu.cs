using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //0 = button | 1 = place to move to | 2 = gameobject to move | 3 = place to move to
    [SerializeField] public GameObject[] buttons = new GameObject[5];

    public void MoveToKeybindsFromMain()
    {
        LeanTween.moveX(buttons[0], buttons[1].transform.position.x, 1f);
        LeanTween.moveX(buttons[2], buttons[3].transform.position.x, 1f);
    }
    
    public void MoveBackToMainFromKeybinds()
    {
        LeanTween.moveX(buttons[0], buttons[3].transform.position.x, 1f);
        LeanTween.moveX(buttons[2], buttons[4].transform.position.x, 1f);
    }
}
