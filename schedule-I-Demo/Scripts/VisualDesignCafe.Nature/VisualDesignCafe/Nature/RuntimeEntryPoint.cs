using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualDesignCafe.Nature
{
	[ExecuteAlways]
	public class RuntimeEntryPoint : MonoBehaviour
	{
		private static RuntimeEntryPoint _entryPoint;

		[SerializeField]
		private ShaderResources _shaderResources;

		[NonSerialized]
		internal bool _isDestroyed = false;

		public static RuntimeEntryPoint EntryPoint => _entryPoint;

		public ShaderResources Resources => _shaderResources;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		internal static void Load()
		{
			if (_entryPoint == null)
			{
				_entryPoint = FindExistingEntryPointInScene();
				if (_entryPoint == null)
				{
					_entryPoint = CreateEntryPoint();
				}
			}
		}

		internal static void Unload()
		{
			if (_entryPoint != null)
			{
				UnityEngine.Object.DestroyImmediate(_entryPoint.gameObject);
			}
		}

		private static RuntimeEntryPoint CreateEntryPoint()
		{
			GameObject gameObject = new GameObject("Runtime Entry Point");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return gameObject.AddComponent<RuntimeEntryPoint>();
		}

		private static RuntimeEntryPoint FindExistingEntryPointInScene()
		{
			RuntimeEntryPoint[] array = UnityEngine.Object.FindObjectsOfType<RuntimeEntryPoint>();
			foreach (RuntimeEntryPoint runtimeEntryPoint in array)
			{
				if (!runtimeEntryPoint._isDestroyed)
				{
					return runtimeEntryPoint;
				}
			}
			for (int j = 0; j < SceneManager.sceneCount; j++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(j);
				if (!sceneAt.IsValid() || !sceneAt.isLoaded)
				{
					continue;
				}
				GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					RuntimeEntryPoint runtimeEntryPoint2 = gameObject.GetComponent<RuntimeEntryPoint>();
					if (runtimeEntryPoint2 != null && runtimeEntryPoint2._isDestroyed)
					{
						runtimeEntryPoint2 = null;
					}
					if (runtimeEntryPoint2 != null)
					{
						return runtimeEntryPoint2;
					}
				}
			}
			return null;
		}

		private void LoadResources()
		{
			if (_shaderResources == null)
			{
				_shaderResources = new ShaderResources();
			}
			_shaderResources.Initialize();
		}

		private void UnloadResources()
		{
			_shaderResources?.Destroy();
		}

		private void Awake()
		{
			LoadResources();
		}

		private void OnDestroy()
		{
			_isDestroyed = true;
			UnloadResources();
			_entryPoint = null;
		}
	}
}
