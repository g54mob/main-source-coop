using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ChineseDetectionModule.Scripts.Data;
using Features.GameModeModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerCustomization.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SessionManagementModule.Models;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Features.ViewSystemModule.Scripts.Windows;
using Fusion;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using NetworkServices.NetworkEvents;
using NetworkServices.RoomCodeGenerators;
using PlayerCustomization.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class MainMenuPresenter : PresenterBehaviour<MainMenuViewBase>, IBackButtonProcessor
	{
		private const int MAX_AUTO_RECOVERY_ATTEMPTS = 1;

		private readonly INavigationService _navigationService;

		private readonly IStartSessionService _startSessionService;

		private readonly ILocalizationService _localizationService;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private readonly IRoomCodeGenerator _roomCodeGenerator;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly ILoadingScreenService _loadingScreenService;

		private readonly JoinCrewPopupModel _joinCrewPopupModel;

		private readonly PlayerProfileModel _playerProfileModel;

		private readonly IPlayerCustomizationService _playerCustomizationService;

		private readonly MainMenuViewModel _mainMenuViewModel;

		private readonly ISessionReconnectService _sessionReconnectService;

		private readonly IWindowsService _windowsService;

		private readonly TutorialModel _tutorialModel;

		private readonly IMultiplayerService _multiplayerService;

		private readonly ISessionRunStarter _sessionRunStarter;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly GameModeModel _gameModeModel;

		private readonly SentAnalyticsModel _sentAnalyticsModel;

		private readonly IQuickJoinService _quickJoinService;

		private readonly MatchmakingPreviewPopupModel _matchmakingPreviewPopupModel;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly ISessionRecoverySource _sessionRecoverySource;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private readonly ChineseDetectionModel _chineseDetectionModel;

		private readonly ILanguageService _languageService;

		private CancellationTokenSource _searchCancellationSource;

		private string _playerName;

		private string _roomIdentifier;

		private bool _nameIsValid;

		private bool _codeIsValid;

		private ShutdownReason _lastFailedConnectionReason;

		private string _sessionName;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public MainMenuPresenter(IStartSessionService startSessionService, MultiplayerSessionConfig multiplayerSessionConfig, IRoomCodeGenerator roomCodeGenerator, MultiplayerModel multiplayerModel, NetworkRunnerEventBus eventBus, ILoadingScreenService loadingScreenService, ILocalizationService localizationService, JoinCrewPopupModel joinCrewPopupModel, PlayerProfileModel playerProfileModel, IPlayerCustomizationService playerCustomizationService, INavigationService navigationService, MainMenuViewModel mainMenuViewModel, ISessionReconnectService sessionReconnectService, IQuickJoinService quickJoinService, MatchmakingPreviewPopupModel matchmakingPreviewPopupModel, IUIBackButtonRegistrationService backButtonRegistrationService, ISessionRecoverySource sessionRecoverySource, IWindowsService windowsService, TutorialModel tutorialModel, IMultiplayerService multiplayerService, Features.LevelModule.Scripts.LevelModel levelModel, GameModeModel gameModeModel, SentAnalyticsModel sentAnalyticsModel, GameAnalyticsEventSendService gameAnalyticsEventSendService, ChineseDetectionModel chineseDetectionModel, ILanguageService languageService)
		{
			_startSessionService = startSessionService;
			_sessionRecoverySource = sessionRecoverySource;
			_multiplayerSessionConfig = multiplayerSessionConfig;
			_roomCodeGenerator = roomCodeGenerator;
			_multiplayerModel = multiplayerModel;
			_eventBus = eventBus;
			_loadingScreenService = loadingScreenService;
			_localizationService = localizationService;
			_joinCrewPopupModel = joinCrewPopupModel;
			_playerProfileModel = playerProfileModel;
			_playerCustomizationService = playerCustomizationService;
			_navigationService = navigationService;
			_mainMenuViewModel = mainMenuViewModel;
			_sessionReconnectService = sessionReconnectService;
			_windowsService = windowsService;
			_tutorialModel = tutorialModel;
			_multiplayerService = multiplayerService;
			_levelModel = levelModel;
			_gameModeModel = gameModeModel;
			_sentAnalyticsModel = sentAnalyticsModel;
			_quickJoinService = quickJoinService;
			_matchmakingPreviewPopupModel = matchmakingPreviewPopupModel;
			_backButtonRegistrationService = backButtonRegistrationService;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_chineseDetectionModel = chineseDetectionModel;
			_languageService = languageService;
		}

		public bool CanHandleBack()
		{
			return _multiplayerModel.IsSearchingInProgress;
		}

		public void OnBack()
		{
			if (_multiplayerModel.IsSearchingInProgress)
			{
				CancelSearchTeammates();
			}
		}

		protected override void OnViewSet()
		{
			Initialize();
			UpdateInteractables();
			Subscribe();
		}

		protected override void OnDisposed()
		{
			Unsubscribe();
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.ErrorPopupCross.onClick.AddListener(ClosePopup);
			FocusFirstSelection();
			base.View.OnFocused += OnViewFocused;
			base.View.ReconnectButton.onClick.AddListener(OnReconnectClicked);
			base.View.StartBaseTutorialFirstTimeButton.onClick.AddListener(OnStartBaseTutorialFirstTimeClicked);
			base.View.StartBaseTutorialRegularButton.onClick.AddListener(OnStartBaseTutorialRegularClicked);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.ErrorPopupCross.onClick.RemoveListener(ClosePopup);
			base.View.OnFocused -= OnViewFocused;
			base.View.ReconnectButton.onClick.RemoveListener(OnReconnectClicked);
			base.View.StartBaseTutorialFirstTimeButton.onClick.RemoveListener(OnStartBaseTutorialFirstTimeClicked);
			base.View.StartBaseTutorialRegularButton.onClick.RemoveListener(OnStartBaseTutorialRegularClicked);
		}

		private void Initialize()
		{
			_playerName = _playerProfileModel.PlayerName;
			if (string.IsNullOrEmpty(_playerName))
			{
				ValidatePlayerName(_playerCustomizationService.GetPlayerDefaultNickName(PlayerCustomizationType.Steam));
			}
			base.View.SetPlayerName(_playerName);
			_nameIsValid = true;
			_codeIsValid = false;
			_joinCrewPopupModel.SetRoomIdentifierDisplay(string.Empty);
			_joinCrewPopupModel.SetJoinViaCodeInteractable(interactable: false);
			ValidateAndShowAsync().Forget();
		}

		private void OnViewFocused()
		{
			FocusFirstSelection();
		}

		private void ApplyChinaServersNotice()
		{
			bool isChineseAudience = _chineseDetectionModel.IsChineseAudience;
			if (isChineseAudience)
			{
				base.View.SetChinaServersNoticeText(_localizationService.GetLocalizedString(base.View.ChinaServersNoticeLocalizationKey));
			}
			base.View.SetChinaServersNoticeVisible(isChineseAudience);
		}

		private void OnIsChineseAudienceChanged(bool isChineseAudience)
		{
			ApplyChinaServersNotice();
		}

		private void Subscribe()
		{
			MainMenuViewBase view = base.View;
			view.OnHostSessionButtonClick = (Action)Delegate.Combine(view.OnHostSessionButtonClick, new Action(OnHostSessionClicked));
			MainMenuViewBase view2 = base.View;
			view2.OnJoinSessionButtonClick = (Action)Delegate.Combine(view2.OnJoinSessionButtonClick, new Action(OpenJoinCrewPopup));
			MainMenuViewBase view3 = base.View;
			view3.OnSearchTeammatesButtonClick = (Action)Delegate.Combine(view3.OnSearchTeammatesButtonClick, new Action(StartSearchTeammates));
			MainMenuViewBase view4 = base.View;
			view4.OnCreditsButtonClick = (Action)Delegate.Combine(view4.OnCreditsButtonClick, new Action(OpenCredits));
			MainMenuViewBase view5 = base.View;
			view5.OnPlayerNameChanged = (Action<string>)Delegate.Combine(view5.OnPlayerNameChanged, new Action<string>(ValidatePlayerName));
			_joinCrewPopupModel.OnJoinViaCodeRequested += OnJoinViaCodeConfirmed;
			_joinCrewPopupModel.OnRoomIdentifierChanged += ValidateRoomIdentifier;
			_matchmakingPreviewPopupModel.OnJoinRequested += OnQuickJoinRequested;
			_matchmakingPreviewPopupModel.OnSearchAgainRequested += StartSearchTeammates;
			_eventBus.Subscribe<OnFailedStartGameEvent>(SetFailedConnectionReason);
			_eventBus.Subscribe<OnSessionInProgressBlockedEvent>(OnSessionInProgressBlocked);
			_chineseDetectionModel.OnIsChineseAudienceChanged += OnIsChineseAudienceChanged;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(ApplyChinaServersNotice));
			ApplyChinaServersNotice();
		}

		private void Unsubscribe()
		{
			MainMenuViewBase view = base.View;
			view.OnHostSessionButtonClick = (Action)Delegate.Remove(view.OnHostSessionButtonClick, new Action(OnHostSessionClicked));
			MainMenuViewBase view2 = base.View;
			view2.OnJoinSessionButtonClick = (Action)Delegate.Remove(view2.OnJoinSessionButtonClick, new Action(OpenJoinCrewPopup));
			MainMenuViewBase view3 = base.View;
			view3.OnSearchTeammatesButtonClick = (Action)Delegate.Remove(view3.OnSearchTeammatesButtonClick, new Action(StartSearchTeammates));
			MainMenuViewBase view4 = base.View;
			view4.OnCreditsButtonClick = (Action)Delegate.Remove(view4.OnCreditsButtonClick, new Action(OpenCredits));
			MainMenuViewBase view5 = base.View;
			view5.OnPlayerNameChanged = (Action<string>)Delegate.Remove(view5.OnPlayerNameChanged, new Action<string>(ValidatePlayerName));
			_joinCrewPopupModel.OnJoinViaCodeRequested -= OnJoinViaCodeConfirmed;
			_joinCrewPopupModel.OnRoomIdentifierChanged -= ValidateRoomIdentifier;
			_matchmakingPreviewPopupModel.OnJoinRequested -= OnQuickJoinRequested;
			_matchmakingPreviewPopupModel.OnSearchAgainRequested -= StartSearchTeammates;
			_eventBus.Unsubscribe<OnFailedStartGameEvent>(SetFailedConnectionReason);
			_eventBus.Unsubscribe<OnSessionInProgressBlockedEvent>(OnSessionInProgressBlocked);
			_chineseDetectionModel.OnIsChineseAudienceChanged -= OnIsChineseAudienceChanged;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(ApplyChinaServersNotice));
			_searchCancellationSource?.Cancel();
			_backButtonRegistrationService.Unregister(this);
			DisposeSearchCancellation();
		}

		private void OnHostSessionClicked()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.Host);
			StartCreateRoomOperationLoading();
		}

		private void OpenJoinCrewPopup()
		{
			if (!_multiplayerModel.IsOperationInProgress)
			{
				_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.JoinOpen);
				_joinCrewPopupModel.SetOpen(isOpen: true);
			}
		}

		private void OnJoinViaCodeConfirmed()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.JoinConfirm);
			StartJoinRoomOperationLoading();
		}

		private void OpenCredits()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.Credits);
			_windowsService.OpenWindow<CreditsWindow>();
		}

		private void ValidatePlayerName(string playerName)
		{
			playerName = ((playerName.Length <= _multiplayerSessionConfig.PlayerNameMaxLength) ? playerName : playerName.Substring(0, _multiplayerSessionConfig.PlayerNameMaxLength));
			playerName = new string(playerName.Where((char character) => HubMenuConstants.PossibleNameCharacters.Contains(character)).ToArray());
			_playerName = playerName;
			_playerProfileModel.SetPlayerName(_playerName);
			base.View.SetPlayerName(_playerName);
			_nameIsValid = !string.IsNullOrEmpty(_playerName);
			UpdateInteractables();
		}

		private void ValidateRoomIdentifier(string roomIdentifier)
		{
			roomIdentifier = ((roomIdentifier.Length <= _multiplayerSessionConfig.RoomCodeLength) ? roomIdentifier : roomIdentifier.Substring(0, _multiplayerSessionConfig.RoomCodeLength));
			roomIdentifier = roomIdentifier.ToUpperInvariant();
			roomIdentifier = new string(roomIdentifier.Where((char character) => HubMenuConstants.PossibleCodeCharacters.Contains(character)).ToArray());
			_roomIdentifier = roomIdentifier;
			_joinCrewPopupModel.SetRoomIdentifierDisplay(_roomIdentifier);
			_codeIsValid = _roomIdentifier.Length == _multiplayerSessionConfig.RoomCodeLength;
			_joinCrewPopupModel.SetJoinViaCodeInteractable(!_multiplayerModel.IsOperationInProgress && _codeIsValid && _nameIsValid);
		}

		private void UpdateInteractables()
		{
			bool flag = _multiplayerModel.IsOperationInProgress || _multiplayerModel.IsSearchingInProgress;
			base.View.SetJoinInteractable(!flag && _nameIsValid);
			base.View.SetHostInteractable(!flag && _nameIsValid);
			base.View.SetSearchTeammatesInteractable(!flag && _nameIsValid);
			base.View.SetNicknameInteractable(!flag);
			_joinCrewPopupModel.SetJoinViaCodeInteractable(!flag && _codeIsValid && _nameIsValid);
		}

		private void StartCreateRoomOperationLoading()
		{
			if (!_multiplayerModel.IsOperationInProgress)
			{
				_multiplayerModel.IsOperationInProgress = true;
				UpdateInteractables();
				base.View.SetActionText(_localizationService.GetLocalizedString(base.View.CreateRoomLocalizationKey));
				_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade, CreateRoom);
			}
		}

		private void StartJoinRoomOperationLoading()
		{
			if (!_multiplayerModel.IsOperationInProgress && _codeIsValid)
			{
				_joinCrewPopupModel.SetOpen(isOpen: false);
				_multiplayerModel.IsOperationInProgress = true;
				UpdateInteractables();
				base.View.SetActionText(_localizationService.GetLocalizedString(base.View.JoinRoomLocalizationKey));
				_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade, JoinRoom);
			}
		}

		private void StartSearchTeammates()
		{
			if (!_multiplayerModel.IsOperationInProgress && !_multiplayerModel.IsSearchingInProgress && _nameIsValid)
			{
				_gameAnalyticsEventSendService.TrackMatchmakingSearchStarted();
				_matchmakingPreviewPopupModel.SetOpen(isOpen: false);
				_multiplayerModel.IsSearchingInProgress = true;
				UpdateInteractables();
				base.View.SetActionText(MatchmakingPlaceholders.OrFallback(_localizationService.GetLocalizedString(base.View.SearchTeammatesLocalizationKey), "Searching for teammates…"));
				_searchCancellationSource = new CancellationTokenSource();
				_backButtonRegistrationService.Register(this);
				SearchTeammatesAsync(_searchCancellationSource.Token).Forget();
			}
		}

		private async UniTaskVoid SearchTeammatesAsync(CancellationToken cancellationToken)
		{
			try
			{
				QuickJoinResult result = await _quickJoinService.FindOpenSessionAsync(cancellationToken);
				if (!cancellationToken.IsCancellationRequested)
				{
					EndSearching();
					if (result.Found)
					{
						_gameAnalyticsEventSendService.TrackMatchmakingSearchFound();
					}
					else
					{
						_gameAnalyticsEventSendService.TrackMatchmakingSearchNotFound();
					}
					_matchmakingPreviewPopupModel.SetResult(result);
					_matchmakingPreviewPopupModel.SetOpen(isOpen: true);
				}
			}
			catch (Exception exception)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					EndSearching();
					Debug.LogException(exception);
				}
			}
		}

		private void CancelSearchTeammates()
		{
			if (_multiplayerModel.IsSearchingInProgress)
			{
				_searchCancellationSource?.Cancel();
				EndSearching();
			}
		}

		private void EndSearching()
		{
			_multiplayerModel.IsSearchingInProgress = false;
			UpdateInteractables();
			base.View.SetActionText(string.Empty);
			_backButtonRegistrationService.Unregister(this);
			DisposeSearchCancellation();
		}

		private void DisposeSearchCancellation()
		{
			_searchCancellationSource?.Dispose();
			_searchCancellationSource = null;
		}

		private void OnQuickJoinRequested()
		{
			if (_matchmakingPreviewPopupModel.Result.Found && !_multiplayerModel.IsOperationInProgress)
			{
				_gameAnalyticsEventSendService.TrackMatchmakingPreviewJoin();
				_matchmakingPreviewPopupModel.SetOpen(isOpen: false);
				_multiplayerModel.IsOperationInProgress = true;
				UpdateInteractables();
				base.View.SetActionText(_localizationService.GetLocalizedString(base.View.JoinRoomLocalizationKey));
				_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade, QuickJoinRoom);
			}
		}

		private async void QuickJoinRoom()
		{
			try
			{
				JoinRoomResult joinRoomResult = await _quickJoinService.JoinSessionAsync(_matchmakingPreviewPopupModel.Result.SessionName, _matchmakingPreviewPopupModel.Result.Region);
				_multiplayerModel.IsOperationInProgress = false;
				if (joinRoomResult.IsSuccess)
				{
					_gameAnalyticsEventSendService.TrackMatchmakingJoinSuccess();
				}
				else
				{
					_gameAnalyticsEventSendService.TrackMatchmakingJoinFailed(ToMatchmakingJoinFailureReason(joinRoomResult.ShutdownReason, joinRoomResult.FailureReason));
				}
				string errorMessage = ((joinRoomResult.FailureReason == JoinFailureReason.SessionInProgress) ? _localizationService.GetLocalizedString(LocalizationKey.Ui_SessionInProgress_Error) : joinRoomResult.ErrorMessage);
				ProcessStartSessionResult(joinRoomResult.IsSuccess, joinRoomResult.ShutdownReason.ToString(), errorMessage);
			}
			catch (Exception exception)
			{
				_multiplayerModel.IsOperationInProgress = false;
				_gameAnalyticsEventSendService.TrackMatchmakingJoinFailed("other");
				Debug.LogException(exception);
			}
		}

		private async void CreateRoom()
		{
			try
			{
				_roomIdentifier = GenerateRandomRoomIdentifier();
				CreateRoomResult createRoomResult = await _startSessionService.StartCreateRoomTask(new SessionCreateData
				{
					RoomCode = _roomIdentifier,
					HostName = _playerProfileModel.PlayerName,
					JoinSource = JoinSource.Host
				});
				_multiplayerModel.IsOperationInProgress = false;
				ProcessStartSessionResult(createRoomResult.IsSuccess, createRoomResult.ShutdownReason.ToString(), createRoomResult.ErrorMessage);
			}
			catch (Exception exception)
			{
				_multiplayerModel.IsOperationInProgress = false;
				Debug.LogException(exception);
			}
		}

		private async void JoinRoom()
		{
			_ = 1;
			try
			{
				if (await _sessionReconnectService.IsJoinBlockedAsync(_roomIdentifier, SessionInProgressJoinPolicy.SettleThenBlock))
				{
					_multiplayerModel.IsOperationInProgress = false;
					ProcessStartSessionResult(isSuccess: false, ShutdownReason.Ok.ToString(), _localizationService.GetLocalizedString(LocalizationKey.Ui_SessionInProgress_Error));
					return;
				}
				JoinRoomResult joinRoomResult = await _startSessionService.StartJoinRoomTask(new SessionCreateData
				{
					RoomCode = _roomIdentifier,
					JoinSource = JoinSource.RoomCode
				});
				_multiplayerModel.IsOperationInProgress = false;
				string errorMessage = ((joinRoomResult.FailureReason == JoinFailureReason.SessionInProgress) ? _localizationService.GetLocalizedString(LocalizationKey.Ui_SessionInProgress_Error) : joinRoomResult.ErrorMessage);
				ProcessStartSessionResult(joinRoomResult.IsSuccess, joinRoomResult.ShutdownReason.ToString(), errorMessage);
			}
			catch (Exception exception)
			{
				_multiplayerModel.IsOperationInProgress = false;
				Debug.LogException(exception);
			}
		}

		private static string ToMatchmakingJoinFailureReason(ShutdownReason shutdownReason, JoinFailureReason failureReason)
		{
			if (failureReason == JoinFailureReason.SessionInProgress)
			{
				return "other";
			}
			string text = shutdownReason.ToString();
			if (text.IndexOf("Full", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return "room_full";
			}
			if (text.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Disconnect", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("PhotonCloud", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Connection", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return "network";
			}
			return "other";
		}

		private void ProcessStartSessionResult(bool isSuccess, string shutdownReason, string errorMessage)
		{
			if (!isSuccess)
			{
				base.View.SetActionText(string.Empty);
				base.View.SetPopupActive(isActive: true);
				base.View.SetShutdownReason(shutdownReason);
				base.View.SetErrorMessage(errorMessage);
				_loadingScreenService.Hide(LoadingScreenShowType.ShowScreenWithFade, ShowPopup);
				if (base.View.FirstErrorButtonToSelect != null)
				{
					_navigationService.SetNavigationToObject(base.View.FirstErrorButtonToSelect);
				}
			}
		}

		private void ShowPopup()
		{
			UpdateInteractables();
		}

		private string GenerateRandomRoomIdentifier()
		{
			return _roomCodeGenerator.Generate(_multiplayerSessionConfig.RoomCodeLength);
		}

		private void SetFailedConnectionReason(OnFailedStartGameEvent failedStartGameEvent)
		{
			_lastFailedConnectionReason = failedStartGameEvent.ShutdownReason;
			base.View.SetActionText(_lastFailedConnectionReason.ToString());
		}

		private void OnSessionInProgressBlocked(OnSessionInProgressBlockedEvent blockedEvent)
		{
			_multiplayerModel.IsOperationInProgress = false;
			ProcessStartSessionResult(isSuccess: false, ShutdownReason.Ok.ToString(), _localizationService.GetLocalizedString(LocalizationKey.Ui_SessionInProgress_Error));
		}

		private void ClosePopup()
		{
			base.View.SetPopupActive(isActive: false);
			FocusFirstSelection();
		}

		private void FocusFirstSelection()
		{
			if (!_mainMenuViewModel.IsAnyPopupOpen)
			{
				Selectable firstButtonToSelect = base.View.GetFirstButtonToSelect();
				if (firstButtonToSelect != null)
				{
					_navigationService.SetNavigationToObject(firstButtonToSelect);
				}
			}
		}

		private async UniTaskVoid ValidateAndShowAsync()
		{
			SwitchBaseTutorialButtons();
			HideReconnectButton();
			if (!TryConsumeAutoRecovery() && _sessionRecoverySource.TryGetReconnectSession(out var sessionName))
			{
				if (!(await _sessionReconnectService.IsSessionValidAsync(sessionName)))
				{
					HideReconnectButton();
				}
				else
				{
					InitializeReconnection(sessionName);
				}
			}
		}

		private bool TryConsumeAutoRecovery()
		{
			if (!PlayerSessionPrefs.TryGetPendingRecovery(out var reason, out var attemptCount))
			{
				return false;
			}
			if (!PlayerSessionPrefs.TryGetSavedSessionName(out var sessionName))
			{
				Debug.LogError($"[SessionRecovery] Pending recovery (reason {reason}) but no saved session to reconnect to — giving up.");
				PlayerSessionPrefs.SetRecoveryResult(SessionRecoveryResult.GaveUp);
				return false;
			}
			if (attemptCount > 1)
			{
				Debug.LogError($"[SessionRecovery] Auto-reconnect budget spent (attempt {attemptCount}, reason {reason}) — giving up, showing the manual reconnect button.");
				PlayerSessionPrefs.SetRecoveryResult(SessionRecoveryResult.GaveUp);
				return false;
			}
			Debug.LogError($"[SessionRecovery] Pending recovery (attempt {attemptCount}, reason {reason}) — auto-reconnecting to '{sessionName}'.");
			PlayerSessionPrefs.SetRecoveryResult(SessionRecoveryResult.Attempting);
			_sessionName = sessionName;
			AttemptReconnect().Forget();
			return true;
		}

		private void InitializeReconnection(string sessionName)
		{
			_sessionName = sessionName;
			base.View.ReconnectButton.gameObject.SetActive(value: true);
			FocusFirstSelection();
		}

		private void OnReconnectClicked()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.Reconnect);
			HideReconnectButton();
			AttemptReconnect().Forget();
		}

		private void OnStartBaseTutorialFirstTimeClicked()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.TutorialFirstTime);
			StartBaseTutorial();
		}

		private void OnStartBaseTutorialRegularClicked()
		{
			_gameAnalyticsEventSendService.TrackMainMenuClick(MainMenuClickAnalyticsButton.TutorialRegular);
			StartBaseTutorial();
		}

		private void StartBaseTutorial()
		{
			_levelModel.SetSelectedLevel(LevelType.TutorialLoopScene);
			_levelModel.SetSelectedSequenceSet(LevelSequenceSet.BaseTutorial);
			_gameModeModel.CurrentGameMode = GameModeType.DisabledEnemies;
			_tutorialModel.IsBaseTutorialSequenceInvoked = true;
			StartCreateRoomOperationLoading();
		}

		private void HideReconnectButton()
		{
			if (!(base.View == null) && !(base.View.ReconnectButton == null))
			{
				base.View.ReconnectButton.gameObject.SetActive(value: false);
			}
		}

		private async UniTaskVoid AttemptReconnect()
		{
			await _sessionReconnectService.TryReconnectAsync(_sessionName);
		}

		private void SwitchBaseTutorialButtons()
		{
			bool flag = _sentAnalyticsModel.SessionStartedCount == 0;
			base.View.StartBaseTutorialFirstTimeButton.gameObject.SetActive(flag);
			base.View.StartBaseTutorialRegularButton.gameObject.SetActive(!flag);
		}
	}
}
