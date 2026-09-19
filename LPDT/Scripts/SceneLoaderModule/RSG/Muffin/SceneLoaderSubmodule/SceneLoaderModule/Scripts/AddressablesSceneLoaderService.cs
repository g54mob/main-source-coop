using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts
{
	public class AddressablesSceneLoaderService
	{
		private Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes { get; } = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

		public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant)
		{
			if (!_loadedScenes.ContainsKey(sceneToLoad))
			{
				LoadSceneMode loadMode = ((!unloadRedundant) ? LoadSceneMode.Additive : LoadSceneMode.Single);
				AsyncOperationHandle<SceneInstance> value = Addressables.LoadSceneAsync(sceneToLoad, loadMode);
				_loadedScenes.Add(sceneToLoad, value);
				await value.Task;
			}
		}

		public AsyncOperationHandle<SceneInstance> LoadScene(string sceneToLoad, bool unloadRedundant)
		{
			if (_loadedScenes.ContainsKey(sceneToLoad))
			{
				throw new Exception("Scene already loaded");
			}
			LoadSceneMode loadMode = ((!unloadRedundant) ? LoadSceneMode.Additive : LoadSceneMode.Single);
			AsyncOperationHandle<SceneInstance> asyncOperationHandle = Addressables.LoadSceneAsync(sceneToLoad, loadMode);
			_loadedScenes.Add(sceneToLoad, asyncOperationHandle);
			return asyncOperationHandle;
		}

		public async UniTask LoadScenesAsync(List<string> scenesToLoad, bool unloadRedundant)
		{
			if (unloadRedundant)
			{
				await UnloadScenesAsync(_loadedScenes.Keys.Except(scenesToLoad).ToList());
			}
			foreach (string item in scenesToLoad)
			{
				await LoadSceneAsync(item, unloadRedundant: false);
			}
		}

		public async UniTask UnloadSceneAsync(string sceneToUnload)
		{
			if (!_loadedScenes.ContainsKey(sceneToUnload))
			{
				Debug.LogError("Cannot unload scene " + sceneToUnload + " as it is not loaded. Ignoring attempt");
				return;
			}
			await Addressables.UnloadSceneAsync(_loadedScenes[sceneToUnload]).Task;
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
