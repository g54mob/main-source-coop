using System.Collections.Generic;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Gameplay
{
	public class GameSceneDirector : MonoBehaviour
	{
		private readonly struct SceneRequest
		{
			public readonly string SceneName;

			public readonly bool Unload;

			public SceneRequest(string sceneName, bool unload)
			{
				SceneName = sceneName;
				Unload = unload;
			}
		}

		[Tooltip("Voxel editor UI'ini tasiyan paylasilan sahne. Sadece UsesVoxelEditor diyen modlar icin yuklenir. Bos birakilirsa editor sahnesi hic yuklenmez.")]
		[AssetSelectorPopup("SceneAsset", false)]
		[SerializeField]
		private string editorSceneName = "Editor";

		private LobbySettingsSync lobbySettings;

		private const float SettingsWaitSeconds = 5f;

		private float waitingForSettingsSince;

		private readonly List<string> loadedScenes = new List<string>();

		private readonly Queue<SceneRequest> pendingScenes = new Queue<SceneRequest>();

		private string requestedMapScene = "";

		private bool modeResolved;

		private bool modeScenesQueued;

		private GameModeDefinition activeMode;

		private void Awake()
		{
			lobbySettings = Object.FindFirstObjectByType<LobbySettingsSync>();
		}

		private void Update()
		{
			if (!IsServerReady())
			{
				return;
			}
			if (!modeResolved)
			{
				string text = lobbySettings.CurrentSettings.ModeId.ToString();
				if (string.IsNullOrEmpty(text))
				{
					if (waitingForSettingsSince <= 0f)
					{
						waitingForSettingsSince = Time.unscaledTime;
					}
					if (Time.unscaledTime - waitingForSettingsSince < 5f)
					{
						return;
					}
					Debug.LogWarning("[GameSceneDirector] Lobi ayarlari " + $"{5f} saniyede gelmedi - varsayilanla devam ediliyor.");
				}
				modeResolved = true;
				activeMode = ResolveMode(text);
			}
			ReconcileMapScene();
			if (!modeScenesQueued)
			{
				modeScenesQueued = true;
				QueueModeScenes();
			}
			PumpQueue();
		}

		private void PumpQueue()
		{
			if (pendingScenes.Count == 0)
			{
				return;
			}
			SceneRequest sceneRequest = pendingScenes.Peek();
			SceneEventProgressStatus sceneEventProgressStatus = (sceneRequest.Unload ? StartUnload(sceneRequest.SceneName) : StartLoad(sceneRequest.SceneName));
			if (sceneEventProgressStatus == SceneEventProgressStatus.SceneEventInProgress)
			{
				return;
			}
			pendingScenes.Dequeue();
			if (sceneEventProgressStatus == SceneEventProgressStatus.Started)
			{
				if (sceneRequest.Unload)
				{
					loadedScenes.Remove(sceneRequest.SceneName);
				}
				else
				{
					loadedScenes.Add(sceneRequest.SceneName);
				}
				return;
			}
			Debug.LogError("[GameSceneDirector] '" + sceneRequest.SceneName + "' sahnesi " + string.Format("{0}: {1}. ", sceneRequest.Unload ? "bosaltilamadi" : "yuklenemedi", sceneEventProgressStatus) + "Sahne Build Settings'te ekli mi?", this);
		}

		private static SceneEventProgressStatus StartLoad(string sceneName)
		{
			return NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		}

		private SceneEventProgressStatus StartUnload(string sceneName)
		{
			Scene sceneByName = SceneManager.GetSceneByName(sceneName);
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				return SceneEventProgressStatus.Started;
			}
			if (sceneByName == SceneManager.GetActiveScene())
			{
				SceneManager.SetActiveScene(base.gameObject.scene);
			}
			return NetworkManager.Singleton.SceneManager.UnloadScene(sceneByName);
		}

		private bool IsServerReady()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsServer || !singleton.IsListening)
			{
				return false;
			}
			if (lobbySettings == null)
			{
				lobbySettings = Object.FindFirstObjectByType<LobbySettingsSync>();
			}
			if (lobbySettings != null)
			{
				return lobbySettings.IsSpawned;
			}
			return false;
		}

		private void QueueModeScenes()
		{
			if (!(activeMode == null))
			{
				Enqueue(activeMode.ModeSceneName);
				if (activeMode.UsesVoxelEditor && !string.IsNullOrWhiteSpace(editorSceneName))
				{
					Enqueue(editorSceneName);
				}
				if (activeMode.UsesHunterRoom)
				{
					Enqueue(activeMode.HunterRoomSceneName);
				}
			}
		}

		private void ReconcileMapScene()
		{
			string text = ResolveDesiredMapScene();
			if (!(text == requestedMapScene))
			{
				if (!string.IsNullOrEmpty(requestedMapScene))
				{
					Enqueue(requestedMapScene, unload: true);
				}
				requestedMapScene = text;
				Enqueue(text);
			}
		}

		private string ResolveDesiredMapScene()
		{
			if (activeMode != null && !activeMode.UsesMap)
			{
				return activeMode.OwnMapSceneName ?? "";
			}
			MapScriptableObject mapScriptableObject = MapCatalog.Find(lobbySettings.CurrentSettings.MapId.ToString()) ?? MapCatalog.Default;
			if (!(mapScriptableObject != null) || !mapScriptableObject.UsesScene)
			{
				return "";
			}
			return mapScriptableObject.SceneName;
		}

		private void Enqueue(string sceneName, bool unload = false)
		{
			if (!string.IsNullOrWhiteSpace(sceneName))
			{
				bool flag = loadedScenes.Contains(sceneName) || IsQueued(sceneName, unload: false);
				if (!(unload ? (!flag) : flag) && !IsQueued(sceneName, unload))
				{
					pendingScenes.Enqueue(new SceneRequest(sceneName, unload));
				}
			}
		}

		private bool IsQueued(string sceneName, bool unload)
		{
			foreach (SceneRequest pendingScene in pendingScenes)
			{
				if (pendingScene.Unload == unload && pendingScene.SceneName == sceneName)
				{
					return true;
				}
			}
			return false;
		}

		private GameModeDefinition ResolveMode(string modeId)
		{
			GameModeDefinition gameModeDefinition = GameModeCatalog.Find(modeId);
			if (gameModeDefinition != null)
			{
				return gameModeDefinition;
			}
			GameModeDefinition gameModeDefinition2 = GameModeCatalog.Default;
			if (gameModeDefinition2 != null)
			{
				Debug.LogError("[GameSceneDirector] '" + modeId + "' oyun modu bulunamadi - '" + gameModeDefinition2.ModeId + "' ile devam ediliyor. Resources/GameModes altinda bu id'ye sahip bir GameModeDefinition var mi?", this);
				return gameModeDefinition2;
			}
			Debug.LogError("[GameSceneDirector] Resources/GameModes altinda kullanilabilir hicbir GameModeDefinition yok - oyun modu yuklenemedi.", this);
			return null;
		}

		public void UnloadModeScenes()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsServer || !singleton.IsListening)
			{
				Forget();
				return;
			}
			foreach (string loadedScene in loadedScenes)
			{
				Scene sceneByName = SceneManager.GetSceneByName(loadedScene);
				if (sceneByName.IsValid() && sceneByName.isLoaded)
				{
					if (sceneByName == SceneManager.GetActiveScene())
					{
						SceneManager.SetActiveScene(base.gameObject.scene);
					}
					singleton.SceneManager.UnloadScene(sceneByName);
				}
			}
			Forget();
		}

		private void Forget()
		{
			loadedScenes.Clear();
			pendingScenes.Clear();
			requestedMapScene = "";
			modeResolved = false;
			modeScenesQueued = false;
			activeMode = null;
		}
	}
}
