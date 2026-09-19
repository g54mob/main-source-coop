using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.DisconnectHandlerModule.Scripts;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.ProgressSavingModule.Scripts.PlayerInfo;
using Features.SceneManagement;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Global.StateMachinesModule.Scripts;
using RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class ProjectBootstrap : MonoBehaviour
	{
		private GameFlowStateMachine _gameFlowStateMachine;

		private AddressablesSceneLoaderService _addressablesSceneLoaderService;

		private INetworkSceneLoaderServiceFacade _networkSceneLoaderService;

		private MultiplayerModel _multiplayerModel;

		private IMultiplayerService _multiplayerService;

		private ILevelService _levelService;

		private LevelModel _levelModel;

		private LevelsConfiguration _levelsConfiguration;

		private PlayerInfoModel _playerInfoModel;

		private ISavingService _savingService;

		private ILoadingScreenService _loadingScreenService;

		[Inject]
		public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine, AddressablesSceneLoaderService addressablesSceneLoaderService, INetworkSceneLoaderServiceFacade networkSceneLoaderService, MultiplayerModel multiplayerModel, IMultiplayerService multiplayerService, ILevelService levelService, LevelModel levelModel, LevelsConfiguration levelsConfiguration, PlayerInfoModel playerInfoModel, ISavingService savingService, ILoadingScreenService loadingScreenService)
		{
			_gameFlowStateMachine = gameFlowStateMachine;
			_addressablesSceneLoaderService = addressablesSceneLoaderService;
			_networkSceneLoaderService = networkSceneLoaderService;
			_multiplayerModel = multiplayerModel;
			_multiplayerService = multiplayerService;
			_levelService = levelService;
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_playerInfoModel = playerInfoModel;
			_savingService = savingService;
			_loadingScreenService = loadingScreenService;
		}

		private void Start()
		{
			RegisterGameLaunch();
			_gameFlowStateMachine.OnStateEnter += HandleGameFlow;
			_gameFlowStateMachine.EnterState(GameFlowState.GlobalSceneState);
		}

		private void OnDestroy()
		{
			if (_gameFlowStateMachine != null)
			{
				_gameFlowStateMachine.OnStateEnter -= HandleGameFlow;
			}
		}

		private async void HandleGameFlow(GameFlowState gameFlowState)
		{
			if (!SceneLoadGuard.CanPerformSceneOperations)
			{
				return;
			}
			CancellationToken cancellationTokenOnDestroy = this.GetCancellationTokenOnDestroy();
			try
			{
				switch (gameFlowState)
				{
				case GameFlowState.UnknowState:
					Debug.LogError("GameFlowState is UnknowState");
					break;
				case GameFlowState.GlobalSceneState:
					await _addressablesSceneLoaderService.LoadScenesAsync(new List<string> { "GlobalScene" }, unloadRedundant: true).AttachExternalCancellation(cancellationTokenOnDestroy);
					if (SceneManager.GetSceneByName("BootstrapScene").IsValid())
					{
						await SceneLoadGuard.AwaitAsyncOperation(SceneManager.UnloadSceneAsync("BootstrapScene"));
					}
					break;
				case GameFlowState.MenuGameState:
					await _addressablesSceneLoaderService.LoadScenesAsync(new List<string> { "GlobalScene", "MenuScene" }, unloadRedundant: true).AttachExternalCancellation(cancellationTokenOnDestroy);
					break;
				case GameFlowState.SessionGameState:
					if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.IsRunning && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
					{
						if (_levelModel.SelectedLevel == LevelType.None)
						{
							_levelModel.SetSelectedLevel(LevelType.CoreLoopScene);
						}
						string selectedLevelName = _levelService.GetSelectedLevelName();
						if (!_loadingScreenService.IsVisible)
						{
							_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade);
						}
						if (!(selectedLevelName == _levelService.GetLevelNameByType(LevelType.CoreLoopScene)) && !(selectedLevelName == _levelService.GetLevelNameByType(LevelType.TutorialLoopScene)))
						{
							await _levelService.LoadLevel(_levelModel.SelectedLevel, pauseGame: false).AttachExternalCancellation(cancellationTokenOnDestroy);
						}
						else
						{
							_levelModel.ParentalLevel = _levelModel.SelectedLevel;
							PrimeChapterCursor();
							await _networkSceneLoaderService.LoadSceneAsync(selectedLevelName, NetworkSceneLoadingFlags.SyncLoading).AttachExternalCancellation(cancellationTokenOnDestroy);
						}
					}
					if (SceneManager.GetSceneByName("MenuScene").IsValid())
					{
						await SceneLoadGuard.AwaitAsyncOperation(SceneManager.UnloadSceneAsync("MenuScene"));
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("gameFlowState", gameFlowState, null);
				case GameFlowState.LobbyGameState:
					break;
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private void PrimeChapterCursor()
		{
			if (_levelModel.SelectedSequenceSet == LevelSequenceSet.None)
			{
				_levelModel.SetSelectedSequenceSet(LevelSequenceSet.Default, sync: false);
			}
			int num = 0;
			int num2 = 0;
			if (_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out var value) && value.Count > 0)
			{
				num = Mathf.Clamp(_levelModel.SelectedChapterIndex, 0, value.Count - 1);
				for (int i = 0; i < num && i < value.Count; i++)
				{
					num2 += value[i].Levels.Count;
				}
			}
			_levelModel.CurrentLevelNumber = num2;
			_levelModel.SetChapterCursor(num, 0, sync: false);
			_levelModel.Synchronize();
		}

		private void RegisterGameLaunch()
		{
			if (!ProjectLaunchSessionState.IsGameLaunchRegistered)
			{
				_playerInfoModel.IncrementGameLaunchCount();
				_savingService.SaveDataForGroup(SavingGroup.PlayerInfo);
				ProjectLaunchSessionState.MarkGameLaunchRegistered();
			}
		}
	}
}
