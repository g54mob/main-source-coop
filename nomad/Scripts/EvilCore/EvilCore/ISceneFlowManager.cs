using System;
using Cysharp.Threading.Tasks;

namespace EvilCore
{
	public interface ISceneFlowManager
	{
		bool IsGameSceneLoaded { get; }

		bool IsMainMenuSceneLoaded { get; }

		bool IsGameMenuSceneLoaded { get; }

		bool IsTransitioning { get; }

		event Action OnGameSceneLoaded;

		event Action OnGameSceneUnloaded;

		event Action OnMainMenuSceneLoaded;

		event Action OnMainMenuSceneUnloaded;

		event Action OnGameMenuSceneLoaded;

		event Action OnGameMenuSceneUnloaded;

		event Action<string> OnSceneLoaded;

		event Action<string> OnSceneUnloaded;

		UniTask LoadSceneAsync(string sceneName, bool setActive = false);

		UniTask UnloadSceneAsync(string sceneName);

		bool IsSceneLoaded(string sceneName);

		UniTask LoadGameSceneAsync();

		UniTask UnloadGameSceneAsync();

		UniTask LoadMainMenuSceneAsync();

		UniTask UnloadMainMenuSceneAsync();

		UniTask LoadGameMenuSceneAsync();

		UniTask UnloadGameMenuSceneAsync();

		UniTask TransitionToGameAsync();

		UniTask TransitionToMainMenuAsync();
	}
}
