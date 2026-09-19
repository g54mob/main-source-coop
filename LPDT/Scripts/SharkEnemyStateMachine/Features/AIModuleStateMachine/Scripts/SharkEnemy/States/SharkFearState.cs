using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy.States
{
	public class SharkFearState : StateBase<SharkStateId>
	{
		private readonly SharkEnemy _enemy;

		private readonly SharkEnemyContext _context;

		public SharkFearState(SharkEnemy enemy, SharkEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(SharkVisualState.Fear);
			if (_context.IsDespawnAfterFear)
			{
				_enemy.RequestDespawnNoItemDrop();
				return;
			}
			_context.IsFearing = false;
			_enemy.FearCompleted = true;
		}
	}
}
