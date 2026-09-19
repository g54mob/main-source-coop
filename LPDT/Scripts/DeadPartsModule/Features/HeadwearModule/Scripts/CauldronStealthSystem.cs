using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class CauldronStealthSystem : ITickable
	{
		private const float MAX_STEALTH_SPEED = 0.2f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly HeadwearModel _headwearModel;

		private readonly CauldronStealthModel _cauldronStealthModel;

		public CauldronStealthSystem(MultiplayerModel multiplayerModel, SpawnedPlayersModel spawnedPlayersModel, HeadwearModel headwearModel, CauldronStealthModel cauldronStealthModel)
		{
			_multiplayerModel = multiplayerModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_headwearModel = headwearModel;
			_cauldronStealthModel = cauldronStealthModel;
		}

		public void Tick()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (var (playerRef2, playerDataHolder2) in _spawnedPlayersModel.Players)
			{
				_cauldronStealthModel.SetStealthed(playerRef2.PlayerId, IsStealthed(playerRef2.PlayerId, playerDataHolder2));
			}
		}

		private bool IsStealthed(int playerId, PlayerDataHolder playerDataHolder)
		{
			if (!_headwearModel.TryGetHeadwear(playerId, out var headwear) || headwear == null || !headwear.IsWorn)
			{
				return false;
			}
			NetworkObject networkObject = playerDataHolder.NetworkObject;
			if (networkObject == null || !networkObject.IsValid || !networkObject.TryGetComponent<CharacterMovableBase>(out var component))
			{
				return false;
			}
			if (component.MovementState != MovementState.Crouching)
			{
				return false;
			}
			Vector3 velocity = component.GetVelocity();
			velocity.y = 0f;
			return velocity.magnitude <= 0.2f;
		}
	}
}
