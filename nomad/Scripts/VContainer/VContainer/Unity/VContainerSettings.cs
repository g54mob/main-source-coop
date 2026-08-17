using UnityEngine;
using UnityEngine.SceneManagement;

namespace VContainer.Unity
{
	public sealed class VContainerSettings : ScriptableObject
	{
		private static LifetimeScope rootLifetimeScopeInstance;

		[SerializeField]
		[Tooltip("Set the Prefab to be the parent of the entire Project.")]
		public LifetimeScope RootLifetimeScope;

		[SerializeField]
		[Tooltip("Enables the collection of information that can be viewed in the VContainerDiagnosticsWindow. Note: Performance degradation")]
		public bool EnableDiagnostics;

		[SerializeField]
		[Tooltip("Disables script modification for LifetimeScope scripts.")]
		public bool DisableScriptModifier;

		[SerializeField]
		[Tooltip("Removes (Clone) postfix in IObjectResolver.Instantiate() and IContainerBuilder.RegisterComponentInNewPrefab().")]
		public bool RemoveClonePostfix;

		public static VContainerSettings Instance { get; private set; }

		public static bool DiagnosticsEnabled
		{
			get
			{
				if (Instance != null)
				{
					return Instance.EnableDiagnostics;
				}
				return false;
			}
		}

		public LifetimeScope GetOrCreateRootLifetimeScopeInstance()
		{
			if (RootLifetimeScope != null && rootLifetimeScopeInstance == null)
			{
				bool activeSelf = RootLifetimeScope.gameObject.activeSelf;
				RootLifetimeScope.gameObject.SetActive(value: false);
				rootLifetimeScopeInstance = Object.Instantiate(RootLifetimeScope);
				Object.DontDestroyOnLoad(rootLifetimeScopeInstance);
				rootLifetimeScopeInstance.gameObject.SetActive(value: true);
				RootLifetimeScope.gameObject.SetActive(activeSelf);
			}
			return rootLifetimeScopeInstance;
		}

		public bool IsRootLifetimeScopeInstance(LifetimeScope lifetimeScope)
		{
			if (!(RootLifetimeScope == lifetimeScope))
			{
				return rootLifetimeScopeInstance == lifetimeScope;
			}
			return true;
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				Instance = this;
				Scene activeScene = SceneManager.GetActiveScene();
				if (activeScene.isLoaded)
				{
					OnFirstSceneLoaded(activeScene, LoadSceneMode.Single);
					return;
				}
				SceneManager.sceneLoaded -= OnFirstSceneLoaded;
				SceneManager.sceneLoaded += OnFirstSceneLoaded;
			}
		}

		private void OnDisable()
		{
			Instance = null;
		}

		private void OnFirstSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (RootLifetimeScope != null && RootLifetimeScope.autoRun && (rootLifetimeScopeInstance == null || rootLifetimeScopeInstance.Container == null))
			{
				GetOrCreateRootLifetimeScopeInstance();
			}
			SceneManager.sceneLoaded -= OnFirstSceneLoaded;
		}
	}
}
