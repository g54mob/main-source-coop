using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts
{
	public interface ISceneLoaderService
	{
		UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant);

		UniTask LoadScenesAsync(List<string> scenesToLoad, string activeScene, bool unloadRedundant);

		UniTask UnloadSceneAsync(string sceneToUnload);

		UniTask UnloadScenesAsync(List<string> scenesToUnload);
	}
}
