using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
	public static SceneLoader instance;

	private static string GameScene = "PlayerSelection";
	private static string MainMenuScene = "MainMenu";
	
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}	
	}

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
