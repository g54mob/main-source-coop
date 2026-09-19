using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using Features.InputModule.Scripts.Generated;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.UINavigationModuleRealization.Scripts.TextInput;
using Features.ViewSystemModule.Scripts.Windows;
using Fusion;
using NetworkServices.NetworkEvents;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class ChapterTypeSelectionPresenter : PresenterBehaviour<ChapterTypeSelectionViewBase>
	{
		private readonly LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ChapterTypeLocalizationConfiguration _chapterTypeLocalizationConfiguration;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LobbyWindow _lobbyWindow;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly INavigationService _navigationService;

		private readonly IInputService _inputService;

		private readonly ITextInputFocusService _textInputFocusService;

		private readonly StartGameLoadingNetworkEvent _startGameLoadingNetworkEvent;

		private readonly IGameUpdater _gameUpdater;

		private readonly DiContainer _container;

		private readonly List<ChapterTypeOptionPresenter> _optionPresenters = new List<ChapterTypeOptionPresenter>();

		private bool _isHost;

		private bool _isLocked;

		private bool _isInputSubscribed;

		private bool _hasHostState;

		private const int CLIENT_SCROLL_SETTLE_FRAMES = 5;

		private int _pendingClientScrollFrames;

		public ChapterTypeSelectionPresenter(LevelModel levelModel, LevelsConfiguration levelsConfiguration, ChapterTypeLocalizationConfiguration chapterTypeLocalizationConfiguration, MultiplayerModel multiplayerModel, LobbyWindow lobbyWindow, NetworkRunnerEventBus networkRunnerEventBus, INavigationService navigationService, IInputService inputService, ITextInputFocusService textInputFocusService, StartGameLoadingNetworkEvent startGameLoadingNetworkEvent, IGameUpdater gameUpdater, DiContainer container)
		{
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_chapterTypeLocalizationConfiguration = chapterTypeLocalizationConfiguration;
			_multiplayerModel = multiplayerModel;
			_lobbyWindow = lobbyWindow;
			_networkRunnerEventBus = networkRunnerEventBus;
			_navigationService = navigationService;
			_inputService = inputService;
			_textInputFocusService = textInputFocusService;
			_startGameLoadingNetworkEvent = startGameLoadingNetworkEvent;
			_gameUpdater = gameUpdater;
			_container = container;
		}

		protected override void OnViewSet()
		{
			_levelModel.OnSelectedChapterTypeChanged += OnSelectedChapterTypeChanged;
			_networkRunnerEventBus.Subscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_startGameLoadingNetworkEvent.OnNetworkEventSend += OnStartGameLoading;
			_gameUpdater.OnUpdate += OnUpdate;
			base.View.LeftNavigationButton.onClick.AddListener(TryNavigateLeft);
			base.View.RightNavigationButton.onClick.AddListener(TryNavigateRight);
			RefreshHostState(force: true);
		}

		protected override void OnDisposed()
		{
			_levelModel.OnSelectedChapterTypeChanged -= OnSelectedChapterTypeChanged;
			_networkRunnerEventBus.Unsubscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_startGameLoadingNetworkEvent.OnNetworkEventSend -= OnStartGameLoading;
			_gameUpdater.OnUpdate -= OnUpdate;
			base.View.LeftNavigationButton.onClick.RemoveListener(TryNavigateLeft);
			base.View.RightNavigationButton.onClick.RemoveListener(TryNavigateRight);
			SetInputSubscribed(isSubscribed: false);
			ClearOptions();
		}

		private void OnUpdate()
		{
			RefreshHostState();
			TickPendingClientScroll();
		}

		private void TickPendingClientScroll()
		{
			if (_pendingClientScrollFrames > 0)
			{
				_pendingClientScrollFrames--;
				if (!_isHost)
				{
					ScrollToSelectedChapterType();
				}
			}
		}

		private void RequestClientScroll()
		{
			_pendingClientScrollFrames = 5;
			ScrollToSelectedChapterType();
		}

		private void OnHostMigration(OnHostMigrationEvent _)
		{
			RefreshHostState();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent _)
		{
			RefreshHostState();
		}

		private void RefreshHostState(bool force = false)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning)
			{
				bool isSharedModeMasterClient = networkRunner.IsSharedModeMasterClient;
				if (force || !_hasHostState || isSharedModeMasterClient != _isHost)
				{
					_hasHostState = true;
					_isHost = isSharedModeMasterClient;
					OnHostStateChanged();
				}
			}
		}

		private void OnHostStateChanged()
		{
			ClearOptions();
			if (_isHost)
			{
				_levelModel.SetSelectedChapterType(GetSelectedOrFirstChapterType());
			}
			BuildOptions();
			RefreshControls();
			RequestClientScroll();
		}

		private ChapterType GetSelectedOrFirstChapterType()
		{
			List<ChapterType> chapterTypes = _levelsConfiguration.GetChapterTypes(_levelModel.SelectedSequenceSet);
			if (chapterTypes.Count == 0)
			{
				return ChapterType.None;
			}
			if (!chapterTypes.Contains(_levelModel.SelectedChapterType))
			{
				return chapterTypes[0];
			}
			return _levelModel.SelectedChapterType;
		}

		private void BuildOptions()
		{
			foreach (ChapterType chapterType in _levelsConfiguration.GetChapterTypes(_levelModel.SelectedSequenceSet))
			{
				ChapterTypeOptionPresenter chapterTypeOptionPresenter = CreateOption(chapterType);
				if (_isHost)
				{
					chapterTypeOptionPresenter.OnClicked += OnChapterTypeOptionClicked;
				}
				else
				{
					chapterTypeOptionPresenter.Selectable.interactable = false;
				}
				_optionPresenters.Add(chapterTypeOptionPresenter);
			}
			RefreshSelectionVisuals(_levelModel.SelectedChapterType);
		}

		private ChapterTypeOptionPresenter CreateOption(ChapterType chapterType)
		{
			GameObject gameObject = _container.InstantiatePrefab(base.View.ChapterTypeOptionViewBase.gameObject);
			gameObject.name = $"{gameObject.name}{chapterType}";
			ChapterTypeOptionViewBase component = gameObject.GetComponent<ChapterTypeOptionViewBase>();
			_lobbyWindow.AddView(component.transform, worldPositionStays: false);
			ChapterTypeOptionPresenter presenterForView = _lobbyWindow.GetPresenterForView<ChapterTypeOptionPresenter>(component);
			presenterForView.SetParent(base.View.GetOptionsContainer());
			presenterForView.Setup(chapterType, _chapterTypeLocalizationConfiguration.GetLocalizationKeyForChapterType(chapterType));
			return presenterForView;
		}

		private void ClearOptions()
		{
			foreach (ChapterTypeOptionPresenter optionPresenter in _optionPresenters)
			{
				optionPresenter.OnClicked -= OnChapterTypeOptionClicked;
				optionPresenter.DestroyView();
			}
			_optionPresenters.Clear();
		}

		private void OnStartGameLoading(StartGameLoadingNetworkEvent _)
		{
			_isLocked = true;
			RefreshControls();
		}

		private void RefreshControls()
		{
			bool flag = _isHost && !_isLocked;
			base.View.ScrollNavigation.SetNavigationEnabled(flag);
			base.View.LeftNavigationButton.interactable = flag;
			base.View.RightNavigationButton.interactable = flag;
			base.View.LeftNavigationContainer.gameObject.SetActive(flag);
			base.View.RightNavigationContainer.gameObject.SetActive(flag);
			SetInputSubscribed(flag);
		}

		private void SetInputSubscribed(bool isSubscribed)
		{
			if (isSubscribed != _isInputSubscribed)
			{
				_isInputSubscribed = isSubscribed;
				if (isSubscribed)
				{
					InputDefaultActions additionalNavigationLeft = _inputService.AdditionalNavigationLeft;
					additionalNavigationLeft.Performed = (Action)Delegate.Combine(additionalNavigationLeft.Performed, new Action(TryNavigateLeftFromHotkey));
					InputDefaultActions additionalNavigationRight = _inputService.AdditionalNavigationRight;
					additionalNavigationRight.Performed = (Action)Delegate.Combine(additionalNavigationRight.Performed, new Action(TryNavigateRightFromHotkey));
				}
				else
				{
					InputDefaultActions additionalNavigationLeft2 = _inputService.AdditionalNavigationLeft;
					additionalNavigationLeft2.Performed = (Action)Delegate.Remove(additionalNavigationLeft2.Performed, new Action(TryNavigateLeftFromHotkey));
					InputDefaultActions additionalNavigationRight2 = _inputService.AdditionalNavigationRight;
					additionalNavigationRight2.Performed = (Action)Delegate.Remove(additionalNavigationRight2.Performed, new Action(TryNavigateRightFromHotkey));
				}
			}
		}

		private void OnSelectedChapterTypeChanged(ChapterType chapterType)
		{
			RefreshSelectionVisuals(chapterType);
			RequestClientScroll();
		}

		private void ScrollToSelectedChapterType()
		{
			int optionIndexOfType = GetOptionIndexOfType(_levelModel.SelectedChapterType);
			if (optionIndexOfType >= 0)
			{
				base.View.ScrollNavigation.ScrollToIndex(optionIndexOfType);
			}
		}

		private void OnChapterTypeOptionClicked(ChapterType chapterType)
		{
			if (!_isLocked)
			{
				_levelModel.SetSelectedChapterType(chapterType);
			}
		}

		private void RefreshSelectionVisuals(ChapterType selectedChapterType)
		{
			foreach (ChapterTypeOptionPresenter optionPresenter in _optionPresenters)
			{
				optionPresenter.SetSelected(optionPresenter.ChapterType == selectedChapterType);
			}
		}

		private int GetOptionIndexOfType(ChapterType chapterType)
		{
			for (int i = 0; i < _optionPresenters.Count; i++)
			{
				if (_optionPresenters[i].ChapterType == chapterType)
				{
					return i;
				}
			}
			return -1;
		}

		private void TryNavigateRight()
		{
			TryNavigate(1);
		}

		private void TryNavigateLeft()
		{
			TryNavigate(-1);
		}

		private void TryNavigateRightFromHotkey()
		{
			if (!_textInputFocusService.IsTextInputEditing)
			{
				TryNavigateRight();
			}
		}

		private void TryNavigateLeftFromHotkey()
		{
			if (!_textInputFocusService.IsTextInputEditing)
			{
				TryNavigateLeft();
			}
		}

		private void TryNavigate(int step)
		{
			if (_isHost && !_isLocked && _optionPresenters.Count != 0)
			{
				int num = GetCurrentOptionIndex() + step;
				if (num >= 0 && num < _optionPresenters.Count)
				{
					ExecuteEvents.Execute(_optionPresenters[num].Selectable.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
				}
			}
		}

		private int GetCurrentOptionIndex()
		{
			return Mathf.Max(0, GetOptionIndexOfType(_levelModel.SelectedChapterType));
		}
	}
}
