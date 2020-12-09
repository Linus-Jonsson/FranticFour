using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Local variables")] [SerializeField]
    private GameObject pauseMenu;

    [SerializeField] private static bool paused = false;

    private static bool isGamePause = false;
    public static bool IsGamePause => isGamePause;


    private void Start()
    {
        if (!(pauseMenu is null)) return;
        Debug.LogError("Pause menu is missing reference");
        Destroy(gameObject);
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (paused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        paused = true;
        isGamePause = paused;
        
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        paused = false;
        isGamePause = paused;
        
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }


    //0 = button | 1 = place to move to | 2 = gameobject to move | 3 = place to move to
    //[SerializeField] public GameObject[] buttons = new GameObject[5];
    //
    //public void MoveToKeybindsFromMain()
    //{
    //    LeanTween.moveX(buttons[0], buttons[1].transform.position.x, 1f);
    //    LeanTween.moveX(buttons[2], buttons[3].transform.position.x, 1f);
    //}
    //
    //public void MoveBackToMainFromKeybinds()
    //{
    //    LeanTween.moveX(buttons[0], buttons[3].transform.position.x, 1f);
    //    LeanTween.moveX(buttons[2], buttons[4].transform.position.x, 1f);
    //}
}