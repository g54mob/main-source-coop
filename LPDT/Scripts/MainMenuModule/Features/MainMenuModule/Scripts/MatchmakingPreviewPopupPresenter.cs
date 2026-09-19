using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.UIAnimationsModule.Scripts;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class MatchmakingPreviewPopupPresenter : PresenterBehaviour<MatchmakingPreviewPopupViewBase>, IBackButtonProcessor
	{
		private readonly MatchmakingPreviewPopupModel _model;

		private readonly ILocalizationService _localizationService;

		private readonly INavigationService _navigationService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly IUIAnimationService _uiAnimationService;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public MatchmakingPreviewPopupPresenter(MatchmakingPreviewPopupModel model, ILocalizationService localizationService, INavigationService navigationService, IUIBackButtonRegistrationService backButtonRegistrationService, IUIAnimationService uiAnimationService, GameAnalyticsEventSendService gameAnalyticsEventSendService)
		{
			_model = model;
			_localizationService = localizationService;
			_navigationService = navigationService;
			_backButtonRegistrationService = backButtonRegistrationService;
			_uiAnimationService = uiAnimationService;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
		}

		public bool CanHandleBack()
		{
			return _model.IsOpen;
		}

		public void OnBack()
		{
			if (_model.IsOpen)
			{
				ClosePopup();
			}
		}

		protected override void OnViewSet()
		{
			base.View.SetVisible(_model.IsOpen);
			RefreshResult(_model.Result);
			base.View.OnJoinClicked += OnJoinClicked;
			base.View.OnSearchAgainClicked += OnSearchAgainClicked;
			base.View.OnCloseClicked += ClosePopup;
			_model.OnOpenChanged += OnOpenChanged;
			_model.OnResultChanged += RefreshResult;
			if (_model.IsOpen)
			{
				_backButtonRegistrationService.Register(this);
			}
		}

		protected override void OnDisposed()
		{
			base.View.OnJoinClicked -= OnJoinClicked;
			base.View.OnSearchAgainClicked -= OnSearchAgainClicked;
			base.View.OnCloseClicked -= ClosePopup;
			_model.OnOpenChanged -= OnOpenChanged;
			_model.OnResultChanged -= RefreshResult;
			_backButtonRegistrationService.Unregister(this);
		}

		private void RefreshResult(QuickJoinResult result)
		{
			if (result.Found)
			{
				base.View.ShowSessionInfo(_localizationService.GetLocalizedString(base.View.GameFoundLocalizationKey), result.HostName, $"{result.PlayerCount}/{result.MaxPlayers}", result.Region, FormatPing(result.RegionPing));
			}
			else
			{
				base.View.ShowNoGamesFound(MatchmakingPlaceholders.OrFallback(_localizationService.GetLocalizedString(base.View.NoGamesFoundLocalizationKey), "No open games found"), _localizationService.GetLocalizedString(base.View.NoGamesFoundDescriptionLocalizationKey));
			}
		}

		private static string FormatPing(int regionPing)
		{
			if (regionPing < 0)
			{
				return string.Empty;
			}
			return $"{regionPing} ms";
		}

		private void OnOpenChanged(bool isOpen)
		{
			base.View.SetVisible(isOpen);
			if (isOpen)
			{
				_backButtonRegistrationService.Register(this);
				if (base.View.AnimationTarget != null)
				{
					_uiAnimationService.Play(base.View.AnimationTarget, UIAnimationKey.ScaleUp);
				}
				if (_model.Result.Found)
				{
					if (base.View.FirstButtonToSelectFound != null)
					{
						_navigationService.SetNavigationToObject(base.View.FirstButtonToSelectFound);
					}
				}
				else if (base.View.FirstButtonToSelectNotFound != null)
				{
					_navigationService.SetNavigationToObject(base.View.FirstButtonToSelectNotFound);
				}
			}
			else
			{
				_backButtonRegistrationService.Unregister(this);
				if (base.View.ButtonToSelectOnExit != null)
				{
					_navigationService.SetNavigationToObject(base.View.ButtonToSelectOnExit);
				}
			}
		}

		private void OnJoinClicked()
		{
			_model.RequestJoin();
		}

		private void OnSearchAgainClicked()
		{
			if (_model.Result.Found)
			{
				_gameAnalyticsEventSendService.TrackMatchmakingPreviewSkip();
			}
			_model.RequestSearchAgain();
		}

		private void ClosePopup()
		{
			_gameAnalyticsEventSendService.TrackMatchmakingPreviewClosed();
			_model.SetOpen(isOpen: false);
		}
	}
}
