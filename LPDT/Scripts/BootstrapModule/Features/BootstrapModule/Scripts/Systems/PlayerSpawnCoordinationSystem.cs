using System;
using Cysharp.Threading.Tasks;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class PlayerSpawnCoordinationSystem : IPlayerSpawnCoordinationService, IInitializable, IDisposable
	{
		private const float SpawnClearanceTimeoutSeconds = 15f;

		private const float MasterSpawnTeleportTimeoutSeconds = 10f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPlayerStateLifecycleService _playerStateLifecycleService;

		private readonly PlayerSpawnRequestNetworkEvent _playerSpawnRequestNetworkEvent;

		private readonly PlayerSpawnReadyNetworkEvent _playerSpawnReadyNetworkEvent;

		public PlayerSpawnCoordinationSystem(MultiplayerModel multiplayerModel, IPlayerStateLifecycleService playerStateLifecycleService, PlayerSpawnRequestNetworkEvent playerSpawnRequestNetworkEvent, PlayerSpawnReadyNetworkEvent playerSpawnReadyNetworkEvent)
		{
			_multiplayerModel = multiplayerModel;
			_playerStateLifecycleService = playerStateLifecycleService;
			_playerSpawnRequestNetworkEvent = playerSpawnRequestNetworkEvent;
			_playerSpawnReadyNetworkEvent = playerSpawnReadyNetworkEvent;
		}

		public void Initialize()
		{
			_playerSpawnRequestNetworkEvent.OnNetworkEventSend += HandleSpawnRequest;
		}

		public void Dispose()
		{
			_playerSpawnRequestNetworkEvent.OnNetworkEventSend -= HandleSpawnRequest;
		}

		public async UniTask WaitForSpawnClearanceAsync(PlayerRef localPlayer, Vector3 spawnPosition, bool hasReconnectSpawnPosition = false)
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (runner == null || !runner.IsRunning || localPlayer == PlayerRef.None)
			{
				return;
			}
			_playerStateLifecycleService.PurgePlayerState(localPlayer);
			if (runner.IsSharedModeMasterClient)
			{
				ClearPlayerObjectsForSpawn(runner, localPlayer);
				return;
			}
			await UniTask.WaitUntil(() => runner != null && runner.IsRunning && runner.LocalPlayer != PlayerRef.None);
			await UniTask.Yield(PlayerLoopTiming.Update);
			if (!(await TryWaitForSpawnReadyAsync(runner, localPlayer, spawnPosition, hasReconnectSpawnPosition)))
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				await TryWaitForSpawnReadyAsync(runner, localPlayer, spawnPosition, hasReconnectSpawnPosition);
				_playerStateLifecycleService.PurgePlayerState(localPlayer);
			}
		}

		private async UniTask<bool> TryWaitForSpawnReadyAsync(NetworkRunner runner, PlayerRef localPlayer, Vector3 spawnPosition, bool hasReconnectSpawnPosition)
		{
			UniTaskCompletionSource readySource = new UniTaskCompletionSource();
			_playerSpawnReadyNetworkEvent.OnNetworkEventSend += OnSpawnReady;
			if (hasReconnectSpawnPosition)
			{
				_playerSpawnRequestNetworkEvent.SendEvent(localPlayer.PlayerId, spawnPosition);
			}
			else
			{
				_playerSpawnRequestNetworkEvent.SendEvent(localPlayer.PlayerId);
			}
			try
			{
				await readySource.Task.Timeout(TimeSpan.FromSeconds(15.0));
				return true;
			}
			catch (TimeoutException)
			{
				return false;
			}
			finally
			{
				_playerSpawnReadyNetworkEvent.OnNetworkEventSend -= OnSpawnReady;
			}
			void OnSpawnReady(PlayerSpawnReadyNetworkEvent evt)
			{
				if (evt.ReadyPlayerId == localPlayer.PlayerId)
				{
					readySource.TrySetResult();
				}
			}
		}

		private void HandleSpawnRequest(PlayerSpawnRequestNetworkEvent evt)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			PlayerRef playerRef = ResolvePlayerRef(networkRunner, evt.RequestingPlayerId);
			if (!(playerRef == PlayerRef.None))
			{
				ClearPlayerObjectsForSpawn(networkRunner, playerRef);
				_playerSpawnReadyNetworkEvent.SendEvent(evt.RequestingPlayerId);
				if (evt.HasReconnectSpawnPosition)
				{
					ApplyMasterSpawnPositionAfterSpawnAsync(networkRunner, playerRef, evt.ReconnectSpawnPosition).Forget();
				}
			}
		}

		private async UniTaskVoid ApplyMasterSpawnPositionAfterSpawnAsync(NetworkRunner runner, PlayerRef playerRef, Vector3 position)
		{
			float elapsed = 0f;
			while (elapsed < 10f && !(runner == null) && runner.IsRunning && !(runner != _multiplayerModel.NetworkRunner))
			{
				foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
				{
					if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.InputAuthority != playerRef))
					{
						if (PlayerSpawnLock.IsFirstSpawnInProgress)
						{
							return;
						}
						PlayerCharacterMovableBase component = allNetworkObject.GetComponent<PlayerCharacterMovableBase>();
						if (!(component == null) && component.Object.HasStateAuthority)
						{
							component.ChangePosition(position, null, isForced: true);
							return;
						}
					}
				}
				elapsed += Time.deltaTime;
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
		}

		private void ClearPlayerObjectsForSpawn(NetworkRunner runner, PlayerRef playerRef)
		{
			_playerStateLifecycleService.PurgePlayerState(playerRef);
			PlayerNetworkObjectCleanup.AtomicDespawnPlayerObjects(runner, playerRef);
		}

		private static PlayerRef ResolvePlayerRef(NetworkRunner runner, int playerId)
		{
			foreach (PlayerRef activePlayer in runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					return activePlayer;
				}
			}
			return PlayerRef.None;
		}
	}
}
