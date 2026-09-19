using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionLobbyChapterProgressSystem : IInitializable, IDisposable
	{
		private const int MAX_MASTER_WAIT_FRAMES = 600;

		private readonly SessionStateMachine _sessionStateMachine;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ChapterProgressModel _chapterProgressModel;

		private readonly ChapterProgressSyncModel _chapterProgressSyncModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public SessionLobbyChapterProgressSystem(SessionStateMachine sessionStateMachine, MultiplayerModel multiplayerModel, Features.LevelModule.Scripts.LevelModel levelModel, LevelsConfiguration levelsConfiguration, ChapterProgressModel chapterProgressModel, ChapterProgressSyncModel chapterProgressSyncModel, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_sessionStateMachine = sessionStateMachine;
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_chapterProgressModel = chapterProgressModel;
			_chapterProgressSyncModel = chapterProgressSyncModel;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void Initialize()
		{
			_sessionStateMachine.CurrentChanged += OnCurrentChanged;
			_networkRunnerEventBus.Subscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			if (IsInLobby())
			{
				PublishHostProgress();
			}
		}

		public void Dispose()
		{
			_sessionStateMachine.CurrentChanged -= OnCurrentChanged;
			_networkRunnerEventBus.Unsubscribe<OnHostMigrationEvent>(OnHostMigration);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		private void OnCurrentChanged(SessionState state)
		{
			if (state == SessionState.Lobby)
			{
				PublishHostProgress();
			}
		}

		private void OnHostMigration(OnHostMigrationEvent _)
		{
			RepublishAfterMigrationAsync().Forget();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent evt)
		{
			if (!(evt.Player == evt.Runner.LocalPlayer))
			{
				RepublishAfterMigrationAsync().Forget();
			}
		}

		private async UniTaskVoid RepublishAfterMigrationAsync()
		{
			try
			{
				for (int i = 0; i < 600; i++)
				{
					if (!IsInLobby())
					{
						break;
					}
					if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
					{
						PublishHostProgress();
						break;
					}
					await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _cancellationTokenSource.Token);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private bool IsInLobby()
		{
			return _sessionStateMachine.Current == SessionState.Lobby;
		}

		private void PublishHostProgress()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				LevelSequenceSet selectedSequenceSet = _levelModel.SelectedSequenceSet;
				List<Chapter> value;
				int chaptersCount = (_levelsConfiguration.ChapterSequences.TryGetValue(selectedSequenceSet, out value) ? value.Count : 0);
				_chapterProgressSyncModel.Publish(selectedSequenceSet, chaptersCount, _chapterProgressModel.GetPassedChapters(selectedSequenceSet), _chapterProgressModel.GetSubLevelProgress(selectedSequenceSet));
			}
		}
	}
}
