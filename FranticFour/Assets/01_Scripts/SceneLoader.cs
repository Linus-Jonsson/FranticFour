using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader instance;  //Singleton instance

	void Start()
	{
		if (instance == null)
		{
			instance = this; //Save our object so we can use it easily
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);   //If we already have an instance, avoid creating another.
		}
	}

	public void ChangeScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadNextScene()
	{
		//Get our number, add one.
		int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
		//If we have a number to large, use modulus to loop back to zero
		nextIndex = nextIndex % SceneManager.sceneCountInBuildSettings;
		SceneManager.LoadScene(nextIndex);
	}

	public void LoadPreviousScene()
	{
		int nextIndex = SceneManager.GetActiveScene().buildIndex - 1 + SceneManager.sceneCountInBuildSettings;
		nextIndex = nextIndex % SceneManager.sceneCountInBuildSettings;
		SceneManager.LoadScene(nextIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
