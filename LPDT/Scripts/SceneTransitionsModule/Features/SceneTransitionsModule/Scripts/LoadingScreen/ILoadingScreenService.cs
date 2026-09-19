using System;
using Cysharp.Threading.Tasks;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public interface ILoadingScreenService
	{
		bool IsVisible { get; }

		bool IsFading { get; }

		bool IsContentLoadingActive { get; }

		LoadingScreenShowType? ActiveShowType { get; }

		bool IsBlackoutRaised { get; }

		void ForceClearBlackout();

		bool CanShow(LoadingScreenShowType type);

		void Show(LoadingScreenShowType type);

		void Show(LoadingScreenShowType type, Action onShown);

		UniTask ShowAsync(LoadingScreenShowType type);

		UniTask HideAsync();

		UniTask HideAsync(LoadingScreenShowType type);

		void Hide(LoadingScreenShowType type);

		void Hide(LoadingScreenShowType type, Action onHidden);

		UniTask FadeOutAsync();

		void FadeOut();

		UniTask DismissVisibleOverlayAsync();

		void HideImmediate();

		void DismissForLateJoiner();

		void RetainBlackScreenBridgeUntilMenuEntrance();

		UniTask PrepareMenuSceneEntranceAsync();
	}
}
