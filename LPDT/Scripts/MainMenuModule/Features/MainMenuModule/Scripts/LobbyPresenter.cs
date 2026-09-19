using System;
using System.Linq;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Features.ConfirmExitPopupService.Scripts;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.GameUpdaterModule;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerCustomization.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SessionManagementModule.Models;
using Features.SettingsMenuModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Features.SteamInviteModule.Scripts;
using Fusion;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using Global.StateMachinesModule.Scripts;
using JetBrains.Annotations;
using NetworkServices.NetworkEvents;
using PlayerCustomization;
using PlayerCustomization.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	[PublicAPI]
	public class LobbyPresenter : PresenterBehaviour<LobbyViewBase>
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly DisconnectRequestEventClass _disconnectRequestEventClass;

		private readonly IGameUpdater _gameUpdater;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly ILoadingScreenService _loadingScreenService;

		private readonly StartGameLoadingNetworkEvent _startGameLoadingNetworkEvent;

		private readonly SettingsWindow _settingsWindow;

		private readonly WantToLeaveLobbyPopupModel _wantToLeaveLobbyPopupModel;

		private readonly ILanguageService _languageService;

		private readonly ILocalizationService _localizationService;

		private readonly ISteamInviteService _steamInviteService;

		private readonly SteamModel _steamModel;

		private readonly IConfirmExitService _confirmExitService;

		private readonly PlayerProfileModel _playerProfileModel;

		private readonly ISavingService _savingService;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly INavigationService _navigationService;

		private readonly MultiplayerSessionConfig _multiplayerSessionConfig;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly ChapterProgressSyncModel _chapterProgressSyncModel;

		private readonly ISessionRunStarter _sessionRunStarter;

		private readonly IMultiplayerService _multiplayerService;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private float _saveColorCooldown;

		private Color _lastSavedColor;

		private float _saveNicknameCooldown;

		private string _lastSavedNickname;

		private string _currentNickname;

		private float _ignoreExitLobbyUntilUnscaledTime;

		private bool _isExitLobbyButtonInCooldown;

		private string _waitingTextBase;

		private float _waitingDotsTimer;

		private int _waitingDotsIndex;

		private bool _isSharedModeMasterClient;

		private bool _isListeningForStartGameLoading;

		public LobbyPresenter(MultiplayerModel multiplayerModel, GameFlowStateMachine gameFlowStateMachine, DisconnectRequestEventClass disconnectRequestEventClass, IGameUpdater gameUpdater, PlayerCustomizationModel playerCustomizationModel, ILoadingScreenService loadingScreenService, StartGameLoadingNetworkEvent startGameLoadingNetworkEvent, SettingsWindow settingsWindow, WantToLeaveLobbyPopupModel wantToLeaveLobbyPopupModel, ILanguageService languageService, ILocalizationService localizationService, ISteamInviteService steamInviteService, SteamModel steamModel, IConfirmExitService confirmExitService, PlayerProfileModel playerProfileModel, ISavingService savingService, NetworkRunnerEventBus eventBus, INavigationService navigationService, MultiplayerSessionConfig multiplayerSessionConfig, Features.LevelModule.Scripts.LevelModel levelModel, ChapterProgressSyncModel chapterProgressSyncModel, ISessionRunStarter sessionRunStarter, IMultiplayerService multiplayerService, CurrentSettingsModel currentSettingsModel, GameAnalyticsEventSendService gameAnalyticsEventSendService)
		{
			_multiplayerModel = multiplayerModel;
			_gameFlowStateMachine = gameFlowStateMachine;
			_disconnectRequestEventClass = disconnectRequestEventClass;
			_gameUpdater = gameUpdater;
			_playerCustomizationModel = playerCustomizationModel;
			_loadingScreenService = loadingScreenService;
			_startGameLoadingNetworkEvent = startGameLoadingNetworkEvent;
			_settingsWindow = settingsWindow;
			_wantToLeaveLobbyPopupModel = wantToLeaveLobbyPopupModel;
			_languageService = languageService;
			_localizationService = localizationService;
			_steamInviteService = steamInviteService;
			_steamModel = steamModel;
			_confirmExitService = confirmExitService;
			_playerProfileModel = playerProfileModel;
			_savingService = savingService;
			_eventBus = eventBus;
			_navigationService = navigationService;
			_multiplayerSessionConfig = multiplayerSessionConfig;
			_levelModel = levelModel;
			_chapterProgressSyncModel = chapterProgressSyncModel;
			_sessionRunStarter = sessionRunStarter;
			_multiplayerService = multiplayerService;
			_currentSettingsModel = currentSettingsModel;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
		}

		protected override void OnViewSet()
		{
			Color playerColor = _playerProfileModel.PlayerColor;
			base.View.RSlider.value = playerColor.r;
			base.View.GSlider.value = playerColor.g;
			base.View.BSlider.value = playerColor.b;
			_lastSavedColor = playerColor;
			_currentNickname = _playerProfileModel.PlayerName;
			_lastSavedNickname = _currentNickname;
			if (base.View.NicknameInput != null)
			{
				base.View.NicknameInput.SetTextWithoutNotify(_currentNickname);
			}
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			RefreshSessionCodeText();
			base.View.CopyCodeButton.onClick.AddListener(Copy);
			base.View.StartGameButton.onClick.AddListener(StartGame);
			base.View.ExitLobbyButton.onClick.AddListener(OnExitLobbyClicked);
			_eventBus.Subscribe<OnPlayerLeftEvent>(HandleLobbyHostChanged);
			_eventBus.Subscribe<OnHostMigrationEvent>(HandleLobbyHostMigration);
			RefreshHostControls(force: true);
			_levelModel.OnSelectedChapterChanged += OnSelectedChapterChanged;
			_chapterProgressSyncModel.OnChanged += OnChapterProgressChanged;
			SelectFirstNavigationButton();
			_settingsWindow.OnWindowClosed += OnSettingsWindowClosed;
			_gameUpdater.OnUpdate += OnUpdate;
			SetLocalizedText();
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(SetLocalizedText));
			base.View.InviteButton.onClick.AddListener(OnInviteButtonClicked);
			if (base.View.PublicToggle != null)
			{
				base.View.PublicToggle.onValueChanged.AddListener(OnPublicToggleChanged);
			}
			_steamInviteService.OnSteamInitialized += UpdateInviteButtonVisibility;
			UpdateInviteButtonVisibility();
			base.View.OnFocused += OnViewFocused;
			if (base.View.NicknameInput != null)
			{
				base.View.NicknameInput.onValueChanged.AddListener(OnNicknameInputChanged);
			}
			_currentSettingsModel.OnSettingsDataHolderChanged += RefreshSessionCodeText;
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.CopyCodeButton.onClick.RemoveListener(Copy);
			base.View.StartGameButton.onClick.RemoveListener(StartGame);
			base.View.ExitLobbyButton.onClick.RemoveListener(OnExitLobbyClicked);
			_levelModel.OnSelectedChapterChanged -= OnSelectedChapterChanged;
			_chapterProgressSyncModel.OnChanged -= OnChapterProgressChanged;
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(HandleLobbyHostChanged);
			_eventBus.Unsubscribe<OnHostMigrationEvent>(HandleLobbyHostMigration);
			SetStartGameLoadingListenerEnabled(enabled: false);
			_settingsWindow.OnWindowClosed -= OnSettingsWindowClosed;
			_gameUpdater.OnUpdate -= OnUpdate;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(SetLocalizedText));
			base.View.InviteButton.onClick.RemoveListener(OnInviteButtonClicked);
			if (base.View.PublicToggle != null)
			{
				base.View.PublicToggle.onValueChanged.RemoveListener(OnPublicToggleChanged);
			}
			_steamInviteService.OnSteamInitialized -= UpdateInviteButtonVisibility;
			base.View.OnFocused -= OnViewFocused;
			if (base.View.NicknameInput != null)
			{
				base.View.NicknameInput.onValueChanged.RemoveListener(OnNicknameInputChanged);
			}
			_currentSettingsModel.OnSettingsDataHolderChanged -= RefreshSessionCodeText;
			FlushPendingNickname();
		}

		private void OnViewFocused()
		{
			SelectFirstNavigationButton();
		}

		private void SelectFirstNavigationButton()
		{
			if (base.View.FirstButtonToSelect != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstButtonToSelect);
			}
		}

		private void HandleLobbyHostChanged(OnPlayerLeftEvent _)
		{
			RefreshHostControls();
		}

		private void HandleLobbyHostMigration(OnHostMigrationEvent _)
		{
			RefreshHostControls();
		}

		private void RefreshHostControls(bool force = false)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			bool isSharedModeMasterClient = networkRunner.IsSharedModeMasterClient;
			if (force || isSharedModeMasterClient != _isSharedModeMasterClient)
			{
				_isSharedModeMasterClient = isSharedModeMasterClient;
				base.View.StartGameButton.gameObject.SetActive(isSharedModeMasterClient);
				RefreshPublicToggle(networkRunner, isSharedModeMasterClient);
				UpdateStartGameInteractable();
				base.View.WaitingTextContainer.SetActive(!isSharedModeMasterClient);
				SetStartGameLoadingListenerEnabled(!isSharedModeMasterClient);
				if (!isSharedModeMasterClient)
				{
					ResetWaitingDotsAnimation();
				}
			}
		}

		private void RefreshPublicToggle(NetworkRunner runner, bool isSharedModeMasterClient)
		{
			if (base.View.PublicToggleContainer != null)
			{
				base.View.PublicToggleContainer.SetActive(isSharedModeMasterClient);
			}
			if (!(base.View.PublicToggle == null))
			{
				base.View.PublicToggle.gameObject.SetActive(isSharedModeMasterClient);
				if (isSharedModeMasterClient)
				{
					bool value;
					bool isOnWithoutNotify = _multiplayerService.TryGetSessionProperty<bool>(runner.SessionInfo, SessionPropertyType.IsPublic, out value) && value;
					base.View.PublicToggle.SetIsOnWithoutNotify(isOnWithoutNotify);
				}
			}
		}

		private void OnPublicToggleChanged(bool isPublic)
		{
			_multiplayerService.SetSessionPublic(_multiplayerModel.NetworkRunner, isPublic);
		}

		private void SetStartGameLoadingListenerEnabled(bool enabled)
		{
			if (enabled != _isListeningForStartGameLoading)
			{
				if (enabled)
				{
					_startGameLoadingNetworkEvent.OnNetworkEventSend += StartLoadingTransition;
				}
				else
				{
					_startGameLoadingNetworkEvent.OnNetworkEventSend -= StartLoadingTransition;
				}
				_isListeningForStartGameLoading = enabled;
			}
		}

		private void SetLocalizedText()
		{
			_waitingTextBase = _localizationService.GetLocalizedString(base.View.WaitingLocalizationKey).TrimEnd('.');
			UpdateWaitingText();
		}

		private void ResetWaitingDotsAnimation()
		{
			_waitingDotsTimer = 0f;
			_waitingDotsIndex = 0;
			UpdateWaitingText();
		}

		private void UpdateWaitingDots()
		{
			if (!_isSharedModeMasterClient)
			{
				_waitingDotsTimer += Time.unscaledDeltaTime;
				if (!(_waitingDotsTimer < base.View.WaitingDotsInterval))
				{
					_waitingDotsTimer = 0f;
					_waitingDotsIndex = (_waitingDotsIndex + 1) % 4;
					UpdateWaitingText();
				}
			}
		}

		private void UpdateWaitingText()
		{
			if (!(base.View.WaitingText == null))
			{
				base.View.WaitingText.SetText(_waitingTextBase + new string('.', _waitingDotsIndex));
			}
		}

		private void RefreshSessionCodeText()
		{
			string name = _multiplayerModel.NetworkRunner.SessionInfo.Name;
			string text = (_currentSettingsModel.StreamerModeEnabled ? new string('?', _multiplayerSessionConfig.RoomCodeLength) : name);
			base.View.CodeText.SetText(text);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.View.CodeContainer);
		}

		private void Copy()
		{
			GUIUtility.systemCopyBuffer = _multiplayerModel.NetworkRunner.SessionInfo.Name;
		}

		private void OnSelectedChapterChanged(int _)
		{
			UpdateStartGameInteractable();
		}

		private void OnChapterProgressChanged()
		{
			UpdateStartGameInteractable();
		}

		private void UpdateStartGameInteractable()
		{
			if (_isSharedModeMasterClient)
			{
				base.View.StartGameButton.interactable = IsSelectedChapterAvailable();
			}
		}

		private bool IsSelectedChapterAvailable()
		{
			return _chapterProgressSyncModel.IsChapterAvailable(_levelModel.SelectedChapterIndex);
		}

		private void StartGame()
		{
			base.View.StartGameButton.interactable = false;
			_gameAnalyticsEventSendService.TrackLobbyClick(LobbyClickAnalyticsButton.GameStarted);
			_sessionRunStarter.RequestStart();
		}

		private void StartLoadingTransition(StartGameLoadingNetworkEvent obj)
		{
			_loadingScreenService.Show(LoadingScreenShowType.ShowUntilPlayersLoading);
		}

		private void OnExitLobbyClicked()
		{
			if (!(Time.unscaledTime < _ignoreExitLobbyUntilUnscaledTime))
			{
				_confirmExitService.ShowPopup(OnConfirmLeaveFromLobby, new ConfirmExitData
				{
					DescriptionLocalizationKey = LocalizationKey.UI_WantLeaveLobbyPopup_Description,
					TitleLocalizationKey = LocalizationKey.UI_WantLeaveLobbyPopup_Title
				});
			}
		}

		private void OnConfirmLeaveFromLobby()
		{
			_gameAnalyticsEventSendService.TrackLobbyClick(LobbyClickAnalyticsButton.LobbyLeave);
			_disconnectRequestEventClass.Publish(DisconnectRequestReason.LobbyLeave);
		}

		private void OnSettingsWindowClosed(Type _)
		{
			RefreshSessionCodeText();
			_ignoreExitLobbyUntilUnscaledTime = Time.unscaledTime + base.View.BackButtonClickDelay;
			_isExitLobbyButtonInCooldown = true;
			if (base.View.ExitLobbyButton != null)
			{
				base.View.ExitLobbyButton.interactable = false;
			}
		}

		private void OnUpdate()
		{
			UpdateExitLobbyCooldown();
			UpdateColor();
			UpdateNickname();
			UpdateWaitingDots();
			RefreshHostControls();
		}

		private void UpdateExitLobbyCooldown()
		{
			if (_isExitLobbyButtonInCooldown && !(Time.unscaledTime < _ignoreExitLobbyUntilUnscaledTime))
			{
				_isExitLobbyButtonInCooldown = false;
				if (base.View.ExitLobbyButton != null)
				{
					base.View.ExitLobbyButton.interactable = true;
				}
			}
		}

		private void UpdateColor()
		{
			if (_saveColorCooldown > 0f)
			{
				_saveColorCooldown -= Time.deltaTime;
			}
			Color color = new Color(base.View.RSlider.value, base.View.GSlider.value, base.View.BSlider.value);
			if (base.View.FinalColorImage.color != color)
			{
				base.View.FinalColorImage.color = color;
			}
			if (_lastSavedColor != color && _saveColorCooldown <= 0f)
			{
				SavePlayerColor(color);
				_saveColorCooldown = base.View.SaveColorDelay;
			}
		}

		private void SavePlayerColor(Color color)
		{
			_playerProfileModel.SetPlayerColor(color);
			_savingService.SaveDataForGroup(SavingGroup.PlayerProfile);
			_lastSavedColor = color;
			int localPlayerPlayerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData s) => s.PlayerId == localPlayerPlayerId);
			if (playerCustomizationSlotData != null)
			{
				playerCustomizationSlotData.PrimaryColor = color;
				playerCustomizationSlotData.VariableColor = color;
				_playerCustomizationModel.OverrideSlotData(localPlayerPlayerId, playerCustomizationSlotData);
			}
		}

		private void OnNicknameInputChanged(string nickname)
		{
			if (nickname.Length > _multiplayerSessionConfig.PlayerNameMaxLength)
			{
				nickname = nickname.Substring(0, _multiplayerSessionConfig.PlayerNameMaxLength);
			}
			nickname = new string(nickname.Where((char character) => HubMenuConstants.PossibleNameCharacters.Contains(character)).ToArray());
			base.View.NicknameInput.SetTextWithoutNotify(nickname);
			_currentNickname = nickname;
		}

		private void UpdateNickname()
		{
			if (_saveNicknameCooldown > 0f)
			{
				_saveNicknameCooldown -= Time.deltaTime;
			}
			if (!string.IsNullOrEmpty(_currentNickname) && !(_currentNickname == _lastSavedNickname) && _saveNicknameCooldown <= 0f)
			{
				SavePlayerNickname(_currentNickname);
				_saveNicknameCooldown = base.View.SaveNicknameDelay;
			}
		}

		private void FlushPendingNickname()
		{
			if (!string.IsNullOrEmpty(_currentNickname) && !(_currentNickname == _lastSavedNickname))
			{
				SavePlayerNickname(_currentNickname);
			}
		}

		private void SavePlayerNickname(string nickname)
		{
			_playerProfileModel.SetPlayerName(nickname);
			_savingService.SaveDataForGroup(SavingGroup.PlayerProfile);
			_lastSavedNickname = nickname;
			int localPlayerPlayerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData s) => s.PlayerId == localPlayerPlayerId);
			if (playerCustomizationSlotData != null)
			{
				playerCustomizationSlotData.Nickname = nickname;
				_playerCustomizationModel.OverrideSlotData(localPlayerPlayerId, playerCustomizationSlotData);
			}
		}

		private void OnInviteButtonClicked()
		{
			_steamInviteService.InviteToCurrentLobby();
		}

		private void UpdateInviteButtonVisibility()
		{
			if (!(base.View.InviteButton == null))
			{
				base.View.InviteButton.gameObject.SetActive(_steamModel.IsSteamInitialized);
			}
		}
	}
}
