using System;
using Cysharp.Threading.Tasks;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenService : ILoadingScreenService
	{
		private readonly LoadingScreenModel _loadingScreenModel;

		private readonly LoadingScreenOverlayController _overlayController;

		public bool IsVisible => _loadingScreenModel.IsVisible;

		public bool IsFading => _loadingScreenModel.IsFading;

		public bool IsContentLoadingActive => _loadingScreenModel.IsContentLoadingActive;

		public LoadingScreenShowType? ActiveShowType => _loadingScreenModel.ActiveShowType;

		public bool IsBlackoutRaised => _overlayController.IsBlackoutRaised;

		public LoadingScreenService(LoadingScreenModel loadingScreenModel, LoadingScreenOverlayController overlayController)
		{
			_loadingScreenModel = loadingScreenModel;
			_overlayController = overlayController;
		}

		public void ForceClearBlackout()
		{
			_overlayController.ForceClearBlackout();
		}

		public bool CanShow(LoadingScreenShowType type)
		{
			return _overlayController.CanShow(type);
		}

		public void Show(LoadingScreenShowType type)
		{
			ShowAsync(type).Forget();
		}

		public void Show(LoadingScreenShowType type, Action onShown)
		{
			ShowAndInvokeAsync(type, onShown).Forget();
		}

		public UniTask ShowAsync(LoadingScreenShowType type)
		{
			return _overlayController.ShowAsync(type);
		}

		public UniTask HideAsync()
		{
			LoadingScreenShowType valueOrDefault = ActiveShowType.GetValueOrDefault();
			return _overlayController.HideAsync(valueOrDefault);
		}

		public UniTask HideAsync(LoadingScreenShowType type)
		{
			return _overlayController.HideAsync(type);
		}

		public void Hide(LoadingScreenShowType type)
		{
			HideAsync(type).Forget();
		}

		public void Hide(LoadingScreenShowType type, Action onHidden)
		{
			HideAndInvokeAsync(type, onHidden).Forget();
		}

		private async UniTaskVoid ShowAndInvokeAsync(LoadingScreenShowType type, Action onShown)
		{
			await _overlayController.ShowAsync(type);
			onShown?.Invoke();
		}

		private async UniTaskVoid HideAndInvokeAsync(LoadingScreenShowType type, Action onHidden)
		{
			await _overlayController.HideAsync(type);
			onHidden?.Invoke();
		}

		public UniTask FadeOutAsync()
		{
			return _overlayController.FadeOutAsync();
		}

		public void FadeOut()
		{
			FadeOutAsync().Forget();
		}

		public UniTask DismissVisibleOverlayAsync()
		{
			return _overlayController.DismissVisibleOverlayAsync();
		}

		public void HideImmediate()
		{
			_overlayController.HideImmediate();
		}

		public void DismissForLateJoiner()
		{
			_overlayController.DismissForLateJoiner();
		}

		public void RetainBlackScreenBridgeUntilMenuEntrance()
		{
			_overlayController.RetainBlackScreenBridgeUntilMenuEntrance();
		}

		public UniTask PrepareMenuSceneEntranceAsync()
		{
			return _overlayController.PrepareMenuSceneEntranceAsync();
		}
	}
}
