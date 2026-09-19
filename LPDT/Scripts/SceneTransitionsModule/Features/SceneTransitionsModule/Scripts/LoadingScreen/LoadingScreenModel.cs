using System;
using Cysharp.Threading.Tasks;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenModel
	{
		private UniTaskCompletionSource _animationTcs;

		private int _animationRequestId;

		public bool IsVisible { get; private set; }

		public bool IsFading { get; private set; }

		public bool IsContentLoadingActive { get; private set; }

		public LoadingScreenShowType? ActiveShowType { get; private set; }

		public event Action OnEndedFadeOut;

		internal event Action<LoadingScreenPreset> OnPresetApplied;

		internal event Action<bool> OnOverlayActiveChanged;

		internal event Action<bool> OnBlocksRaycastsChanged;

		internal event Action<bool> OnScreensContentVisibleChanged;

		internal event Action OnStopActiveContentRequested;

		internal event Action OnHideImmediateRequested;

		internal event Action<LoadingScreenPreset, bool, int> OnFadeInRequested;

		internal event Action<LoadingScreenPreset, int> OnFadeOutRequested;

		internal event Action<LoadingScreenPreset, bool> OnShowInstantRequested;

		internal void ApplyPreset(LoadingScreenPreset preset)
		{
			this.OnPresetApplied?.Invoke(preset);
		}

		internal void SetOverlayActive(bool active)
		{
			this.OnOverlayActiveChanged?.Invoke(active);
		}

		internal void SetBlocksRaycasts(bool value)
		{
			this.OnBlocksRaycastsChanged?.Invoke(value);
		}

		internal void SetScreensContentVisible(bool visible)
		{
			this.OnScreensContentVisibleChanged?.Invoke(visible);
		}

		internal void StopActiveContent()
		{
			this.OnStopActiveContentRequested?.Invoke();
		}

		internal void HideImmediate()
		{
			this.OnHideImmediateRequested?.Invoke();
		}

		internal void ShowInstant(LoadingScreenPreset preset, bool showContentAfterFade)
		{
			this.OnShowInstantRequested?.Invoke(preset, showContentAfterFade);
		}

		internal UniTask FadeInAsync(LoadingScreenPreset preset, bool showContentAfterFade)
		{
			CompleteAnimation();
			int arg = ++_animationRequestId;
			_animationTcs = new UniTaskCompletionSource();
			this.OnFadeInRequested?.Invoke(preset, showContentAfterFade, arg);
			return _animationTcs.Task;
		}

		internal UniTask FadeOutAsync(LoadingScreenPreset preset)
		{
			CompleteAnimation();
			int arg = ++_animationRequestId;
			_animationTcs = new UniTaskCompletionSource();
			this.OnFadeOutRequested?.Invoke(preset, arg);
			return _animationTcs.Task;
		}

		internal void SetVisible(bool isVisible)
		{
			IsVisible = isVisible;
		}

		internal void SetFading(bool isFading)
		{
			IsFading = isFading;
		}

		internal void SetContentLoadingActive(bool isContentLoadingActive)
		{
			IsContentLoadingActive = isContentLoadingActive;
		}

		internal void SetActiveShowType(LoadingScreenShowType? activeShowType)
		{
			ActiveShowType = activeShowType;
		}

		internal void CompleteAnimation()
		{
			_animationRequestId++;
			UniTaskCompletionSource animationTcs = _animationTcs;
			_animationTcs = null;
			animationTcs?.TrySetResult();
		}

		internal void CompleteAnimation(int requestId)
		{
			if (requestId == _animationRequestId)
			{
				UniTaskCompletionSource animationTcs = _animationTcs;
				_animationTcs = null;
				animationTcs?.TrySetResult();
			}
		}

		internal void Reset()
		{
			IsVisible = false;
			IsFading = false;
			IsContentLoadingActive = false;
			ActiveShowType = null;
		}

		internal void InvokeEndedFadeOut()
		{
			this.OnEndedFadeOut?.Invoke();
		}
	}
}
