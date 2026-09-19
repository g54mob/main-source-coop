using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Features.SceneManagement
{
	public interface INetworkSceneLoader
	{
		UniTask LoadScene(string scenePath, bool unloadRedundant);

		UniTask LoadScenes(List<string> scenesPaths, string activeScene, bool unloadRedundant);

		UniTask UnloadScene(string scenePath);

		UniTask UnloadScenes(List<string> scenesPaths);
	}
}
