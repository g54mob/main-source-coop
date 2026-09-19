using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Global.StateMachinesModule.Scripts;
using NetworkServices.NetworkEvents;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts
{
	public class LobbyHostMigrationSystem : IInitializable, IDisposable
	{
		private const int MaxMasterWaitFrames = 600;

		private const int MigrationLockTimeoutSeconds = 30;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly IMultiplayerService _multiplayerService;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly SemaphoreSlim _migrationLock = new SemaphoreSlim(1, 1);

		public LobbyHostMigrationSystem(NetworkRunnerEventBus eventBus, IMultiplayerService multiplayerService, GameFlowStateMachine gameFlowStateMachine)
		{
			_eventBus = eventBus;
			_multiplayerService = multiplayerService;
			_gameFlowStateMachine = gameFlowStateMachine;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
			_eventBus.Subscribe<OnHostMigrationEvent>(HandleHostMigration);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
			_eventBus.Unsubscribe<OnHostMigrationEvent>(HandleHostMigration);
			_migrationLock.Dispose();
		}

		private void HandlePlayerLeft(OnPlayerLeftEvent evt)
		{
			if (IsInLobby(evt.Runner) && !(evt.Player == evt.Runner.LocalPlayer))
			{
				RunLobbyHostMigrationAsync(evt.Runner).Forget();
			}
		}

		private void HandleHostMigration(OnHostMigrationEvent evt)
		{
			if (IsInLobby(evt.Runner))
			{
				RunLobbyHostMigrationAsync(evt.Runner).Forget();
			}
		}

		private bool IsInLobby(NetworkRunner runner)
		{
			if (runner == null || !runner.IsRunning)
			{
				return false;
			}
			return _gameFlowStateMachine.CurrentState == GameFlowState.LobbyGameState;
		}

		private async UniTaskVoid RunLobbyHostMigrationAsync(NetworkRunner runner)
		{
			using CancellationTokenSource timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30.0));
			try
			{
				await _migrationLock.WaitAsync(timeoutCts.Token);
			}
			catch (OperationCanceledException)
			{
				return;
			}
			try
			{
				if (await TryWaitUntilSharedMasterAsync(runner, timeoutCts.Token))
				{
					_multiplayerService.RefreshSessionVisibility(runner);
				}
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				_migrationLock.Release();
			}
		}

		private static async UniTask<bool> TryWaitUntilSharedMasterAsync(NetworkRunner runner, CancellationToken token)
		{
			for (int i = 0; i < 600; i++)
			{
				token.ThrowIfCancellationRequested();
				if (runner != null && runner.IsRunning && runner.IsSharedModeMasterClient)
				{
					return true;
				}
				await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
			}
			return runner != null && runner.IsRunning && runner.IsSharedModeMasterClient;
		}
	}
}
