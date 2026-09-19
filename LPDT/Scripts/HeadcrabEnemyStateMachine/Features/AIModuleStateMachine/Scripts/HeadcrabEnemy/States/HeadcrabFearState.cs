using Features.AIModuleStateMachine.Scripts.Data;
using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.States
{
	public class HeadcrabFearState : StateBase<HeadcrabStateId>
	{
		private readonly HeadcrabEnemy _enemy;

		private readonly HeadcrabEnemyContext _context;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		public HeadcrabFearState(HeadcrabEnemy enemy, HeadcrabEnemyContext context, BusyByHeadCrabPlayers busyPlayers)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_busyPlayers = busyPlayers;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadcrabVisualState.Fear);
			if (_context.IsDespawnAfterFear)
			{
				_enemy.RequestDespawnNoItemDrop();
				return;
			}
			if (_context.TargetPlayer != PlayerRef.None && _busyPlayers != null)
			{
				_busyPlayers.BusyPlayers.Remove(_context.TargetPlayer);
			}
			_context.IsFearing = false;
			_enemy.FearCompleted = true;
		}
	}
}
