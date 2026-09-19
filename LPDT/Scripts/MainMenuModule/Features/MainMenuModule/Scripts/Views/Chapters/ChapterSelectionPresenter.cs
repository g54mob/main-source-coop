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
	public class ChapterSelectionPresenter : PresenterBehaviour<ChapterSelectionViewBase>
	{
		private readonly struct ChapterOptionEntry
		{
			public readonly ChapterOptionPresenter Presenter;

			public readonly int ChapterIndex;

			public readonly int LevelsCount;

			public ChapterOptionEntry(ChapterOptionPresenter presenter, int chapterIndex, int levelsCount)
			{
				Presenter = presenter;
				ChapterIndex = chapterIndex;
				LevelsCount = levelsCount;
			}
		}

		private readonly LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ChapterPreviewConfiguration _chapterPreviewConfiguration;

		private readonly ChapterProgressModel _chapterProgressModel;

		private readonly ChapterProgressSyncModel _chapterProgressSyncModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LobbyWindow _lobbyWindow;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly INavigationService _navigationService;

		private readonly IInputService _inputService;

		private readonly ITextInputFocusService _textInputFocusService;

		private readonly StartGameLoadingNetworkEvent _startGameLoadingNetworkEvent;

		private readonly IGameUpdater _gameUpdater;

		private readonly DiContainer _container;

		private readonly List<ChapterOptionEntry> _optionEntries = new List<ChapterOptionEntry>();

		private bool _isHost;

		private bool _isLocked;

		private bool _isInputSubscribed;

		private bool _hasHostState;

		private const int CLIENT_SCROLL_SETTLE_FRAMES = 5;

		private int _pendingClientScrollFrames;

		public ChapterSelectionPresenter(LevelModel levelModel, LevelsConfiguration levelsConfiguration, ChapterPreviewConfiguration chapterPreviewConfiguration, ChapterProgressModel chapterProgressModel, ChapterProgressSyncModel chapterProgressSyncModel, MultiplayerModel multiplayerModel, LobbyWindow lobbyWindow, NetworkRunnerEventBus networkRunnerEventBus, INavigationService navigationService, IInputService inputService, ITextInputFocusService textInputFocusService, StartGameLoadingNetworkEvent startGameLoadingNetworkEvent, IGameUpdater gameUpdater, DiContainer container)
		{
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_chapterPreviewConfiguration = chapterPreviewConfiguration;
			_chapterProgressModel = chapterProgressModel;
			_chapterProgressSyncModel = chapterProgressSyncModel;
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
			_levelModel.OnSelectedChapterChanged += OnSelectedChapterChanged;
			_levelModel.OnSelectedChapterTypeChanged += OnSelectedChapterTypeChanged;
			_networkRunnerEventBus.Subscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_startGameLoadingNetworkEvent.OnNetworkEventSend += OnStartGameLoading;
			_chapterProgressSyncModel.OnChanged += OnSyncProgressChanged;
			_gameUpdater.OnUpdate += OnUpdate;
			base.View.LeftNavigationButton.onClick.AddListener(TryNavigateLeft);
			base.View.RightNavigationButton.onClick.AddListener(TryNavigateRight);
			RefreshHostState(force: true);
		}

		protected override void OnDisposed()
		{
			_levelModel.OnSelectedChapterChanged -= OnSelectedChapterChanged;
			_levelModel.OnSelectedChapterTypeChanged -= OnSelectedChapterTypeChanged;
			_networkRunnerEventBus.Unsubscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_startGameLoadingNetworkEvent.OnNetworkEventSend -= OnStartGameLoading;
			_chapterProgressSyncModel.OnChanged -= OnSyncProgressChanged;
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
					ScrollToSelectedChapter();
				}
			}
		}

		private void RequestClientScroll()
		{
			_pendingClientScrollFrames = 5;
			ScrollToSelectedChapter();
		}

		private void OnHostMigration(OnHostMigrationEvent _)
		{
			RefreshHostState();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent _)
		{
			RefreshHostState();
		}

		private void BuildOptions()
		{
			LevelSequenceSet selectedSequenceSet = _levelModel.SelectedSequenceSet;
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(selectedSequenceSet, out var value))
			{
				return;
			}
			List<int> chapterIndexesOfType = _levelsConfiguration.GetChapterIndexesOfType(selectedSequenceSet, _levelModel.SelectedChapterType);
			bool isSharedModeMasterClient = _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
			bool flag = base.View.FinalChapterOptionViewBase != null;
			int num = chapterIndexesOfType.Count + (flag ? 1 : 0);
			for (int i = 0; i < num; i++)
			{
				bool num2 = i >= chapterIndexesOfType.Count;
				ChapterOptionViewBase prefab = (num2 ? base.View.FinalChapterOptionViewBase : base.View.ChapterOptionViewBase);
				int num3 = (num2 ? value.Count : chapterIndexesOfType[i]);
				int levelsCount = ((!num2) ? value[num3].Levels.Count : 0);
				ChapterOptionPresenter chapterOptionPresenter = CreateOption(prefab, num3, levelsCount);
				if (!num2)
				{
					ChapterPreviewData previewOrFallback = _chapterPreviewConfiguration.GetPreviewOrFallback(num3);
					if (previewOrFallback != null)
					{
						chapterOptionPresenter.SetChapterData(previewOrFallback);
					}
				}
				if (isSharedModeMasterClient)
				{
					chapterOptionPresenter.OnClicked += OnChapterOptionClicked;
				}
				else
				{
					chapterOptionPresenter.Selectable.interactable = false;
				}
				_optionEntries.Add(new ChapterOptionEntry(chapterOptionPresenter, num3, levelsCount));
			}
			RefreshAllOptions();
		}

		private ChapterOptionPresenter CreateOption(ChapterOptionViewBase prefab, int chapterIndex, int levelsCount)
		{
			GameObject gameObject = _container.InstantiatePrefab(prefab.gameObject);
			gameObject.name = $"{gameObject.name}{chapterIndex}";
			ChapterOptionViewBase component = gameObject.GetComponent<ChapterOptionViewBase>();
			_lobbyWindow.AddView(component.transform, worldPositionStays: false);
			ChapterOptionPresenter presenterForView = _lobbyWindow.GetPresenterForView<ChapterOptionPresenter>(component);
			presenterForView.SetParent(base.View.GetOptionsContainer());
			presenterForView.Setup(chapterIndex, levelsCount);
			return presenterForView;
		}

		private void ClearOptions()
		{
			foreach (ChapterOptionEntry optionEntry in _optionEntries)
			{
				optionEntry.Presenter.OnClicked -= OnChapterOptionClicked;
				optionEntry.Presenter.DestroyView();
			}
			_optionEntries.Clear();
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
			if (_isHost)
			{
				PublishHostProgress();
			}
			base.View.ShowOptionsInstantly();
			RebuildForSelectedChapterType();
		}

		private void OnSelectedChapterTypeChanged(ChapterType _)
		{
			base.View.PlayRebuildFade(RebuildForSelectedChapterType);
		}

		private void RebuildForSelectedChapterType()
		{
			ClearOptions();
			if (_isHost)
			{
				_levelModel.SetSelectedChapter(GetFirstAvailableChapterOfSelectedType());
			}
			BuildOptions();
			RefreshControls();
			if (!_isHost)
			{
				RequestClientScroll();
			}
		}

		private void PublishHostProgress()
		{
			LevelSequenceSet selectedSequenceSet = _levelModel.SelectedSequenceSet;
			List<Chapter> value;
			int chaptersCount = (_levelsConfiguration.ChapterSequences.TryGetValue(selectedSequenceSet, out value) ? value.Count : 0);
			_chapterProgressSyncModel.Publish(selectedSequenceSet, chaptersCount, _chapterProgressModel.GetPassedChapters(selectedSequenceSet), _chapterProgressModel.GetSubLevelProgress(selectedSequenceSet));
		}

		private void OnSyncProgressChanged()
		{
			RefreshAllOptions();
		}

		private void RefreshAllOptions()
		{
			if (_optionEntries.Count == 0)
			{
				return;
			}
			int selectedChapterIndex = _levelModel.SelectedChapterIndex;
			foreach (ChapterOptionEntry optionEntry in _optionEntries)
			{
				ApplyProgressAndState(optionEntry, selectedChapterIndex);
			}
		}

		private void ApplyProgressAndState(ChapterOptionEntry optionEntry, int selectedChapterIndex)
		{
			optionEntry.Presenter.SetProgress(optionEntry.LevelsCount, _chapterProgressSyncModel.GetReachedSubLevelCount(optionEntry.ChapterIndex), _chapterProgressSyncModel.IsChapterPassed(optionEntry.ChapterIndex));
			ApplyOptionState(optionEntry, selectedChapterIndex);
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
					InputDefaultActions extraAdditionalNavigationLeft = _inputService.ExtraAdditionalNavigationLeft;
					extraAdditionalNavigationLeft.Performed = (Action)Delegate.Combine(extraAdditionalNavigationLeft.Performed, new Action(TryNavigateLeftFromHotkey));
					InputDefaultActions extraAdditionalNavigationRight = _inputService.ExtraAdditionalNavigationRight;
					extraAdditionalNavigationRight.Performed = (Action)Delegate.Combine(extraAdditionalNavigationRight.Performed, new Action(TryNavigateRightFromHotkey));
				}
				else
				{
					InputDefaultActions extraAdditionalNavigationLeft2 = _inputService.ExtraAdditionalNavigationLeft;
					extraAdditionalNavigationLeft2.Performed = (Action)Delegate.Remove(extraAdditionalNavigationLeft2.Performed, new Action(TryNavigateLeftFromHotkey));
					InputDefaultActions extraAdditionalNavigationRight2 = _inputService.ExtraAdditionalNavigationRight;
					extraAdditionalNavigationRight2.Performed = (Action)Delegate.Remove(extraAdditionalNavigationRight2.Performed, new Action(TryNavigateRightFromHotkey));
				}
			}
		}

		private int GetFirstAvailableChapterOfSelectedType()
		{
			LevelSequenceSet selectedSequenceSet = _levelModel.SelectedSequenceSet;
			List<int> chapterIndexesOfType = _levelsConfiguration.GetChapterIndexesOfType(selectedSequenceSet, _levelModel.SelectedChapterType);
			if (chapterIndexesOfType.Count == 0)
			{
				return _levelModel.SelectedChapterIndex;
			}
			foreach (int item in chapterIndexesOfType)
			{
				if (_chapterProgressModel.IsChapterAvailable(selectedSequenceSet, item))
				{
					return item;
				}
			}
			return chapterIndexesOfType[0];
		}

		private void OnSelectedChapterChanged(int selectedChapterIndex)
		{
			RefreshSelectionVisuals(selectedChapterIndex);
			if (!_isHost)
			{
				RequestClientScroll();
			}
		}

		private void ScrollToSelectedChapter()
		{
			int optionIndexOfChapter = GetOptionIndexOfChapter(_levelModel.SelectedChapterIndex);
			if (optionIndexOfChapter >= 0)
			{
				base.View.ScrollNavigation.ScrollToIndex(optionIndexOfChapter);
			}
		}

		private void OnChapterOptionClicked(int chapterIndex)
		{
			if (!_isLocked)
			{
				_levelModel.SetSelectedChapter(chapterIndex);
				RefreshSelectionVisuals(chapterIndex);
			}
		}

		private void RefreshSelectionVisuals(int selectedChapterIndex)
		{
			foreach (ChapterOptionEntry optionEntry in _optionEntries)
			{
				ApplyOptionState(optionEntry, selectedChapterIndex);
			}
		}

		private void ApplyOptionState(ChapterOptionEntry optionEntry, int selectedChapterIndex)
		{
			optionEntry.Presenter.SetLocked(!_chapterProgressSyncModel.IsChapterAvailable(optionEntry.ChapterIndex));
			optionEntry.Presenter.SetSelected(optionEntry.ChapterIndex == selectedChapterIndex);
		}

		private int GetOptionIndexOfChapter(int chapterIndex)
		{
			for (int i = 0; i < _optionEntries.Count; i++)
			{
				if (_optionEntries[i].ChapterIndex == chapterIndex)
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
			if (_isHost && !_isLocked && _optionEntries.Count != 0)
			{
				int num = GetCurrentOptionIndex() + step;
				if (num >= 0 && num < _optionEntries.Count)
				{
					ExecuteEvents.Execute(_optionEntries[num].Presenter.Selectable.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
				}
			}
		}

		private int GetCurrentOptionIndex()
		{
			return Mathf.Max(0, GetOptionIndexOfChapter(_levelModel.SelectedChapterIndex));
		}
	}
}
