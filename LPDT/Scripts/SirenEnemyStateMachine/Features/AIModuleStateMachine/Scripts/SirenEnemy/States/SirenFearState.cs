using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.States
{
	public class SirenFearState : StateBase<SirenStateId>
	{
		private readonly SirenEnemy _enemy;

		private readonly SirenEnemyContext _context;

		public SirenFearState(SirenEnemy enemy, SirenEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SirenVisualState.Fear);
			_enemy.SetLookAtPresentationActive(active: false);
			_enemy.RaiseScreamSound(play: false);
			_enemy.RaiseSongSound(play: false);
			_enemy.RaiseHitSound(play: false);
			if (_context.IsDespawnAfterFear)
			{
				_enemy.RequestDespawnAfterFear();
				return;
			}
			_context.IsFearing = false;
			_enemy.MarkFearCompleted();
		}
	}
}
