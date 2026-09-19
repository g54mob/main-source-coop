using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.ProgressSavingModule.Scripts.PlayerInfo;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class ThankPlayingPopupPresenter : PresenterBehaviour<ThankPlayingPopupViewBase>, IBackButtonProcessor
	{
		private const string RELEASE_ANNOUNCE_POPUP_NAME = "ReleaseAnnouncement";

		private readonly INavigationService _navigationService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly PlayerInfoModel _playerInfoModel;

		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly ISavingService _savingService;

		private readonly MainMenuPopupsConfiguration _mainMenuPopupsConfiguration;

		private readonly MainMenuViewModel _mainMenuViewModel;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public ThankPlayingPopupPresenter(INavigationService navigationService, IUIBackButtonRegistrationService backButtonRegistrationService, PlayerInfoModel playerInfoModel, SentAnalyticsModel sentAnalyticsModel, ISavingService savingService, MainMenuPopupsConfiguration mainMenuPopupsConfiguration, MainMenuViewModel mainMenuViewModel, GameAnalyticsEventSendService analyticsEventSendService)
		{
			_navigationService = navigationService;
			_backButtonRegistrationService = backButtonRegistrationService;
			_playerInfoModel = playerInfoModel;
			_sentAnalyticsModel = sentAnalyticsModel;
			_savingService = savingService;
			_mainMenuPopupsConfiguration = mainMenuPopupsConfiguration;
			_mainMenuViewModel = mainMenuViewModel;
			_gameAnalyticsEventSendService = analyticsEventSendService;
		}

		public bool CanHandleBack()
		{
			return IsPopupVisible();
		}

		public void OnBack()
		{
			TryClosePopup();
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			if (ShouldShowPopup())
			{
				MarkPopupAsShown();
				ShowPopup();
			}
			else
			{
				TryClosePopup();
			}
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnCloseClicked += HandleCloseClicked;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnCloseClicked -= HandleCloseClicked;
			_backButtonRegistrationService.Unregister(this);
		}

		private void MarkPopupAsShown()
		{
			_playerInfoModel.MarkReleaseAnnouncePopupShown();
			_savingService.SaveDataForGroup(SavingGroup.PlayerInfo);
		}

		private void HandleCloseClicked()
		{
			ClosePopup();
		}

		private bool IsPopupVisible()
		{
			return base.View.ContentContainer.activeSelf;
		}

		private void SelectFirstNavigationButton()
		{
			if (base.View.FirstSelectable != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstSelectable);
			}
		}

		private void SelectExitNavigationButton()
		{
			if (base.View.ExitSelectable != null)
			{
				_navigationService.SetNavigationToObject(base.View.ExitSelectable);
			}
		}

		private void TryClosePopup()
		{
			if (IsPopupVisible())
			{
				ClosePopup();
			}
		}

		private void ClosePopup()
		{
			_mainMenuViewModel.SetMainMenuPopupOpenStatus(anyPopupOpen: false);
			base.View.ContentContainer.SetActive(value: false);
			_backButtonRegistrationService.Unregister(this);
			SelectExitNavigationButton();
		}

		private void ShowPopup()
		{
			_gameAnalyticsEventSendService.TrackPopupShown("ReleaseAnnouncement");
			_mainMenuViewModel.SetMainMenuPopupOpenStatus(anyPopupOpen: true);
			base.View.ContentContainer.SetActive(value: true);
			_backButtonRegistrationService.Register(this);
			SelectFirstNavigationButton();
		}

		private bool ShouldShowPopup()
		{
			if (_mainMenuPopupsConfiguration.ReleaseAnnouncePopupEnabled && _sentAnalyticsModel.SessionStartedCount >= 1)
			{
				return !_playerInfoModel.ReleaseAnnouncePopupShown;
			}
			return false;
		}
	}
}
