using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using NetworkServices.NetworkEvents;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class SharedMasterMigrationService : IDisposable
	{
		private const int MaxMasterWaitFrames = 600;

		private const int MaxAuthorityWaitFrames = 600;

		private const int MigrationLockTimeoutSeconds = 30;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IReadOnlyList<ISharedModeMasterMigrationHandler> _migrationHandlers;

		private readonly SemaphoreSlim _migrationLock = new SemaphoreSlim(1, 1);

		public SharedMasterMigrationService(NetworkRunnerEventBus eventBus, MultiplayerModel multiplayerModel, IEnumerable<ISharedModeMasterMigrationHandler> migrationHandlers)
		{
			_eventBus = eventBus;
			_multiplayerModel = multiplayerModel;
			_migrationHandlers = ((migrationHandlers != null) ? migrationHandlers.OrderBy((ISharedModeMasterMigrationHandler h) => h.SharedModeMasterMigrationHandleOrder).ToList() : new List<ISharedModeMasterMigrationHandler>());
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
			if (!(evt.Runner == null) && evt.Runner.IsRunning && !(evt.Player == evt.Runner.LocalPlayer))
			{
				RunSharedMasterMigrationAsync(evt.Runner, evt.Player).Forget();
			}
		}

		private void HandleHostMigration(OnHostMigrationEvent evt)
		{
			if (!(evt.Runner == null) && evt.Runner.IsRunning)
			{
				RunSharedMasterMigrationAsync(evt.Runner, null).Forget();
			}
		}

		private async UniTaskVoid RunSharedMasterMigrationAsync(NetworkRunner runner, PlayerRef? leftPlayer)
		{
			using CancellationTokenSource timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30.0));
			await _migrationLock.WaitAsync(timeoutCts.Token);
			try
			{
				await ExecuteMigrationAsync(runner, leftPlayer, timeoutCts.Token);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception)
			{
			}
			finally
			{
				_migrationLock.Release();
			}
		}

		private async UniTask ExecuteMigrationAsync(NetworkRunner runner, PlayerRef? leftPlayer, CancellationToken token)
		{
			if (!(await TryWaitUntilSharedMasterAsync(runner, token)))
			{
				return;
			}
			runner.SessionInfo.IsOpen = true;
			_multiplayerModel.InvokeSessionOpenedChanged(runner.SessionInfo.IsOpen);
			HashSet<PlayerRef> activePlayers = new HashSet<PlayerRef>(runner.ActivePlayers);
			HashSet<NetworkObject> hashSet = new HashSet<NetworkObject>();
			if (_multiplayerModel.NetworkMasterClientTracker != null && _multiplayerModel.NetworkMasterClientTracker.Object != null)
			{
				NetworkObject networkObject = _multiplayerModel.NetworkMasterClientTracker.Object;
				if (!networkObject.HasStateAuthority && !IsStateAuthorityInActiveSet(networkObject, activePlayers))
				{
					hashSet.Add(networkObject);
				}
			}
			if (leftPlayer.HasValue && leftPlayer.Value != PlayerRef.None)
			{
				foreach (NetworkObject item in CollectWithStateAuthority(runner, leftPlayer.Value))
				{
					hashSet.Add(item);
				}
			}
			foreach (NetworkObject item2 in CollectOrphanedByInactiveAuthority(runner, activePlayers))
			{
				hashSet.Add(item2);
			}
			List<NetworkObject> list = hashSet.ToList();
			foreach (NetworkObject item3 in list)
			{
				token.ThrowIfCancellationRequested();
				if (!(item3 == null) && item3.IsValid && !item3.HasStateAuthority && (item3.Flags & NetworkObjectFlags.MasterClientObject) == 0)
				{
					RequestAuthority(item3);
				}
			}
			if (list.Count > 0)
			{
				UniTask[] array = new UniTask[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = WaitForLocalStateAuthorityOrFailAsync(list[i], token);
				}
				await UniTask.WhenAll(array);
			}
			if (runner.IsSharedModeMasterClient)
			{
				foreach (ISharedModeMasterMigrationHandler migrationHandler in _migrationHandlers)
				{
					try
					{
						migrationHandler.OnSharedModeMasterMigrationCompleted(runner);
					}
					catch (Exception)
					{
					}
				}
			}
			await UniTask.Yield(PlayerLoopTiming.Update, token);
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

		private static bool IsStateAuthorityInActiveSet(NetworkObject networkObject, HashSet<PlayerRef> activePlayers)
		{
			PlayerRef stateAuthority = networkObject.StateAuthority;
			if (stateAuthority == PlayerRef.None)
			{
				return false;
			}
			return activePlayers.Contains(stateAuthority);
		}

		private static List<NetworkObject> CollectWithStateAuthority(NetworkRunner runner, PlayerRef player)
		{
			List<NetworkObject> list = new List<NetworkObject>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && allNetworkObject.StateAuthority == player)
				{
					list.Add(allNetworkObject);
				}
			}
			return list;
		}

		private static List<NetworkObject> CollectOrphanedByInactiveAuthority(NetworkRunner runner, HashSet<PlayerRef> activePlayers)
		{
			List<NetworkObject> list = new List<NetworkObject>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !allNetworkObject.HasStateAuthority && !IsStateAuthorityInActiveSet(allNetworkObject, activePlayers))
				{
					list.Add(allNetworkObject);
				}
			}
			return list;
		}

		private static void RequestAuthority(NetworkObject networkObject)
		{
			if (!(networkObject == null) && networkObject.IsValid)
			{
				networkObject.RequestStateAuthority();
			}
		}

		private static async UniTask WaitForLocalStateAuthorityOrFailAsync(NetworkObject networkObject, CancellationToken token)
		{
			int frames = 0;
			while (networkObject != null && networkObject.IsValid && !networkObject.HasStateAuthority && frames < 600)
			{
				token.ThrowIfCancellationRequested();
				await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
				frames++;
			}
		}
	}
}
