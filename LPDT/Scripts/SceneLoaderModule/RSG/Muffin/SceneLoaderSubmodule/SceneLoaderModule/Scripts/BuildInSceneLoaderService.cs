using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts
{
	public class BuildInSceneLoaderService
	{
		private readonly List<string> _loadedScenes = new List<string>();

		public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant)
		{
			if (!_loadedScenes.Contains(sceneToLoad))
			{
				LoadSceneMode mode = ((!unloadRedundant) ? LoadSceneMode.Additive : LoadSceneMode.Single);
				await SceneManager.LoadSceneAsync(sceneToLoad, mode);
				_loadedScenes.Add(sceneToLoad);
			}
		}

		public AsyncOperation LoadScene(string sceneToLoad, bool unloadRedundant)
		{
			if (_loadedScenes.Contains(sceneToLoad))
			{
				throw new Exception("Scene already loaded");
			}
			LoadSceneMode mode = ((!unloadRedundant) ? LoadSceneMode.Additive : LoadSceneMode.Single);
			_loadedScenes.Add(sceneToLoad);
			return SceneManager.LoadSceneAsync(sceneToLoad, mode);
		}

		public async UniTask LoadScenesAsync(List<string> scenesToLoad, bool unloadRedundant)
		{
			if (unloadRedundant)
			{
				await UnloadScenesAsync(_loadedScenes.Except(scenesToLoad).ToList());
			}
			foreach (string item in scenesToLoad)
			{
				await LoadSceneAsync(item, unloadRedundant: false);
			}
		}

		public async UniTask UnloadSceneAsync(string sceneToUnload)
		{
			if (!_loadedScenes.Contains(sceneToUnload))
			{
				Debug.LogError("Cannot unload scene " + sceneToUnload + " as it is not loaded. Ignoring attempt");
				return;
			}
			await SceneManager.UnloadSceneAsync(sceneToUnload);
			_loadedScenes.Remove(sceneToUnload);
		}

		public async UniTask UnloadScenesAsync(List<string> scenesToUnload)
		{
			foreach (string item in scenesToUnload)
			{
				await UnloadSceneAsync(item);
			}
		}
	}
}
