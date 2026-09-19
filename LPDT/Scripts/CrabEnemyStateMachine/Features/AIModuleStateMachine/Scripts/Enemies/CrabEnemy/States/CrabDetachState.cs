using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabDetachState : StateBase<CrabStateId>
	{
		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		private bool _completed;

		private CrabClawSide _detachClaw;

		public CrabDetachState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(CrabStateId.Detach);
			_completed = false;
			_context.ClearDetachCompleted();
			_context.SetMoveSpeed(0f);
			StopAgent();
			_detachClaw = _context.ActiveClaw;
			CrabVisualState visualState = ((_detachClaw == CrabClawSide.Left) ? CrabVisualState.DetachingLeft : CrabVisualState.DetachingRight);
			_context.SetVisualState(visualState);
			_context.StateDurationTimeSystem.Enable();
			_context.CrabClawDetachLootSystem.Enable();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
			_context.CrabClawDetachLootSystem.Disable();
			_context.PlayerGrabSystem.Disable();
			_context.PlayerGrabSystem.ClearActiveClaw();
			_context.ClearDetachCompleted();
			_detachClaw = CrabClawSide.None;
		}

		public override void OnLogic()
		{
			if (!_completed)
			{
				bool detachCompletedRequested = _context.DetachCompletedRequested;
				bool flag = _context.CurrentStateTime >= _context.DetachDuration;
				if (detachCompletedRequested || flag)
				{
					_context.ClearDetachCompleted();
					_completed = true;
					_enemy.TriggerEvent(CrabEvent.OnDetachCompleted);
				}
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
