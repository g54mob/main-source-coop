using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.States
{
	public class SirenStunState : StateBase<SirenStateId>
	{
		private readonly SirenEnemy _enemy;

		private readonly SirenEnemyContext _context;

		public SirenStunState(SirenEnemy enemy, SirenEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SirenVisualState.Stun);
			_enemy.SetLookAtPresentationActive(active: false);
			_enemy.RaiseScreamSound(play: false);
			_enemy.RaiseSongSound(play: false);
			_enemy.RaiseHitSound(play: true);
			ReleaseChaseTarget();
		}

		private void ReleaseChaseTarget()
		{
			if (_context.HasTarget)
			{
				int playerId = _context.TargetPlayer.PlayerId;
				_enemy.RaiseResetPlayerRotation(playerId);
				_enemy.RaiseExitFollowMode(playerId);
			}
			_context.TargetPlayer = PlayerRef.None;
			_context.ChaseElapsed = 0f;
			_context.LostTargetElapsed = 0f;
			_context.IsLosingTarget = false;
			_enemy.SetCurrentTargetPlayerId(-1);
		}
	}
}
