using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts.Services
{
	public class PlayerTrackingPositionService : IPlayerTrackingPositionService
	{
		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly PlayerRaycastPointsModel _playerRaycastPointsModel;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		public PlayerTrackingPositionService(PlayersRagdollModel playersRagdollModel, PlayerRaycastPointsModel playerRaycastPointsModel, SpawnedPlayersModel spawnedPlayersModel)
		{
			_playersRagdollModel = playersRagdollModel;
			_playerRaycastPointsModel = playerRaycastPointsModel;
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public bool TryGetTrackingPosition(PlayerRef player, out Vector3 position)
		{
			position = Vector3.zero;
			if (player == PlayerRef.None)
			{
				return false;
			}
			int playerId = player.PlayerId;
			if (CheckForRagdoll(ref position, playerId))
			{
				return true;
			}
			if (TryReturnPositionForDefaultState(player, ref position))
			{
				return true;
			}
			return false;
		}

		private bool TryReturnPositionForDefaultState(PlayerRef player, ref Vector3 position)
		{
			if (_playerRaycastPointsModel.TryGetRaycastPoint(player, PlayerRaycastPoint.MiddleBelt, out var point) && point != null)
			{
				position = point.position;
				return true;
			}
			if (_spawnedPlayersModel.Players.TryGetValue(player, out var value) && value != null && value.NetworkObject != null)
			{
				position = value.NetworkObject.transform.position;
				return true;
			}
			return false;
		}

		private bool CheckForRagdoll(ref Vector3 position, int playerId)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll) && ragdoll != null && ragdoll.TrackingPoint != null && ragdoll.IsSimulated)
			{
				position = ragdoll.TrackingPoint.position;
				return true;
			}
			return false;
		}
	}
}
