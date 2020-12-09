using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private static bool paused = false;

    private void Start()
    {
        if (pauseMenuUI is null)
        {
            Debug.LogError("PauseMenu missing");
            Destroy(gameObject);
        }
        
        //To set the right state of the pause menu
        if (paused)
            ResumeGame();
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (!Input.GetKeyUp(KeyCode.Escape))
            return;

        if (paused)
            ResumeGame();
        else
            PauseGame();
    }
    
    public void ResumeGame()
    {
        paused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void PauseGame()
    {
        paused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}