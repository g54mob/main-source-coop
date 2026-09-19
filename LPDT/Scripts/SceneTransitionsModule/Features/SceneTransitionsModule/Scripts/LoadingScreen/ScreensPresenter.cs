using Features.GlobalFactories;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class ScreensPresenter : PresenterBehaviour<ScreensViewBase>
	{
		private readonly ILoadingScreenPresetResolver _presetResolver;

		private readonly InjectedPrefabFactory _prefabFactory;

		private readonly ILoadingScreenUIService _loadingScreenUIService;

		private bool _showScreensContainer;

		private bool _isContentVisible;

		private GameObject _activeScreenObject;

		private LoadingScreenContentViewBase _activeScreenContent;

		private LoadingScreenScreenPreset _activeScreenPreset;

		private LoadingScreenTipsPresenter _tipsPresenter;

		private bool _useUnscaledTime = true;

		public ScreensPresenter(ILoadingScreenPresetResolver presetResolver, InjectedPrefabFactory prefabFactory, ILoadingScreenUIService loadingScreenUIService)
		{
			_presetResolver = presetResolver;
			_prefabFactory = prefabFactory;
			_loadingScreenUIService = loadingScreenUIService;
		}

		protected override void OnDisposed()
		{
			ClearScreen();
		}

		public void ApplyPreset(LoadingScreenPreset preset)
		{
			_useUnscaledTime = preset.UseUnscaledTime;
			_showScreensContainer = preset.HasScreenContent;
			_isContentVisible = false;
			StopTips();
			ClearScreen();
			if (_showScreensContainer)
			{
				LoadScreen(preset.ScreenType);
			}
			SetContentVisible(visible: false);
		}

		public void SetContentVisible(bool visible)
		{
			bool flag = visible && _showScreensContainer;
			if (flag && !_isContentVisible)
			{
				_activeScreenContent?.OnShown();
				StartTips();
			}
			else if (!flag && _isContentVisible)
			{
				StopTips();
				_activeScreenContent?.OnHidden();
			}
			_isContentVisible = flag;
			base.View.SetRootActive(_showScreensContainer && (flag || _activeScreenObject != null));
		}

		public void StopTips()
		{
			_tipsPresenter?.Stop();
			_tipsPresenter = null;
		}

		private void LoadScreen(LoadingScreenScreenType screenType)
		{
			LoadingScreenScreenPreset screenPreset = _presetResolver.GetScreenPreset(screenType);
			if (screenPreset == null || screenPreset.Prefab == null)
			{
				Debug.LogError($"Loading screen content preset '{screenType}' is not configured or has no prefab.");
				return;
			}
			_activeScreenPreset = screenPreset;
			_activeScreenObject = _prefabFactory.Create(screenPreset.Prefab, base.View.ScreenContainer);
			base.View.SetActiveScreen(_activeScreenObject);
			_activeScreenContent = _activeScreenObject.GetComponent<LoadingScreenContentViewBase>();
			if (_activeScreenContent == null)
			{
				Debug.LogError(string.Format("Loading screen prefab for '{0}' has no {1} component.", screenType, "LoadingScreenContentViewBase"));
			}
			else
			{
				BindTipsPresenter(_activeScreenContent.TipsView);
			}
		}

		private void BindTipsPresenter(LoadingScreenTipsViewBase tipsView)
		{
			StopTips();
			if (!(tipsView == null))
			{
				_tipsPresenter = _loadingScreenUIService.BindPresenter<LoadingScreenTipsPresenter>(tipsView);
				_tipsPresenter.SetUseUnscaledTime(_useUnscaledTime);
				_tipsPresenter.SetAnimatedLoadingTip(_activeScreenPreset.IsAnimatedLoadingTip);
			}
		}

		private void StartTips()
		{
			if (_activeScreenPreset == null || !_activeScreenPreset.IsTipsEnabled || _tipsPresenter == null)
			{
				return;
			}
			LoadingScreenTipEntry[] entries = _activeScreenPreset.Entries;
			if (entries.Length == 0)
			{
				if (_activeScreenPreset.IsAnimatedLoadingTip)
				{
					_tipsPresenter.ShowPrefabTip();
				}
			}
			else if (_activeScreenPreset.IsTipsSequence)
			{
				_tipsPresenter.RunTipsSequence(entries);
			}
			else
			{
				_tipsPresenter.ShowRandomTip(entries);
			}
		}

		private void ClearScreen()
		{
			StopTips();
			if (_isContentVisible)
			{
				_activeScreenContent?.OnHidden();
			}
			_isContentVisible = false;
			_activeScreenPreset = null;
			base.View.DestroyActiveScreen();
			_activeScreenObject = null;
			_activeScreenContent = null;
		}
	}
}
