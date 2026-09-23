using System;
using System.Collections.Generic;
using Mimicraft.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Gameplay
{
	public class MapLoader : MonoBehaviour
	{
		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private LobbySettingsSync lobbySettings;

		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private GameModeController roundManager;

		[Tooltip("Parent for the instantiated map. Left empty, this object is used.")]
		[SerializeField]
		private Transform mapRoot;

		private string loadedMapId;

		private GameObject loadedMap;

		private int boundSceneHandle;

		private Scene previousActiveScene;

		private bool subscribed;

		private int boundHunterRoomHandle;

		public static MapScriptableObject CurrentMap { get; private set; }

		public static event Action<MapScriptableObject> MapChanged;

		private static void SetCurrentMap(MapScriptableObject map)
		{
			if (!(CurrentMap == map))
			{
				CurrentMap = map;
				MapLoader.MapChanged?.Invoke(map);
			}
		}

		private void Awake()
		{
			if (lobbySettings == null)
			{
				lobbySettings = UnityEngine.Object.FindFirstObjectByType<LobbySettingsSync>();
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current ?? UnityEngine.Object.FindFirstObjectByType<GameModeController>();
			}
			SetCurrentMap(null);
			if (LobbySettingsSync.PendingSettings.HasValue)
			{
				Load(LobbySettingsSync.PendingSettings.Value.MapId.ToString());
			}
		}

		private void Start()
		{
			TryLoadFromCurrentSettings();
		}

		private void Update()
		{
			if (loadedMap == null && boundSceneHandle == 0)
			{
				TryLoadFromCurrentSettings();
			}
			if (boundHunterRoomHandle == 0)
			{
				TryBindHunterRoom();
			}
		}

		private void TryLoadFromCurrentSettings()
		{
			if (lobbySettings == null)
			{
				lobbySettings = UnityEngine.Object.FindFirstObjectByType<LobbySettingsSync>();
			}
			if (!(lobbySettings == null))
			{
				Subscribe();
				if (lobbySettings.IsSpawned)
				{
					Load(lobbySettings.CurrentSettings.MapId.ToString());
				}
			}
		}

		private void Subscribe()
		{
			if (!subscribed && !(lobbySettings == null))
			{
				lobbySettings.SettingsChanged += OnSettingsChanged;
				subscribed = true;
			}
		}

		private void OnEnable()
		{
			Subscribe();
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		private void OnDisable()
		{
			if (subscribed && lobbySettings != null)
			{
				lobbySettings.SettingsChanged -= OnSettingsChanged;
			}
			subscribed = false;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
		}

		private void OnDestroy()
		{
			SetCurrentMap(null);
		}

		private void OnSettingsChanged(LobbySettingsData settings)
		{
			Load(settings.MapId.ToString());
		}

		private void Load(string mapId)
		{
			MapScriptableObject mapScriptableObject = MapCatalog.Find(mapId) ?? MapCatalog.Default;
			if (mapScriptableObject == null)
			{
				Debug.LogError("[MapLoader] Yüklenecek harita yok - Assets/Resources/Maps altında kullanılabilir bir MapScriptableObject olmalı.");
			}
			else if (mapScriptableObject.UsesScene)
			{
				BindMapScene(mapScriptableObject);
			}
			else if (!(loadedMap != null) || !(loadedMapId == mapScriptableObject.MapId))
			{
				if (!string.IsNullOrEmpty(mapId) && MapCatalog.Find(mapId) == null)
				{
					Debug.LogWarning("[MapLoader] '" + mapId + "' bu build'de yok - '" + mapScriptableObject.DisplayName + "' yükleniyor. Sunucuyla farklı harita görüyor olabilirsin.");
				}
				if (loadedMap != null)
				{
					UnityEngine.Object.Destroy(loadedMap);
				}
				Transform parent = ((mapRoot != null) ? mapRoot : base.transform);
				loadedMap = UnityEngine.Object.Instantiate(mapScriptableObject.Prefab, parent);
				loadedMap.name = mapScriptableObject.Prefab.name;
				loadedMapId = mapScriptableObject.MapId;
				SetCurrentMap(mapScriptableObject);
				mapScriptableObject.Environment.Apply();
				ApplyMarkers(loadedMap.GetComponentsInChildren<MapMarker>(includeInactive: true), mapScriptableObject);
				ApplyModeObjects(loadedMap);
			}
		}

		private void ApplyModeObjects(GameObject root)
		{
			GameModeController gameModeController = ((roundManager != null) ? roundManager : GameModeController.Current);
			if (!(gameModeController == null))
			{
				int num = MapModeObject.ApplyAll(root, gameModeController.MarkerMode);
				if (num > 0)
				{
					Debug.Log($"[MapLoader] {num} moda özel obje {gameModeController.MarkerMode} için ayarlandı.");
				}
			}
		}

		private void ApplyModeObjects(Scene scene)
		{
			GameModeController gameModeController = ((roundManager != null) ? roundManager : GameModeController.Current);
			if (!(gameModeController == null))
			{
				int num = MapModeObject.ApplyAll(scene, gameModeController.MarkerMode);
				if (num > 0)
				{
					Debug.Log($"[MapLoader] {num} moda özel obje {gameModeController.MarkerMode} için ayarlandı.");
				}
			}
		}

		private void BindMapScene(MapScriptableObject map)
		{
			Scene sceneByName = SceneManager.GetSceneByName(map.SceneName);
			if (sceneByName.IsValid() && sceneByName.isLoaded && !(sceneByName.handle == boundSceneHandle))
			{
				if (loadedMap != null)
				{
					UnityEngine.Object.Destroy(loadedMap);
					loadedMap = null;
				}
				Scene activeScene = SceneManager.GetActiveScene();
				if (activeScene != sceneByName)
				{
					previousActiveScene = activeScene;
					SceneManager.SetActiveScene(sceneByName);
				}
				boundSceneHandle = sceneByName.handle;
				loadedMapId = map.MapId;
				SetCurrentMap(map);
				List<MapMarker> list = new List<MapMarker>();
				GameObject[] rootGameObjects = sceneByName.GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					list.AddRange(gameObject.GetComponentsInChildren<MapMarker>(includeInactive: true));
				}
				ApplyMarkers(list, map);
				ApplyModeObjects(sceneByName);
			}
		}

		private void OnSceneUnloaded(Scene scene)
		{
			if (scene.handle == boundHunterRoomHandle)
			{
				boundHunterRoomHandle = 0;
				if (roundManager != null)
				{
					roundManager.SetHunterRoom(Array.Empty<Transform>());
				}
			}
			if (!(scene.handle != boundSceneHandle))
			{
				boundSceneHandle = 0;
				if (previousActiveScene.IsValid() && previousActiveScene.isLoaded)
				{
					SceneManager.SetActiveScene(previousActiveScene);
				}
			}
		}

		private void TryBindHunterRoom()
		{
			GameModeDefinition gameModeDefinition = ResolveMode();
			if (gameModeDefinition == null || !gameModeDefinition.UsesHunterRoom)
			{
				return;
			}
			Scene sceneByName = SceneManager.GetSceneByName(gameModeDefinition.HunterRoomSceneName);
			if (!sceneByName.IsValid() || !sceneByName.isLoaded || sceneByName.handle == boundHunterRoomHandle)
			{
				return;
			}
			boundHunterRoomHandle = sceneByName.handle;
			Vector3 hunterRoomOffset = gameModeDefinition.HunterRoomOffset;
			List<MapMarker> list = new List<MapMarker>();
			bool flag = false;
			GameObject[] rootGameObjects = sceneByName.GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				if (hunterRoomOffset != Vector3.zero)
				{
					gameObject.transform.position += hunterRoomOffset;
					flag |= HasStaticRenderer(gameObject);
				}
				list.AddRange(gameObject.GetComponentsInChildren<MapMarker>(includeInactive: true));
			}
			if (flag)
			{
				Debug.LogWarning("[MapLoader] '" + gameModeDefinition.HunterRoomSceneName + "' static mesh iceriyor ama " + $"Hunter Room Offset {hunterRoomOffset} - static batching kose noktalarini DUNYA uzayinda " + "birlestirdigi icin transform artik cizimi surmuyor. Oda carpisma olarak taşınır, gorüntusu eski yerinde kalir. Odayi sahnede olmasi gereken yerde kur ve offset'i sifirla.");
			}
			List<Transform> list2 = new List<Transform>();
			List<Transform> list3 = new List<Transform>();
			foreach (MapMarker item in list)
			{
				if (item.Is(MapMarkerKind.HunterRoomSpawn))
				{
					list2.Add(item.transform);
				}
				if (item.Is(MapMarkerKind.ArenaSpawn))
				{
					list3.Add(item.transform);
				}
			}
			if (list2.Count == 0)
			{
				Debug.LogWarning("[MapLoader] '" + gameModeDefinition.HunterRoomSceneName + "' içinde HunterRoomSpawn marker'ı yok - Avcılar Hazırlık'ta doğrudan haritada başlayacak.");
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (roundManager != null)
			{
				roundManager.SetHunterRoom(list2.ToArray());
				if (list3.Count > 0)
				{
					roundManager.SetArena(list3.ToArray());
				}
			}
		}

		private static bool HasStaticRenderer(GameObject root)
		{
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.isStatic)
				{
					return true;
				}
			}
			return false;
		}

		private GameModeDefinition ResolveMode()
		{
			if (lobbySettings == null || !lobbySettings.IsSpawned)
			{
				return null;
			}
			return GameModeCatalog.Find(lobbySettings.CurrentSettings.ModeId.ToString()) ?? GameModeCatalog.Default;
		}

		private void ApplyMarkers(IReadOnlyList<MapMarker> markers, MapScriptableObject map)
		{
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
				if (roundManager == null)
				{
					Debug.LogWarning("[MapLoader] Oyun modu denetleyicisi yok - spawn noktaları bağlanamadı.");
					return;
				}
			}
			List<Transform> list = new List<Transform>();
			List<Transform> list2 = new List<Transform>();
			List<Transform> list3 = new List<Transform>();
			List<Transform> list4 = new List<Transform>();
			GameObject gameObject = null;
			foreach (MapMarker marker in markers)
			{
				if (marker.Is(MapMarkerKind.HunterSpawn))
				{
					list.Add(marker.transform);
				}
				if (marker.Is(MapMarkerKind.HiderSpawn))
				{
					list2.Add(marker.transform);
				}
				if (marker.Is(MapMarkerKind.LobbySpawn))
				{
					list3.Add(marker.transform);
				}
				if (marker.Is(MapMarkerKind.ArenaSpawn))
				{
					list4.Add(marker.transform);
				}
				if (marker.Is(MapMarkerKind.HunterDoor))
				{
					if (gameObject != null)
					{
						Debug.LogWarning("[MapLoader] '" + map.DisplayName + "' birden fazla HunterDoor marker'ı içeriyor - '" + gameObject.name + "' kullanılıyor, '" + marker.name + "' yok sayıldı.");
					}
					else
					{
						gameObject = marker.gameObject;
					}
				}
			}
			if (list.Count == 0)
			{
				Debug.LogWarning("[MapLoader] '" + map.DisplayName + "' içinde HunterSpawn marker'ı yok.");
			}
			if (list2.Count == 0)
			{
				Debug.LogWarning("[MapLoader] '" + map.DisplayName + "' içinde HiderSpawn marker'ı yok.");
			}
			roundManager.SetMap(list.ToArray(), list2.ToArray(), list3.ToArray(), gameObject);
			if (list4.Count > 0)
			{
				roundManager.SetArena(list4.ToArray());
			}
		}
	}
}
