using Features.DeadPartsModule.Scripts;
using PlayerCustomization;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class PlayerTutorialGuideDeadState : StateBase<PlayerTutorialGuideStateId>
	{
		private readonly PlayerTutorialGuideEnemy _enemy;

		private readonly PlayerTutorialGuideEnemyContext _context;

		private readonly IPlayerDeadPartSpawnService _deadPartSpawnService;

		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private float _deadTimer;

		public PlayerTutorialGuideDeadState(PlayerTutorialGuideEnemy enemy, PlayerTutorialGuideEnemyContext context, IPlayerDeadPartSpawnService deadPartSpawnService, PlayerDeadPartModel playerDeadPartModel)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_deadPartSpawnService = deadPartSpawnService;
			_playerDeadPartModel = playerDeadPartModel;
		}

		public override void OnEnter()
		{
			_context.UpdateBottomPartVisibility(enabledVis: false);
			_enemy.SetCurrentStateId(PlayerTutorialGuideStateId.Dead);
			_context.SetIsDead(value: true);
			if (_context.CarryItemGrabbable != null)
			{
				_context.CarryItemTransformReplicator.StopReplication();
				_context.CarryItemGrabbable.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				_context.CarryItemGrabbable.Rigidbody.isKinematic = false;
				_context.CarryItemGrabbable.ChangeRigidbodyKinematic = true;
				_context.HasCarryItem = false;
			}
			if (_context.NavMeshAgent != null && _context.NavMeshAgent.isOnNavMesh)
			{
				_context.NavMeshAgent.ResetPath();
			}
			SpawnDeadPart();
		}

		public override void OnLogic()
		{
			_deadTimer += _enemy.GetTickDelta();
			if (!(_deadTimer < _context.ReviveMinTime) && _context.CanBeRevived && TryFindConnectingDeadPart(out var connectingDeadPart))
			{
				Revive(connectingDeadPart);
			}
		}

		private bool TryFindConnectingDeadPart(out PlayerDeadPart connectingDeadPart)
		{
			connectingDeadPart = null;
			Vector3 b = ((_context.DownPos != null) ? _context.DownPos.position : _enemy.transform.position);
			foreach (PlayerDeadPart spawnedDeadPart in _playerDeadPartModel.SpawnedDeadParts)
			{
				if (!(Vector3.Distance(spawnedDeadPart.UpperPos.position, b) > _context.ReviveConnectDistance))
				{
					Vector3 vector = _context.DownPos.position - _context.UpperPos.position;
					Vector3 to = spawnedDeadPart.UpperPos.position - _context.UpperPos.position;
					if (Vector3.Angle(vector, to) <= _context.ReviveConnectAngle)
					{
						connectingDeadPart = spawnedDeadPart;
						return true;
					}
				}
			}
			return false;
		}

		private void Revive(PlayerDeadPart connectingDeadPart)
		{
			connectingDeadPart.Use();
			_context.SetBottomPartUsageCount(connectingDeadPart.UsageCount);
			connectingDeadPart.DespawnPart();
			if (_context.HealthController != null)
			{
				_context.HealthController.Revive();
			}
			_context.SetIsDead(value: false);
			_enemy.TriggerEvent(PlayerTutorialGuideEvent.OnRevived);
		}

		private async void SpawnDeadPart()
		{
			Vector3 position = ((_context.DownPos != null) ? _context.DownPos.position : _enemy.transform.position);
			PlayerCustomizationSlotData currentAppearanceSlotData = _context.ReplicateTargetSystem.CurrentAppearanceSlotData;
			PlayerTutorialGuideEnemyContext context = _context;
			context.LastSpawnedDeadPart = await _deadPartSpawnService.SpawnPlayerDeadPart(_context.DeadPartType, position, 1, currentAppearanceSlotData, addForce: true);
		}
	}
}
