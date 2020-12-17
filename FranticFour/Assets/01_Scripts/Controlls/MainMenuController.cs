using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionsWindow = null;
    [SerializeField] GameObject descriptionWindow = null;
    [SerializeField] GameObject controlsWindow = null;
    [SerializeField] GameObject creditsWindow = null;

    void Start()
    {
        FindObjectOfType<AudioController>().TransitionToMain();
        FindObjectOfType<AudioController>().PlayMenuMusic(true);
    }

    public void SetOptionsWindow(bool value)
    {
        optionsWindow.SetActive(value);
    }

    public void SetDescriptionWindow(bool value)
    {
        descriptionWindow.SetActive(value);
    }
    
    public void SetControlsWindow(bool value)
    {
        descriptionWindow.SetActive(false);
        controlsWindow.SetActive(value);
    }

    public void SetCreditsWindow(bool value)
    {
        creditsWindow.SetActive(value);
    }
}
