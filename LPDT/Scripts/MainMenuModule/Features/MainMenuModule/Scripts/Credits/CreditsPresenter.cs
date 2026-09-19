using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ChineseDetectionModule.Scripts.Core;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Features.ViewSystemModule.Scripts.Windows;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.MainMenuModule.Scripts.Credits
{
	[PublicAPI]
	public class CreditsPresenter : PresenterBehaviour<CreditsViewBase>, IBackButtonProcessor
	{
		private readonly CreditsConfiguration _creditsConfiguration;

		private readonly CreditsWindow _creditsWindow;

		private readonly IWindowsService _windowsService;

		private readonly INavigationService _navigationService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly DiContainer _container;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly IChineseDetectionService _chineseDetectionService;

		private readonly List<CreditsSectionPresenter> _sectionPresenters = new List<CreditsSectionPresenter>();

		private CancellationTokenSource _autoScrollCts;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public CreditsPresenter(CreditsConfiguration creditsConfiguration, CreditsWindow creditsWindow, IWindowsService windowsService, INavigationService navigationService, IUIBackButtonRegistrationService backButtonRegistrationService, DiContainer container, GameAnalyticsEventSendService gameAnalyticsEventSendService, IChineseDetectionService chineseDetectionService)
		{
			_creditsConfiguration = creditsConfiguration;
			_creditsWindow = creditsWindow;
			_windowsService = windowsService;
			_navigationService = navigationService;
			_backButtonRegistrationService = backButtonRegistrationService;
			_container = container;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_chineseDetectionService = chineseDetectionService;
		}

		public bool CanHandleBack()
		{
			return _creditsWindow.WindowStatus == WindowStatus.Showed;
		}

		public void OnBack()
		{
			CloseWindow();
		}

		protected override void OnViewSet()
		{
			BuildSections();
			_backButtonRegistrationService.Register(this);
			if (base.View.FirstSelectable != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstSelectable);
			}
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.BackButton.onClick.AddListener(CloseWindow);
			ShowBanner();
			HideWholeCanvas();
			StartAutoScroll();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.BackButton.onClick.RemoveListener(CloseWindow);
			StopAutoScroll();
			ShowBanner();
			if (base.View.WholeCanvasGroup != null)
			{
				base.View.WholeCanvasGroup.alpha = 1f;
			}
		}

		protected override void OnDisposed()
		{
			StopAutoScroll();
			_backButtonRegistrationService.Unregister(this);
			ClearSections();
		}

		private void BuildSections()
		{
			if (_creditsConfiguration.Sections == null)
			{
				return;
			}
			foreach (CreditsSection section in _creditsConfiguration.Sections)
			{
				CreditsSectionPresenter creditsSectionPresenter = CreateSection();
				creditsSectionPresenter.Setup(section);
				_sectionPresenters.Add(creditsSectionPresenter);
			}
		}

		private CreditsSectionPresenter CreateSection()
		{
			CreditsSectionViewBase component = _container.InstantiatePrefab(base.View.SectionPrefab.gameObject).GetComponent<CreditsSectionViewBase>();
			_creditsWindow.AddView(component.transform, worldPositionStays: false);
			CreditsSectionPresenter presenterForView = _creditsWindow.GetPresenterForView<CreditsSectionPresenter>(component);
			presenterForView.SetParent(base.View.SectionsContainer);
			return presenterForView;
		}

		private void ClearSections()
		{
			foreach (CreditsSectionPresenter sectionPresenter in _sectionPresenters)
			{
				sectionPresenter.DestroyView();
			}
			_sectionPresenters.Clear();
		}

		private void StartAutoScroll()
		{
			if (!(base.View.ScrollRect == null))
			{
				StopAutoScroll();
				_autoScrollCts = new CancellationTokenSource();
				AutoScroll(_autoScrollCts.Token).Forget();
			}
		}

		private void StopAutoScroll()
		{
			if (_autoScrollCts != null)
			{
				_autoScrollCts.Cancel();
				_autoScrollCts.Dispose();
				_autoScrollCts = null;
			}
		}

		private async UniTaskVoid AutoScroll(CancellationToken token)
		{
			ScrollRect scrollRect = base.View.ScrollRect;
			RectTransform content = scrollRect.content;
			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, token);
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
			scrollRect.velocity = Vector2.zero;
			float startY = 0f - scrollRect.viewport.rect.height;
			float endY = content.rect.height;
			float y = startY;
			SetContentY(content, y);
			await FadeCanvasGroup(base.View.WholeCanvasGroup, 0f, 1f, base.View.WindowFadeInDuration, token);
			await PlayBannerIntro(token);
			while (!token.IsCancellationRequested)
			{
				y += base.View.AutoScrollSpeed * Time.unscaledDeltaTime;
				if (y >= endY)
				{
					y = startY;
				}
				SetContentY(content, y);
				await UniTask.Yield(PlayerLoopTiming.Update, token);
			}
		}

		private async UniTask PlayBannerIntro(CancellationToken token)
		{
			CanvasGroup bannerCanvasGroup = base.View.BannerCanvasGroup;
			if (!(bannerCanvasGroup == null))
			{
				if (_chineseDetectionService.IsChineseAudience())
				{
					bannerCanvasGroup.alpha = 0f;
					return;
				}
				bannerCanvasGroup.alpha = 1f;
				await UniTask.Delay((int)(base.View.BannerHoldDuration * 1000f), DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, token);
				await FadeCanvasGroup(bannerCanvasGroup, 1f, 0f, base.View.BannerFadeDuration, token);
			}
		}

		private async UniTask FadeCanvasGroup(CanvasGroup canvasGroup, float from, float to, float duration, CancellationToken token)
		{
			if (!(canvasGroup == null))
			{
				canvasGroup.alpha = from;
				float elapsed = 0f;
				while (elapsed < duration && !token.IsCancellationRequested)
				{
					elapsed += Time.unscaledDeltaTime;
					canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
				canvasGroup.alpha = to;
			}
		}

		private void ShowBanner()
		{
			if (!(base.View.BannerCanvasGroup == null))
			{
				base.View.BannerCanvasGroup.alpha = (_chineseDetectionService.IsChineseAudience() ? 0f : 1f);
			}
		}

		private void HideWholeCanvas()
		{
			if (base.View.WholeCanvasGroup != null)
			{
				base.View.WholeCanvasGroup.alpha = 0f;
			}
		}

		private static void SetContentY(RectTransform content, float y)
		{
			Vector2 anchoredPosition = content.anchoredPosition;
			anchoredPosition.y = y;
			content.anchoredPosition = anchoredPosition;
		}

		private void CloseWindow()
		{
			_gameAnalyticsEventSendService.TrackMainMenuOpened(MainMenuOpenedAnalyticsSource.Credits);
			_windowsService.CloseWindow<CreditsWindow>();
		}
	}
}
