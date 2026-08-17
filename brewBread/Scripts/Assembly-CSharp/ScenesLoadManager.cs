using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoadManager : MonoBehaviour
{
	public static ScenesLoadManager instance;

	public event Action<string> SceneUnloaded;

	public event Action<string> SceneLoaded;

	private void Awake()
	{
		if (UnityEngine.Object.FindObjectsOfType<ScenesLoadManager>().Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void UnloadScene(string sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName);
		this.SceneUnloaded?.Invoke(sceneName);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		this.SceneLoaded?.Invoke(sceneName);
	}
}
