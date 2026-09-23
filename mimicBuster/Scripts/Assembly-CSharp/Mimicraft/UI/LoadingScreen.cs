using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class LoadingScreen
	{
		private class Driver : MonoBehaviour
		{
			private const float ConnectWeight = 0.2f;

			private const float SceneWeight = 0.65f;

			private bool watching;

			private float giveUpAt;

			private NetworkSceneManager sceneManager;

			private AsyncOperation loading;

			private float sceneProgress;

			private int scenesDone;

			private const float QuietSeconds = 0.75f;

			private float lastSceneEventAt;

			public void Begin()
			{
				watching = true;
				giveUpAt = Time.unscaledTime + 90f;
				sceneProgress = 0f;
				scenesDone = 0;
				loading = null;
				lastSceneEventAt = Time.unscaledTime;
				Subscribe();
			}

			public void End()
			{
				watching = false;
				Unsubscribe();
				loading = null;
			}

			private void OnDisable()
			{
				Unsubscribe();
			}

			private void Update()
			{
				if (!watching)
				{
					return;
				}
				Subscribe();
				Report();
				if (IsWorldReady() || Time.unscaledTime >= giveUpAt)
				{
					if (Time.unscaledTime >= giveUpAt && !IsWorldReady())
					{
						Debug.LogWarning($"[Yukleme] {90f:0} saniyede oyun hazir olmadi - " + "yukleme ekrani kaldiriliyor. Mod sahnesi ya da harita yuklenmemis olabilir.");
					}
					Hide();
				}
			}

			private bool IsWorldReady()
			{
				if (GameModeController.Current != null)
				{
					return Time.unscaledTime - lastSceneEventAt >= 0.75f;
				}
				return false;
			}

			private void Report()
			{
				NetworkManager singleton = NetworkManager.Singleton;
				if (!(singleton != null) || (!singleton.IsConnectedClient && !singleton.IsServer))
				{
					SetStep("Loading.Connecting");
					SetProgress(0.1f);
					return;
				}
				if (loading != null)
				{
					sceneProgress = Mathf.Clamp01(loading.progress / 0.9f);
				}
				float num = 1f - Mathf.Pow(0.5f, (float)scenesDone + sceneProgress);
				SetProgress(0.2f + 0.65f * num);
			}

			private void Subscribe()
			{
				NetworkManager singleton = NetworkManager.Singleton;
				if (!(singleton == null) && sceneManager != singleton.SceneManager)
				{
					Unsubscribe();
					sceneManager = singleton.SceneManager;
					if (sceneManager != null)
					{
						sceneManager.OnSceneEvent += OnSceneEvent;
					}
				}
			}

			private void Unsubscribe()
			{
				if (sceneManager != null)
				{
					sceneManager.OnSceneEvent -= OnSceneEvent;
				}
				sceneManager = null;
			}

			private void OnSceneEvent(SceneEvent sceneEvent)
			{
				lastSceneEventAt = Time.unscaledTime;
				switch (sceneEvent.SceneEventType)
				{
				case SceneEventType.Load:
					loading = sceneEvent.AsyncOperation;
					sceneProgress = 0f;
					SetStep(StepFor(sceneEvent.SceneName));
					break;
				case SceneEventType.LoadComplete:
					loading = null;
					sceneProgress = 0f;
					scenesDone++;
					break;
				case SceneEventType.Synchronize:
					SetStep("Loading.Syncing");
					break;
				case SceneEventType.SynchronizeComplete:
					SetStep("Loading.GettingReady");
					break;
				}
			}

			private static string StepFor(string sceneName)
			{
				if (string.IsNullOrEmpty(sceneName))
				{
					return "Loading.GettingReady";
				}
				foreach (MapScriptableObject item in MapCatalog.All)
				{
					if (item != null && item.SceneName == sceneName)
					{
						return "Loading.LoadingMap";
					}
				}
				return "Loading.LoadingScene";
			}
		}

		private const string ResourcePath = "LoadingScreen";

		private const float GiveUpSeconds = 90f;

		private static LoadingScreenView view;

		private static Driver driver;

		private static bool warned;

		public static bool IsVisible
		{
			get
			{
				if (view != null)
				{
					return view.IsVisible;
				}
				return false;
			}
		}

		public static void Show(string stepKey)
		{
			if (EnsureView())
			{
				view.Show(stepKey);
				EnsureDriver().Begin();
			}
		}

		public static void ShowWaiting(string stepKey)
		{
			if (EnsureView())
			{
				if (driver != null)
				{
					driver.End();
				}
				view.Show(stepKey);
			}
		}

		public static void SetStep(string stepKey)
		{
			if (view != null && view.IsVisible)
			{
				view.SetStep(stepKey);
			}
		}

		public static void SetProgress(float value)
		{
			if (view != null && view.IsVisible)
			{
				view.SetProgress(value);
			}
		}

		public static void Hide()
		{
			if (view != null && view.IsVisible)
			{
				view.Hide();
			}
			if (driver != null)
			{
				driver.End();
			}
		}

		private static bool EnsureView()
		{
			if (view != null)
			{
				return true;
			}
			view = Object.FindFirstObjectByType<LoadingScreenView>(FindObjectsInactive.Include);
			if (view != null)
			{
				Object.DontDestroyOnLoad(view.transform.root.gameObject);
				return true;
			}
			GameObject gameObject = Resources.Load<GameObject>("LoadingScreen");
			if (gameObject == null)
			{
				if (!warned)
				{
					warned = true;
					Debug.LogWarning("[Yukleme] Resources/LoadingScreen bulunamadi - yukleme ekrani gosterilmeyecek." + Elsewhere());
				}
				return false;
			}
			GameObject gameObject2 = Object.Instantiate(gameObject);
			gameObject2.name = gameObject.name;
			Object.DontDestroyOnLoad(gameObject2);
			view = gameObject2.GetComponentInChildren<LoadingScreenView>(includeInactive: true);
			return view != null;
		}

		private static string Elsewhere()
		{
			return " Uzerinde LoadingScreenView olan bir prefab'i bir Resources klasorune 'LoadingScreen' adiyla koy.";
		}

		private static Driver EnsureDriver()
		{
			if (driver != null)
			{
				return driver;
			}
			GameObject gameObject = new GameObject("LoadingScreen");
			Object.DontDestroyOnLoad(gameObject);
			driver = gameObject.AddComponent<Driver>();
			return driver;
		}
	}
}
