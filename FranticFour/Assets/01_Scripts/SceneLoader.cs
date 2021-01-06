using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

	private static string GameScene = "PlayerSelection";
	private static string MainMenuScene = "MainMenu";

	public void ChangeScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void LoadPlayerSelection()
	{
		SceneManager.LoadScene(GameScene);
	}

	public void LoadMainMenuScene()
	{
		SceneManager.LoadScene(MainMenuScene);
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
