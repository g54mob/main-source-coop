using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Core.Enemy.States
{
	public class SimpleAttackState : StateBase<SimpleEnemyStateId>
	{
		private readonly MinimalEnemy _enemy;

		private readonly EnemyContextBase _context;

		public SimpleAttackState(MinimalEnemy enemy, EnemyContextBase context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SimpleEnemyStateId.Attack);
			_context.RotateTowardsDirectionSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.TargetAttackSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.RotateTowardsDirectionSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.TargetAttackSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				_enemy.TriggerEvent(SimpleEnemyEvent.OnTargetLost);
			}
			else if (Vector3.Distance(_enemy.transform.position, priorityPlayer.NetworkObject.transform.position) > _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(SimpleEnemyEvent.OnOutOfAttackRange);
			}
		}
	}
}
