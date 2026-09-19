using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine.SceneManagement;

namespace Features.SceneManagement
{
	public class NetworkSceneLoader : INetworkSceneLoader
	{
		private readonly MultiplayerModel _multiplayerModel;

		public NetworkSceneLoader(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public async UniTask LoadScene(string scenePath, bool unloadRedundant)
		{
			if (IsValid())
			{
				LoadSceneMode loadSceneMode = ((!unloadRedundant) ? LoadSceneMode.Additive : LoadSceneMode.Single);
				if (loadSceneMode != LoadSceneMode.Additive || !IsSceneLoaded(scenePath))
				{
					await _multiplayerModel.NetworkRunner.LoadScene(SceneRef.FromPath(scenePath), loadSceneMode);
				}
			}
		}

		public async UniTask LoadScenes(List<string> scenesPaths, string activeScene, bool unloadRedundant)
		{
			if (unloadRedundant)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					list.Add(SceneManager.GetSceneAt(i).name);
				}
				await UnloadScenes(list);
			}
			foreach (string scenesPath in scenesPaths)
			{
				await LoadScene(scenesPath, unloadRedundant);
			}
			SetActiveScene(activeScene);
		}

		public async UniTask UnloadScene(string scenePath)
		{
			if (IsValid() && scenePath != null)
			{
				await _multiplayerModel.NetworkRunner.UnloadScene(SceneRef.FromPath(scenePath));
			}
		}

		public async UniTask UnloadScenes(List<string> scenesPaths)
		{
			foreach (string scenesPath in scenesPaths)
			{
				await UnloadScene(scenesPath);
			}
		}

		private bool IsValid()
		{
			if (_multiplayerModel.NetworkRunner != null)
			{
				return _multiplayerModel.NetworkRunner.IsSceneAuthority;
			}
			return false;
		}

		private static bool IsSceneLoaded(string scenePath)
		{
			Scene sceneByName = SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(scenePath));
			if (sceneByName.IsValid())
			{
				return sceneByName.isLoaded;
			}
			return false;
		}

		private void SetActiveScene(string activeScene)
		{
			if (!(SceneManager.GetActiveScene().name == activeScene))
			{
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeScene));
			}
		}
	}
}
