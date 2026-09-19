using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenPresenter : PresenterBehaviour<LoadingScreenViewBase>
	{
		private readonly LoadingScreenWindow _loadingScreenWindow;

		private readonly LoadingScreenModel _model;

		private ScreensPresenter _screensPresenter;

		private bool _showScreensContent;

		private int _animationVersion;

		public LoadingScreenPresenter(LoadingScreenWindow loadingScreenWindow, LoadingScreenModel model)
		{
			_loadingScreenWindow = loadingScreenWindow;
			_model = model;
		}

		protected override void OnViewSet()
		{
			_screensPresenter = _loadingScreenWindow.GetPresenterForView<ScreensPresenter>(base.View.ScreensView);
			base.View.SetVisible(visible: false);
			_model.OnPresetApplied += HandlePresetApplied;
			_model.OnOverlayActiveChanged += HandleOverlayActive;
			_model.OnBlocksRaycastsChanged += HandleBlocksRaycasts;
			_model.OnScreensContentVisibleChanged += HandleScreensContentVisible;
			_model.OnStopActiveContentRequested += HandleStopActiveContent;
			_model.OnHideImmediateRequested += HandleHideImmediate;
			_model.OnFadeInRequested += HandleFadeInAsync;
			_model.OnFadeOutRequested += HandleFadeOutAsync;
			_model.OnShowInstantRequested += HandleShowInstant;
		}

		protected override void OnDisposed()
		{
			_model.OnPresetApplied -= HandlePresetApplied;
			_model.OnOverlayActiveChanged -= HandleOverlayActive;
			_model.OnBlocksRaycastsChanged -= HandleBlocksRaycasts;
			_model.OnScreensContentVisibleChanged -= HandleScreensContentVisible;
			_model.OnStopActiveContentRequested -= HandleStopActiveContent;
			_model.OnHideImmediateRequested -= HandleHideImmediate;
			_model.OnFadeInRequested -= HandleFadeInAsync;
			_model.OnFadeOutRequested -= HandleFadeOutAsync;
			_model.OnShowInstantRequested -= HandleShowInstant;
		}

		private void HandlePresetApplied(LoadingScreenPreset preset)
		{
			_animationVersion++;
			_showScreensContent = preset.HasScreenContent;
			base.View.SetBackgroundActive(active: true);
			_screensPresenter.ApplyPreset(preset);
			HandleScreensContentVisible(visible: false);
		}

		private void HandleOverlayActive(bool active)
		{
			base.View.SetVisible(active);
		}

		private void HandleBlocksRaycasts(bool value)
		{
			base.View.SetInteractable(value);
		}

		private void HandleScreensContentVisible(bool visible)
		{
			bool screensRootVisible = visible && _showScreensContent;
			base.View.SetScreensRootVisible(screensRootVisible);
			_screensPresenter.SetContentVisible(visible);
		}

		private void HandleStopActiveContent()
		{
			_screensPresenter?.StopTips();
		}

		private void HandleHideImmediate()
		{
			_animationVersion++;
			HandleStopActiveContent();
			_model.SetFading(isFading: false);
			base.View.SetVisible(visible: false);
		}

		private async void HandleFadeInAsync(LoadingScreenPreset preset, bool showContentAfterFade, int requestId)
		{
			int animationVersion = ++_animationVersion;
			_model.SetFading(isFading: true);
			base.View.SetAlpha(0f);
			await base.View.FadeAsync(preset.TargetAlpha, preset.FadeInDuration, preset.UseUnscaledTime);
			if (animationVersion != _animationVersion)
			{
				_model.CompleteAnimation(requestId);
				return;
			}
			CompleteFadeIn(preset, showContentAfterFade);
			_model.CompleteAnimation(requestId);
		}

		private async void HandleFadeOutAsync(LoadingScreenPreset preset, int requestId)
		{
			int animationVersion = ++_animationVersion;
			_model.SetFading(isFading: true);
			await base.View.FadeAsync(0f, preset.FadeOutDuration, preset.UseUnscaledTime);
			if (animationVersion != _animationVersion)
			{
				_model.CompleteAnimation(requestId);
				return;
			}
			_model.SetFading(isFading: false);
			_model.CompleteAnimation(requestId);
		}

		private void HandleShowInstant(LoadingScreenPreset preset, bool showContentAfterFade)
		{
			_animationVersion++;
			base.View.SetAlpha(preset.TargetAlpha);
			CompleteFadeIn(preset, showContentAfterFade);
		}

		private void CompleteFadeIn(LoadingScreenPreset preset, bool showContentAfterFade)
		{
			if (showContentAfterFade)
			{
				HandleScreensContentVisible(visible: true);
			}
			_model.SetFading(isFading: false);
		}
	}
}
