using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperSleepState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		public SleeperSleepState(SleeperEnemy enemy, SleeperEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Sleep);
			_context.SetVisualState(SleeperVisualState.Sleep);
			if (_context.NavMeshAgent.isOnNavMesh)
			{
				_context.NavMeshAgent.ResetPath();
			}
			_context.NavMeshAgent.velocity = Vector3.zero;
			_context.AttackCooldown = 0f;
			_context.IsAttackOnCooldown = false;
			_context.SleeperSoundOcclusionHearingStrengthSetupSystem.Enable();
			_context.SoundOcclusionMonoSystem.Enable();
			_context.SleeperSoundAggroSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
		}

		public override void OnExit()
		{
			_context.SleeperSoundOcclusionHearingStrengthSetupSystem.Disable();
			_context.SoundOcclusionMonoSystem.Disable();
			_context.SleeperSoundAggroSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
		}
	}
}
