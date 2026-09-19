using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeBodyStepAggroSystem : NetworkBehaviour
	{
		private const int DefaultPlayerLayer = 7;

		[SerializeField]
		private LayerMask _playerLayers = 128;

		[Tooltip("Debug: skip body-step overlap aggro. Grabbing near the body often hits this path too.")]
		[SerializeField]
		private bool _debugDisableStepAggro;

		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		[Inject]
		public void InjectDependencies(SnakeEnemy enemy, SnakeEnemyContext context, SpawnedPlayersModel spawnedPlayersModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_enemy = enemy;
			_context = context;
			_spawnedPlayersModel = spawnedPlayersModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public void OnPlayerLayerEntered(Collider other)
		{
			if (!_debugDisableStepAggro && base.HasStateAuthority && !(_enemy == null) && !(_context == null) && !(other == null) && _enemy.CurrentStateId != SnakeStateId.Fear && _enemy.CurrentStateId != SnakeStateId.SafeZoneApproach && !_context.IsWrapEscapeCooldownActive && (_playerLayers.value & (1 << other.gameObject.layer)) != 0 && TryResolvePlayer(other, out var player) && !_context.IsPlayerDataInSafeZone(player))
			{
				int playerId = player.NetworkObject.InputAuthority.PlayerId;
				if (_enemyPlayerAttackabilityService.CanEnemyTargetPlayer(playerId) && _context.CanTargetPlayerForWrap(playerId) && !_context.IsPlayerOutsideGate(playerId))
				{
					_context.SetPriorityPlayer(player);
					_context.ApplyStepAggroMoveSpeed();
					_enemy.TriggerEvent(SnakeEvent.OnSteppedOn);
				}
			}
		}

		private bool TryResolvePlayer(Collider other, out PlayerDataHolder player)
		{
			player = null;
			if (_spawnedPlayersModel == null)
			{
				return false;
			}
			NetworkObject componentInParent = other.GetComponentInParent<NetworkObject>();
			if (componentInParent == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				PlayerDataHolder value = player2.Value;
				if (value != null && !(value.NetworkObject == null) && !(value.NetworkObject != componentInParent))
				{
					player = value;
					return true;
				}
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
