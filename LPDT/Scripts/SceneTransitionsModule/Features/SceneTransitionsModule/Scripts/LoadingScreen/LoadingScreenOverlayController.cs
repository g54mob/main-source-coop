using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenOverlayController
	{
		private readonly ILoadingScreenPresetResolver _presetResolver;

		private readonly LoadingScreenModel _loadingScreenModel;

		private LoadingScreenPreset _activePreset;

		private float _shownAtUnscaledTime;

		private float _minDisplayStartUnscaledTime;

		private int _operationGeneration;

		public bool IsBlackoutRaised
		{
			get
			{
				if (BlackScreenWindow.Instance != null)
				{
					return BlackScreenWindow.Instance.IsShown;
				}
				return false;
			}
		}

		public LoadingScreenOverlayController(ILoadingScreenPresetResolver presetResolver, LoadingScreenModel loadingScreenModel)
		{
			_presetResolver = presetResolver;
			_loadingScreenModel = loadingScreenModel;
		}

		private int BeginOperation()
		{
			return ++_operationGeneration;
		}

		private bool IsCurrent(int generation)
		{
			return generation == _operationGeneration;
		}

		public bool CanShow(LoadingScreenShowType type)
		{
			LoadingScreenPreset showPreset = _presetResolver.GetShowPreset(type);
			if (showPreset == null || !showPreset.IsAvailable)
			{
				return false;
			}
			if (!TryGetBlockingActiveShowType(out var blockingType))
			{
				return true;
			}
			if (blockingType != type)
			{
				return _presetResolver.CanOverrideActiveShowType(type, blockingType);
			}
			return true;
		}

		public async UniTask ShowAsync(LoadingScreenShowType type)
		{
			LoadingScreenPreset preset = _presetResolver.GetShowPreset(type);
			LoadingScreenShowType blockingType;
			if (preset == null || !preset.IsAvailable)
			{
				Debug.LogError($"Loading screen preset '{type}' is not configured or not available.");
			}
			else if (TryGetBlockingActiveShowType(out blockingType))
			{
				if (blockingType != type && _presetResolver.CanOverrideActiveShowType(type, blockingType))
				{
					int overrideGeneration = BeginOperation();
					await RefreshContentLoadingAsync(type, preset);
					RaiseBlackScreenIfCurrent(overrideGeneration, preset);
				}
			}
			else
			{
				int generation = BeginOperation();
				PrepareShow(type, preset);
				await PlayShowAnimationAsync(preset);
				RaiseBlackScreenIfCurrent(generation, preset);
			}
		}

		private void RaiseBlackScreenIfCurrent(int generation, LoadingScreenPreset preset)
		{
			if (IsCurrent(generation) && preset.UsesBlackScreenWindow)
			{
				BlackScreenWindow.Instance?.Show();
			}
		}

		public async UniTask HideAsync(LoadingScreenShowType type)
		{
			int generation = BeginOperation();
			await HideAsyncInternal(type, generation);
			if (IsCurrent(generation) && _loadingScreenModel.IsVisible)
			{
				LoadingScreenShowType type2 = _loadingScreenModel.ActiveShowType ?? type;
				await HideAsyncInternal(type2, generation);
			}
		}

		private async UniTask HideAsyncInternal(LoadingScreenShowType type, int generation)
		{
			if (!_loadingScreenModel.IsVisible)
			{
				HideBlackScreenOverlay();
				return;
			}
			LoadingScreenPreset hidePreset = _presetResolver.GetShowPreset(type) ?? _activePreset;
			if (hidePreset == null)
			{
				HideImmediate();
				_loadingScreenModel.InvokeEndedFadeOut();
				return;
			}
			await WaitForMinDisplayAsync(hidePreset);
			if (!IsCurrent(generation))
			{
				return;
			}
			if (!_loadingScreenModel.IsVisible)
			{
				HideBlackScreenOverlay();
				return;
			}
			_loadingScreenModel.CompleteAnimation();
			HideBlackScreenOverlay();
			await _loadingScreenModel.FadeOutAsync(hidePreset);
			if (IsCurrent(generation))
			{
				HideImmediate();
				_loadingScreenModel.InvokeEndedFadeOut();
			}
		}

		public async UniTask FadeOutAsync()
		{
			if (ShouldUseHideInsteadOfFadeOut())
			{
				await HideAsync(_loadingScreenModel.ActiveShowType ?? LoadingScreenShowType.ShowScreenWithFade);
				return;
			}
			if (_loadingScreenModel.IsVisible && _activePreset != null)
			{
				await HideAsync(_loadingScreenModel.ActiveShowType ?? _activePreset.ShowType);
				return;
			}
			LoadingScreenPreset showPreset = _presetResolver.GetShowPreset(LoadingScreenShowType.ShowFade);
			if (showPreset == null || !showPreset.IsAvailable)
			{
				Debug.LogError($"Loading screen preset '{LoadingScreenShowType.ShowFade}' is not configured or not available.");
				return;
			}
			int generation = BeginOperation();
			PrepareShow(LoadingScreenShowType.ShowFade, showPreset);
			_loadingScreenModel.ShowInstant(showPreset, showContentAfterFade: false);
			HideBlackScreenOverlay();
			await _loadingScreenModel.FadeOutAsync(showPreset);
			if (IsCurrent(generation))
			{
				HideImmediate();
				_loadingScreenModel.InvokeEndedFadeOut();
			}
		}

		public UniTask DismissVisibleOverlayAsync()
		{
			if (_loadingScreenModel.IsContentLoadingActive)
			{
				return HideAsync(_loadingScreenModel.ActiveShowType ?? LoadingScreenShowType.ShowScreenWithFade);
			}
			return FadeOutAsync();
		}

		public void HideImmediate()
		{
			_loadingScreenModel.HideImmediate();
			_activePreset = null;
			_minDisplayStartUnscaledTime = 0f;
			_shownAtUnscaledTime = 0f;
			HideBlackScreenOverlay();
			_loadingScreenModel.Reset();
		}

		public void ForceClearBlackout()
		{
			BeginOperation();
			HideImmediate();
			_loadingScreenModel.InvokeEndedFadeOut();
		}

		public void DismissForLateJoiner()
		{
			HideImmediate();
			_loadingScreenModel.InvokeEndedFadeOut();
		}

		public void HideBlackScreenOverlay()
		{
			BlackScreenWindow.Instance?.Hide();
		}

		public void RetainBlackScreenBridgeUntilMenuEntrance()
		{
			if (_loadingScreenModel.ActiveShowType.HasValue)
			{
				BlackScreenWindow.Instance?.RetainAsBridge(_loadingScreenModel.ActiveShowType.Value);
			}
		}

		public async UniTask PrepareMenuSceneEntranceAsync()
		{
			if (BlackScreenWindow.Instance != null && BlackScreenWindow.Instance.TryConsumeBridge(out var showType))
			{
				RestoreOverlayInstant(showType);
				HideBlackScreenOverlay();
				await HideAsync(showType);
				return;
			}
			HideBlackScreenOverlay();
			if (_loadingScreenModel.IsContentLoadingActive || _loadingScreenModel.IsVisible)
			{
				await DismissVisibleOverlayAsync();
			}
		}

		private void RestoreOverlayInstant(LoadingScreenShowType showType)
		{
			LoadingScreenPreset showPreset = _presetResolver.GetShowPreset(showType);
			if (showPreset == null || !showPreset.IsAvailable)
			{
				Debug.LogError($"Loading screen preset '{showType}' is not configured or not available.");
				return;
			}
			PrepareShow(showType, showPreset);
			_minDisplayStartUnscaledTime = Time.unscaledTime;
			_loadingScreenModel.ShowInstant(showPreset, showContentAfterFade: true);
		}

		private bool TryGetBlockingActiveShowType(out LoadingScreenShowType blockingType)
		{
			if ((_loadingScreenModel.IsVisible || _loadingScreenModel.IsContentLoadingActive) && _loadingScreenModel.ActiveShowType.HasValue)
			{
				blockingType = _loadingScreenModel.ActiveShowType.Value;
				return true;
			}
			if (BlackScreenWindow.Instance != null && BlackScreenWindow.Instance.TryPeekBridge(out blockingType))
			{
				return true;
			}
			blockingType = LoadingScreenShowType.ShowFade;
			return false;
		}

		private async UniTask RefreshContentLoadingAsync(LoadingScreenShowType type, LoadingScreenPreset preset)
		{
			PrepareShow(type, preset);
			_minDisplayStartUnscaledTime = 0f;
			await PlayShowAnimationAsync(preset);
		}

		private async UniTask PlayShowAnimationAsync(LoadingScreenPreset preset)
		{
			bool hasScreenContent = preset.HasScreenContent;
			if (preset.FadeInDuration > 0f)
			{
				await _loadingScreenModel.FadeInAsync(preset, hasScreenContent);
			}
			else
			{
				_loadingScreenModel.ShowInstant(preset, hasScreenContent);
			}
			_minDisplayStartUnscaledTime = Time.unscaledTime;
		}

		private void PrepareShow(LoadingScreenShowType type, LoadingScreenPreset preset)
		{
			_loadingScreenModel.StopActiveContent();
			_activePreset = preset;
			_loadingScreenModel.SetActiveShowType(type);
			_shownAtUnscaledTime = Time.unscaledTime;
			if (preset.IsContentLoadingMode)
			{
				_loadingScreenModel.SetContentLoadingActive(isContentLoadingActive: true);
			}
			_loadingScreenModel.ApplyPreset(preset);
			_loadingScreenModel.SetScreensContentVisible(visible: false);
			_loadingScreenModel.SetOverlayActive(active: true);
			_loadingScreenModel.SetBlocksRaycasts(value: true);
			_loadingScreenModel.SetVisible(isVisible: true);
		}

		private async UniTask WaitForMinDisplayAsync(LoadingScreenPreset hidePreset)
		{
			LoadingScreenPreset loadingScreenPreset = _activePreset ?? hidePreset;
			if (loadingScreenPreset == null || !loadingScreenPreset.UseUnscaledTime || loadingScreenPreset.MinDisplayDuration <= 0f)
			{
				return;
			}
			float num = ((_minDisplayStartUnscaledTime > 0f) ? _minDisplayStartUnscaledTime : _shownAtUnscaledTime);
			if (!(num <= 0f))
			{
				float num2 = Time.unscaledTime - num;
				if (!(num2 >= loadingScreenPreset.MinDisplayDuration))
				{
					await UniTask.Delay(Mathf.CeilToInt((loadingScreenPreset.MinDisplayDuration - num2) * 1000f), ignoreTimeScale: true);
				}
			}
		}

		private bool ShouldUseHideInsteadOfFadeOut()
		{
			if (_loadingScreenModel.IsVisible && _activePreset != null)
			{
				return _activePreset.IsContentLoadingMode;
			}
			return false;
		}
	}
}
