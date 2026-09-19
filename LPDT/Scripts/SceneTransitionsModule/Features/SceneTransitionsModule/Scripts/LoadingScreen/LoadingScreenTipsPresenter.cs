using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenTipsPresenter : PresenterBehaviour<LoadingScreenTipsViewBase>
	{
		private readonly ILocalizationService _localizationService;

		private readonly ILanguageService _languageService;

		private CancellationTokenSource _tipsSequenceCts;

		private CancellationTokenSource _animationCts;

		private LoadingScreenTipEntry _currentEntry;

		private bool _useUnscaledTime = true;

		private bool _isAnimatedLoadingTip;

		public LoadingScreenTipsPresenter(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(OnLanguageChanged));
		}

		protected override void OnDisposed()
		{
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(OnLanguageChanged));
			Stop();
		}

		public void SetUseUnscaledTime(bool useUnscaledTime)
		{
			_useUnscaledTime = useUnscaledTime;
		}

		public void SetAnimatedLoadingTip(bool isAnimatedLoadingTip)
		{
			_isAnimatedLoadingTip = isAnimatedLoadingTip;
		}

		public void ShowPrefabTip()
		{
			ShowTip(null);
		}

		public void Stop()
		{
			_tipsSequenceCts?.Cancel();
			_tipsSequenceCts?.Dispose();
			_tipsSequenceCts = null;
			StopAnimation();
			_currentEntry = null;
			base.View.Reset();
		}

		public void ShowRandomTip(LoadingScreenTipEntry[] entries)
		{
			if (entries != null && entries.Length != 0)
			{
				LoadingScreenTipEntry entry = entries[UnityEngine.Random.Range(0, entries.Length)];
				ShowTip(entry);
			}
		}

		public void RunTipsSequence(LoadingScreenTipEntry[] entries)
		{
			if (entries != null && entries.Length != 0)
			{
				RunTipsSequenceAsync(entries).Forget();
			}
		}

		private void OnLanguageChanged()
		{
			if (_currentEntry != null)
			{
				ShowTip(_currentEntry);
			}
		}

		private void ShowTip(LoadingScreenTipEntry entry)
		{
			_currentEntry = entry;
			StopAnimation();
			string text = ResolveTipText(entry);
			if (!string.IsNullOrEmpty(text))
			{
				base.View.SetVisible(visible: true);
				if (_isAnimatedLoadingTip)
				{
					RunAnimatedTipAsync(text).Forget();
				}
				else
				{
					base.View.SetText(text);
				}
			}
		}

		private string ResolveTipText(LoadingScreenTipEntry entry)
		{
			if (entry != null && entry.LocalizationKey != LocalizationKey.NullLocalization)
			{
				string localizedString = _localizationService.GetLocalizedString(entry.LocalizationKey);
				if (!string.IsNullOrEmpty(localizedString))
				{
					return localizedString;
				}
			}
			return base.View.DefaultText;
		}

		private void StopAnimation()
		{
			_animationCts?.Cancel();
			_animationCts?.Dispose();
			_animationCts = null;
		}

		private async UniTaskVoid RunAnimatedTipAsync(string baseText)
		{
			_animationCts = new CancellationTokenSource();
			CancellationToken token = _animationCts.Token;
			float interval = base.View.AnimatedTipDotsInterval;
			int maxDots = base.View.AnimatedTipMaxDots;
			int dotCount = 0;
			try
			{
				while (!token.IsCancellationRequested)
				{
					base.View.SetText(baseText + new string('.', dotCount));
					dotCount = ((dotCount < maxDots) ? (dotCount + 1) : 0);
					await UniTask.Delay(TimeSpan.FromSeconds(interval), _useUnscaledTime, PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async UniTaskVoid RunTipsSequenceAsync(LoadingScreenTipEntry[] entries)
		{
			Stop();
			_tipsSequenceCts = new CancellationTokenSource();
			CancellationToken token = _tipsSequenceCts.Token;
			int index = 0;
			try
			{
				while (!token.IsCancellationRequested)
				{
					ShowTip(entries[index]);
					float tipsShowingTime = entries[index].TipsShowingTime;
					if (tipsShowingTime > 0f)
					{
						await UniTask.Delay(TimeSpan.FromSeconds(tipsShowingTime), _useUnscaledTime, PlayerLoopTiming.Update, token);
					}
					index = (index + 1) % entries.Length;
				}
			}
			catch (OperationCanceledException)
			{
			}
		}
	}
}
