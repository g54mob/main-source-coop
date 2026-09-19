using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts.RoomVariations;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts
{
	public class TeleportationPointsSystem : IInitializable, IDisposable, ISpawnPointTeleport
	{
		private readonly TeleportationPointsEventClass _teleportationPointsEventClass;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		private CancellationTokenSource _teleportCancellation = new CancellationTokenSource();

		public TeleportationPointsSystem(TeleportationPointsEventClass teleportationPointsEventClass, MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel, SpawnedRoomsModel spawnedRoomsModel)
		{
			_teleportationPointsEventClass = teleportationPointsEventClass;
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_spawnedRoomsModel = spawnedRoomsModel;
		}

		public void Initialize()
		{
			_teleportationPointsEventClass.OnBeachTeleportationRequested += TeleportOnBeach;
			_teleportationPointsEventClass.OnCancelPendingTeleportsRequested += RenewTeleportCancellation;
		}

		public void Dispose()
		{
			_playerMovableModel.AllCharacterMovablesPlayerAdded -= TeleportPlayerWhenInitialized;
			_teleportationPointsEventClass.OnBeachTeleportationRequested -= TeleportOnBeach;
			_teleportationPointsEventClass.OnCancelPendingTeleportsRequested -= RenewTeleportCancellation;
			CancelPendingTeleports();
		}

		public void TeleportToBeach()
		{
			List<TeleportationPoint> beachTeleportationPoints = _teleportationPointsEventClass.BeachTeleportationPoints;
			if (beachTeleportationPoints == null || beachTeleportationPoints.Count == 0)
			{
				Debug.LogError("[Teleport] TeleportToBeach: the active level registered no beach teleportation points — the level is missing its BeachTeleportationPoints; players stay where they spawned.");
			}
			else
			{
				TeleportPlayer(beachTeleportationPoints);
			}
		}

		public void TeleportLocalTo(Vector3 position, float yaw)
		{
			PlayerCharacterMovableBase localMovable = _playerMovableModel.LocalMovable;
			if (!(localMovable == null))
			{
				Quaternion value = Quaternion.Euler(0f, yaw, 0f);
				localMovable.SetLinearVelocity(Vector3.zero);
				localMovable.ChangePosition(position, value, isForced: true);
				localMovable.SetLinearVelocity(Vector3.zero);
			}
		}

		private void RenewTeleportCancellation()
		{
			CancelPendingTeleports();
			_teleportCancellation = new CancellationTokenSource();
		}

		private void CancelPendingTeleports()
		{
			if (_teleportCancellation != null)
			{
				_teleportCancellation.Cancel();
				_teleportCancellation.Dispose();
				_teleportCancellation = null;
			}
		}

		private void TeleportOnBeach(List<TeleportationPoint> teleportationPoints)
		{
			TeleportPlayer(teleportationPoints);
		}

		private async void TeleportPlayer(List<TeleportationPoint> teleportationPoints, bool awaitStateAuthority = false)
		{
			Vector3 firstPoint = ((teleportationPoints.Count > 0) ? teleportationPoints[0].Position : Vector3.zero);
			CancellationToken cancellationToken = _teleportCancellation?.Token ?? CancellationToken.None;
			await UniTask.WaitUntil(() => !PlayerSpawnLock.ShouldBlockPositionOverride() && _spawnedRoomsModel.IsAllTasksCompleted, PlayerLoopTiming.Update, cancellationToken).TimeoutWithoutException(TimeSpan.FromSeconds(15.0)).SuppressCancellationThrow();
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
			if (PlayerSpawnLock.ShouldBlockPositionOverride())
			{
				Debug.LogError($"[SpawnDiag] Beach teleport FAILED — player stranded at initial spawn (\"roof\"). Cause: spawn-lock still held after 15s wait, atomic spawn never consumed. firstSpawnInProgress={PlayerSpawnLock.IsFirstSpawnInProgress} reconnectSpawn={PlayerSpawnLock.IsReconnectSpawn} suppressed={PlayerSpawnLock.IsLevelSpawnTeleportSuppressed} allRoomsReady={_spawnedRoomsModel.IsAllTasksCompleted} target={firstPoint} frame={Time.frameCount}");
				return;
			}
			List<PlayerRef> playerRefs = _multiplayerModel.NetworkRunner.ActivePlayers.OrderBy((PlayerRef p) => p.PlayerId).ToList();
			for (int index = 0; index < playerRefs.Count; index++)
			{
				PlayerRef playerRef = playerRefs[index];
				int pointIndex = ResolveTeleportPointIndex(index, teleportationPoints.Count);
				if (pointIndex < 0 || ShouldSkipDefaultLevelSpawnTeleport(playerRef))
				{
					continue;
				}
				if (_playerMovableModel.AllCharacterMovables.ContainsKey(playerRef))
				{
					PlayerCharacterMovableBase playerCharacterMovableBase = _playerMovableModel.AllCharacterMovables[playerRef];
					if (awaitStateAuthority && playerCharacterMovableBase.Object.HasInputAuthority)
					{
						await UniTask.WaitUntil(() => playerCharacterMovableBase.Object.HasStateAuthority, PlayerLoopTiming.Update, cancellationToken).TimeoutWithoutException(TimeSpan.FromSeconds(5.0)).SuppressCancellationThrow();
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}
					}
					if (awaitStateAuthority ? playerCharacterMovableBase.Object.HasInputAuthority : playerCharacterMovableBase.Object.HasStateAuthority)
					{
						TeleportCharacterToPoint(playerCharacterMovableBase, teleportationPoints[pointIndex]);
					}
				}
				else
				{
					_playerMovableModel.AllCharacterMovablesPlayerAdded += TeleportPlayerWhenInitialized;
				}
			}
		}

		private void TeleportPlayerWhenInitialized(PlayerRef initializedPlayer)
		{
			if (!(_multiplayerModel.NetworkRunner.LocalPlayer != initializedPlayer))
			{
				TeleportInitializedPlayerWhenReadyAsync(initializedPlayer).Forget();
			}
		}

		private async UniTaskVoid TeleportInitializedPlayerWhenReadyAsync(PlayerRef initializedPlayer)
		{
			CancellationToken cancellationToken = _teleportCancellation?.Token ?? CancellationToken.None;
			await UniTask.WaitUntil(() => !PlayerSpawnLock.IsFirstSpawnInProgress && !PlayerSpawnLock.ShouldBlockPositionOverride(), PlayerLoopTiming.Update, cancellationToken).TimeoutWithoutException(TimeSpan.FromSeconds(15.0)).SuppressCancellationThrow();
			if (cancellationToken.IsCancellationRequested || _multiplayerModel.NetworkRunner.LocalPlayer != initializedPlayer)
			{
				return;
			}
			_playerMovableModel.AllCharacterMovablesPlayerAdded -= TeleportPlayerWhenInitialized;
			if (ShouldSkipDefaultLevelSpawnTeleport(initializedPlayer))
			{
				return;
			}
			List<TeleportationPoint> beachTeleportationPoints = _teleportationPointsEventClass.BeachTeleportationPoints;
			if (beachTeleportationPoints == null || beachTeleportationPoints.Count == 0)
			{
				return;
			}
			List<PlayerRef> list = _multiplayerModel.NetworkRunner.ActivePlayers.OrderBy((PlayerRef p) => p.PlayerId).ToList();
			for (int num = 0; num < list.Count; num++)
			{
				PlayerRef playerRef = list[num];
				if (!(initializedPlayer != playerRef))
				{
					int num2 = ResolveTeleportPointIndex(num, beachTeleportationPoints.Count);
					if (num2 >= 0 && _playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value))
					{
						TeleportCharacterToPoint(value, beachTeleportationPoints[num2]);
					}
					break;
				}
			}
		}

		private void TeleportCharacterToPoint(PlayerCharacterMovableBase playerCharacterMovableBase, TeleportationPoint teleportationPoint)
		{
			playerCharacterMovableBase.SetLinearVelocity(Vector3.zero);
			playerCharacterMovableBase.ChangePosition(teleportationPoint.Position, teleportationPoint.Rotation, isForced: true);
			playerCharacterMovableBase.SetLinearVelocity(Vector3.zero);
			_teleportationPointsEventClass.InvokeOnPlayerTeleported();
		}

		private static int ResolveTeleportPointIndex(int playerIndex, int pointCount)
		{
			if (pointCount <= 0)
			{
				return -1;
			}
			return playerIndex % pointCount;
		}

		private bool ShouldSkipDefaultLevelSpawnTeleport(PlayerRef playerRef)
		{
			if (playerRef != _multiplayerModel.NetworkRunner.LocalPlayer)
			{
				return false;
			}
			if (PlayerSpawnLock.IsLevelSpawnTeleportSuppressed)
			{
				return true;
			}
			if (PlayerSpawnLock.IsFirstSpawnInProgress)
			{
				return PlayerSpawnLock.IsReconnectSpawn;
			}
			return false;
		}
	}
}
