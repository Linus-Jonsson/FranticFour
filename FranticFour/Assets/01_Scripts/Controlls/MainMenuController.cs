using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionsWindow = null;
    [SerializeField] GameObject controlsWindow = null;
    [SerializeField] GameObject creditsWindow = null;

    public void SetOptionsWindow(bool value)
    {
        optionsWindow.SetActive(value);
    }

    public void SetControlsWindow(bool value)
    {
        controlsWindow.SetActive(value);
    }

    public void SetCreditsWindow(bool value)
    {
        creditsWindow.SetActive(value);
    }
}
