using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EvilCore
{
	public class SceneFlowManager : MonoBehaviour, ISceneFlowManager
	{
		[SerializeField]
		private SceneFlowConfig config;

		private readonly HashSet<string> _loadedScenes = new HashSet<string>();

		public bool IsGameSceneLoaded { get; private set; }

		public bool IsMainMenuSceneLoaded { get; private set; }

		public bool IsGameMenuSceneLoaded { get; private set; }

		public bool IsTransitioning { get; private set; }

		private string GameSceneName
		{
			get
			{
				if (!(config != null))
				{
					return "Game";
				}
				return config.GameSceneName;
			}
		}

		private string MainMenuSceneName
		{
			get
			{
				if (!(config != null))
				{
					return "MainMenu";
				}
				return config.MainMenuSceneName;
			}
		}

		private string GameMenuSceneName
		{
			get
			{
				if (!(config != null))
				{
					return "GameMenu";
				}
				return config.GameMenuSceneName;
			}
		}

		public event Action OnGameSceneLoaded;

		public event Action OnGameSceneUnloaded;

		public event Action OnMainMenuSceneLoaded;

		public event Action OnMainMenuSceneUnloaded;

		public event Action OnGameMenuSceneLoaded;

		public event Action OnGameMenuSceneUnloaded;

		public event Action<string> OnSceneLoaded;

		public event Action<string> OnSceneUnloaded;

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public async UniTask LoadSceneAsync(string sceneName, bool setActive = false)
		{
			if (_loadedScenes.Contains(sceneName) || IsTransitioning)
			{
				return;
			}
			IsTransitioning = true;
			try
			{
				await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
				if (setActive)
				{
					Scene sceneByName = SceneManager.GetSceneByName(sceneName);
					if (sceneByName.IsValid())
					{
						SceneManager.SetActiveScene(sceneByName);
					}
				}
				_loadedScenes.Add(sceneName);
				this.OnSceneLoaded?.Invoke(sceneName);
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[SceneFlow] Failed to load scene '" + sceneName + "': " + ex.Message, "LoadSceneAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Scripts\\SceneFlowManager.cs", 75);
			}
			finally
			{
				IsTransitioning = false;
			}
		}

		public async UniTask UnloadSceneAsync(string sceneName)
		{
			if (!_loadedScenes.Contains(sceneName) || IsTransitioning)
			{
				return;
			}
			IsTransitioning = true;
			try
			{
				AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
				if (asyncOperation == null)
				{
					_loadedScenes.Remove(sceneName);
					return;
				}
				await asyncOperation.ToUniTask();
				_loadedScenes.Remove(sceneName);
				this.OnSceneUnloaded?.Invoke(sceneName);
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[SceneFlow] Failed to unload scene '" + sceneName + "': " + ex.Message, "UnloadSceneAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Scripts\\SceneFlowManager.cs", 119);
			}
			finally
			{
				IsTransitioning = false;
			}
		}

		public bool IsSceneLoaded(string sceneName)
		{
			return _loadedScenes.Contains(sceneName);
		}

		public async UniTask LoadGameSceneAsync()
		{
			await LoadSceneAsync(GameSceneName, setActive: true);
			IsGameSceneLoaded = _loadedScenes.Contains(GameSceneName);
			if (IsGameSceneLoaded)
			{
				this.OnGameSceneLoaded?.Invoke();
			}
		}

		public async UniTask UnloadGameSceneAsync()
		{
			Scene sceneByBuildIndex = SceneManager.GetSceneByBuildIndex(0);
			if (sceneByBuildIndex.IsValid())
			{
				SceneManager.SetActiveScene(sceneByBuildIndex);
			}
			await UnloadSceneAsync(GameSceneName);
			IsGameSceneLoaded = false;
			this.OnGameSceneUnloaded?.Invoke();
		}

		public async UniTask LoadMainMenuSceneAsync()
		{
			await LoadSceneAsync(MainMenuSceneName);
			IsMainMenuSceneLoaded = _loadedScenes.Contains(MainMenuSceneName);
			if (IsMainMenuSceneLoaded)
			{
				this.OnMainMenuSceneLoaded?.Invoke();
			}
		}

		public async UniTask UnloadMainMenuSceneAsync()
		{
			await UnloadSceneAsync(MainMenuSceneName);
			IsMainMenuSceneLoaded = false;
			this.OnMainMenuSceneUnloaded?.Invoke();
		}

		public async UniTask LoadGameMenuSceneAsync()
		{
			await LoadSceneAsync(GameMenuSceneName);
			IsGameMenuSceneLoaded = _loadedScenes.Contains(GameMenuSceneName);
			if (IsGameMenuSceneLoaded)
			{
				this.OnGameMenuSceneLoaded?.Invoke();
			}
		}

		public async UniTask UnloadGameMenuSceneAsync()
		{
			await UnloadSceneAsync(GameMenuSceneName);
			IsGameMenuSceneLoaded = false;
			this.OnGameMenuSceneUnloaded?.Invoke();
		}

		public async UniTask TransitionToGameAsync()
		{
			await LoadGameSceneAsync();
		}

		public async UniTask TransitionToMainMenuAsync()
		{
			if (IsGameMenuSceneLoaded)
			{
				await UnloadGameMenuSceneAsync();
			}
			if (IsGameSceneLoaded)
			{
				await UnloadGameSceneAsync();
			}
			await LoadMainMenuSceneAsync();
		}
	}
}
