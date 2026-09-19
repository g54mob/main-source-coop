using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Features.SceneManagement
{
	public interface INetworkSceneLoaderServiceFacade
	{
		UniTask LoadSceneAsync(string sceneToLoad, NetworkSceneLoadingFlags networkSceneLoadingFlags);

		UniTask LoadScenesAsync(List<string> scenesToLoad, string activeScene, NetworkSceneLoadingFlags networkSceneLoadingFlags);

		UniTask UnloadSceneAsync(string sceneToUnload, NetworkSceneLoadingFlags networkSceneLoadingFlags);

		UniTask UnloadScenesAsync(List<string> scenesToUnload, NetworkSceneLoadingFlags networkSceneLoadingFlags);
	}
}
