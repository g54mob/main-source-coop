using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class MasterOrphanAvatarCleanupSystem : IMasterOrphanAvatarCleanup, ISharedModeMasterMigrationHandler, IInitializable, IDisposable
	{
		private const float MasterCleanupDelaySeconds = 0.5f;

		private const float AuthorityReclaimTimeoutSeconds = 5f;

		private const float AuthorityReclaimPollSeconds = 0.1f;

		private readonly NetworkRunnerEventBus _eventBus;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPlayerStateLifecycleService _playerStateLifecycleService;

		private bool _masterCleanupScheduled;

		public int SharedModeMasterMigrationHandleOrder => 0;

		public MasterOrphanAvatarCleanupSystem(NetworkRunnerEventBus eventBus, MultiplayerModel multiplayerModel, IPlayerStateLifecycleService playerStateLifecycleService)
		{
			_eventBus = eventBus;
			_multiplayerModel = multiplayerModel;
			_playerStateLifecycleService = playerStateLifecycleService;
		}

		public void Initialize()
		{
			_eventBus.Subscribe<OnPlayerJoinedEvent>(HandlePlayerJoined);
			_eventBus.Subscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
		}

		public void Dispose()
		{
			_eventBus.Unsubscribe<OnPlayerJoinedEvent>(HandlePlayerJoined);
			_eventBus.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
		}

		public void ScheduleMasterOrphanCleanup()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient && !_masterCleanupScheduled)
			{
				_masterCleanupScheduled = true;
				DespawnAllOrphanAvatarsDelayedAsync().Forget();
			}
		}

		public void OnSharedModeMasterMigrationCompleted(NetworkRunner runner)
		{
			if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (PlayerRef item in PlayerNetworkObjectCleanup.CollectDepartedOwners(runner))
			{
				_playerStateLifecycleService.PurgePlayerState(item);
				PlayerNetworkObjectCleanup.ReleaseLocalGrabsOnPlayerObjects(runner, item);
				PlayerNetworkObjectCleanup.AtomicDespawnPlayerObjects(runner, item);
			}
		}

		private void HandlePlayerJoined(OnPlayerJoinedEvent evt)
		{
			if (!(evt.Runner == null) && evt.Runner.IsRunning && evt.Runner.IsSharedModeMasterClient)
			{
				ScheduleMasterOrphanCleanup();
			}
		}

		private void HandlePlayerLeft(OnPlayerLeftEvent evt)
		{
			if (!(evt.Runner == null) && evt.Runner.IsRunning && !(evt.Player == evt.Runner.LocalPlayer))
			{
				PlayerNetworkObjectCleanup.ReleaseLocalGrabsOnPlayerObjects(evt.Runner, evt.Player);
				PlayerNetworkObjectCleanup.DespawnAuthoritativeObjectsOf(evt.Runner, evt.Player);
				if (evt.Runner.IsSharedModeMasterClient)
				{
					AdoptBorrowedAvatars(evt.Runner, evt.Player);
					DespawnLeavingPlayerImmediately(evt.Runner, evt.Player);
					ReclaimAndDespawnLeftoverAvatarAsync(evt.Runner, evt.Player).Forget();
				}
			}
		}

		private async UniTaskVoid ReclaimAndDespawnLeftoverAvatarAsync(NetworkRunner runner, PlayerRef player)
		{
			float deadline = Time.realtimeSinceStartup + 5f;
			while (Time.realtimeSinceStartup < deadline)
			{
				if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient || !PlayerNetworkObjectCleanup.HasPlayerObjects(runner, player))
				{
					return;
				}
				PlayerNetworkObjectCleanup.RequestAuthorityOverPlayerObjects(runner, player);
				if (PlayerNetworkObjectCleanup.HasAuthorityOverPlayerObjects(runner, player))
				{
					break;
				}
				await UniTask.Delay(TimeSpan.FromSeconds(0.10000000149011612));
			}
			if (!(runner == null) && runner.IsRunning && runner.IsSharedModeMasterClient && PlayerNetworkObjectCleanup.HasPlayerObjects(runner, player))
			{
				PlayerNetworkObjectCleanup.ReleaseLocalGrabsOnPlayerObjects(runner, player);
				PlayerNetworkObjectCleanup.DespawnAuthoritativeObjectsOf(runner, player);
			}
		}

		private void AdoptBorrowedAvatars(NetworkRunner runner, PlayerRef leaver)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (allNetworkObject == null || !allNetworkObject.IsValid)
				{
					continue;
				}
				PlayerInitializer component = allNetworkObject.GetComponent<PlayerInitializer>();
				if (component == null)
				{
					continue;
				}
				PlayerRef originalOwner = component.OriginalOwner;
				if (originalOwner == PlayerRef.None || originalOwner == leaver || !runner.ActivePlayers.Any((PlayerRef p) => p == originalOwner))
				{
					continue;
				}
				NetworkObject[] componentsInChildren = allNetworkObject.GetComponentsInChildren<NetworkObject>(includeInactive: true);
				foreach (NetworkObject networkObject in componentsInChildren)
				{
					if (!(networkObject == null) && networkObject.IsValid && networkObject.StateAuthority == leaver)
					{
						networkObject.RequestStateAuthority();
					}
				}
			}
		}

		private void DespawnLeavingPlayerImmediately(NetworkRunner runner, PlayerRef player)
		{
			_playerStateLifecycleService.PurgePlayerState(player);
			PlayerNetworkObjectCleanup.AtomicDespawnPlayerObjects(runner, player);
		}

		private async UniTaskVoid DespawnAllOrphanAvatarsDelayedAsync()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(0.5));
			_masterCleanupScheduled = false;
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!(networkRunner == null) && networkRunner.IsRunning && networkRunner.IsSharedModeMasterClient)
			{
				DespawnAllOrphanAvatars(networkRunner);
			}
		}

		private void DespawnAllOrphanAvatars(NetworkRunner runner)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (TryGetCleanupCandidate(allNetworkObject, out var playerInitializer))
				{
					PlayerRef originalOwner = playerInitializer.OriginalOwner;
					if (IsSafeToDespawn(runner, allNetworkObject, originalOwner) && !(originalOwner == PlayerRef.None) && hashSet.Add(originalOwner.PlayerId))
					{
						_playerStateLifecycleService.PurgePlayerState(originalOwner);
						PlayerNetworkObjectCleanup.AtomicDespawnPlayerObjects(runner, originalOwner);
					}
				}
			}
		}

		private static bool IsSafeToDespawn(NetworkRunner runner, NetworkObject networkObject, PlayerRef originalOwner)
		{
			if (originalOwner == PlayerRef.None)
			{
				return false;
			}
			if (!runner.ActivePlayers.Any((PlayerRef p) => p.PlayerId == originalOwner.PlayerId))
			{
				return true;
			}
			if (networkObject.InputAuthority == originalOwner)
			{
				return false;
			}
			return true;
		}

		private bool TryGetCleanupCandidate(NetworkObject networkObject, out PlayerInitializer playerInitializer)
		{
			playerInitializer = null;
			if (networkObject == null || !networkObject.IsValid)
			{
				return false;
			}
			playerInitializer = networkObject.GetComponent<PlayerInitializer>();
			if (playerInitializer == null)
			{
				return false;
			}
			PlayerRef originalOwner = playerInitializer.OriginalOwner;
			if (originalOwner == PlayerRef.None)
			{
				return false;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			if (!networkRunner.ActivePlayers.Any((PlayerRef p) => p.PlayerId == originalOwner.PlayerId))
			{
				return true;
			}
			return networkObject.InputAuthority != originalOwner;
		}
	}
}
