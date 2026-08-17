using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionSystem : Singleton<TransitionSystem>
{
	[SerializeField]
	private Image _transition;

	[SerializeField]
	private GameObject _loadingScreen;

	[SerializeField]
	private Slider _loadingBar;

	[SerializeField]
	private AudioClip _fadeIn;

	[SerializeField]
	private AudioClip _fadeOut;

	private Material _transitionMaterial;

	private AudioSource _audioSource;

	private string _sceneLoaded;

	public int ScenesCount => SceneManager.sceneCount;

	public event Action<string> OnSceneUnloaded;

	public event Action<string> OnSceneLoaded;

	private void Start()
	{
		_transitionMaterial = _transition.material;
		_transitionMaterial.SetFloat("_IrisScale", 1.5f);
		_audioSource = GetComponent<AudioSource>();
	}

	public bool IsSceneLoaded(Scenes sceneName)
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).name == sceneName.ToString())
			{
				return true;
			}
		}
		return false;
	}

	public void LoadScene(Scenes scene, LoadSceneMode loadMode = LoadSceneMode.Single)
	{
		string text = scene.ToString();
		SceneManager.LoadScene(text, loadMode);
		this.OnSceneLoaded?.Invoke(text);
	}

	public AsyncOperation LoadSceneAsync(Scenes scene, LoadSceneMode loadMode = LoadSceneMode.Single, TransitionType type = TransitionType.None, bool loadinScene = false)
	{
		string sceneName = scene.ToString();
		if (loadinScene)
		{
			_loadingScreen.SetActive(value: true);
			Application.backgroundLoadingPriority = ThreadPriority.Low;
		}
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
		if (loadinScene)
		{
			StartCoroutine(LoadingScreen(asyncOperation));
		}
		StartCoroutine(LoadScene(asyncOperation, sceneName, type));
		return asyncOperation;
	}

	public string GetActiveScene()
	{
		return SceneManager.GetActiveScene().name;
	}

	private IEnumerator LoadScene(AsyncOperation operation, string sceneName, TransitionType type = TransitionType.None)
	{
		_sceneLoaded = sceneName;
		operation.allowSceneActivation = false;
		operation.completed += SceneLoaded;
		bool coroutineStarted = false;
		while (!operation.isDone)
		{
			if (operation.progress >= 0.9f)
			{
				if (type != TransitionType.None && !coroutineStarted)
				{
					StartCoroutine(PlayTransition(operation, _sceneLoaded));
					break;
				}
				if (type == TransitionType.None)
				{
					operation.allowSceneActivation = true;
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator LoadingScreen(AsyncOperation operation)
	{
		while (!operation.isDone)
		{
			float value = Mathf.Clamp01(operation.progress / 0.9f);
			_loadingBar.value = value;
			yield return new WaitForEndOfFrame();
		}
	}

	private void SceneLoaded(AsyncOperation obj)
	{
		this.OnSceneLoaded?.Invoke(_sceneLoaded);
	}

	private IEnumerator PlayTransition(AsyncOperation operation, string sceneName)
	{
		float t = 0f;
		if (_audioSource != null)
		{
			_audioSource.PlayOneShot(_fadeIn);
		}
		do
		{
			_transitionMaterial.SetFloat("_IrisScale", Mathf.Lerp(1.5f, 0f, t));
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (!(t >= 1f));
		t = 1f;
		_transitionMaterial.SetFloat("_IrisScale", Mathf.Lerp(1.5f, 0f, t));
		operation.allowSceneActivation = true;
		_transitionMaterial.SetFloat("_IrisScale", 0f);
		while (!operation.isDone)
		{
			yield return new WaitForEndOfFrame();
			_loadingScreen.SetActive(value: false);
		}
		_audioSource.PlayOneShot(_fadeOut);
		while (true)
		{
			_transitionMaterial.SetFloat("_IrisScale", Mathf.Lerp(1.5f, 0f, t));
			t -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
			if (t <= 0f)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		t = 0f;
		_transitionMaterial.SetFloat("_IrisScale", Mathf.Lerp(1.5f, 0f, t));
	}

	public void UnloadScene(Scenes sceneName)
	{
		UnloadScene(sceneName.ToString());
	}

	private void UnloadScene(string sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName);
		this.OnSceneUnloaded?.Invoke(sceneName);
	}
}
