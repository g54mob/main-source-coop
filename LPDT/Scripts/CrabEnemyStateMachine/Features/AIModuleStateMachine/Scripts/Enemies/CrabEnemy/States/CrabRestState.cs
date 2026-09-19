using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabRestState : StateBase<CrabStateId>
	{
		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		public CrabRestState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(CrabStateId.Rest);
			_context.SetVisualState(CrabVisualState.Rest);
			_context.CrabDamageAggrSystem.Enable();
			_context.SetMoveSpeed(0f);
			StopAgent();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime >= _context.RestDuration)
			{
				_enemy.TriggerEvent(CrabEvent.OnRestCompleted);
			}
		}

		private void StopAgent()
		{
			NavMeshAgent navMeshAgent = _context.NavMeshAgent;
			if (!(navMeshAgent == null) && navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
			{
				navMeshAgent.ResetPath();
				navMeshAgent.velocity = Vector3.zero;
			}
		}
	}
}
