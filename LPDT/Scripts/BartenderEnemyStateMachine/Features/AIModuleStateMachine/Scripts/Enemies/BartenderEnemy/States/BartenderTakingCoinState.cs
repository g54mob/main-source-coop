using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.States
{
	public class BartenderTakingCoinState : StateBase<BartenderStateId>
	{
		private enum Phase
		{
			Reach = 0,
			Return = 1,
			Done = 2
		}

		private readonly BartenderEnemy _enemy;

		private readonly BartenderEnemyContext _context;

		private Phase _phase;

		private bool _consumed;

		public BartenderTakingCoinState(BartenderEnemy enemy, BartenderEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(BartenderStateId.TakingCoin);
			_context.SetIsDancing(value: false);
			_context.SetActiveDanceIndex(-1);
			_context.SetIsReacting(value: false);
			_context.CurrentStateTime = 0f;
			_context.AnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
			_consumed = false;
			_phase = Phase.Reach;
			if (!_enemy.TryBeginHandReachToActiveCoin())
			{
				FinishImmediate();
			}
		}

		public override void OnLogic()
		{
			if (_phase == Phase.Done)
			{
				return;
			}
			_context.CurrentStateTime += _enemy.GetTickDelta();
			if (_phase == Phase.Reach)
			{
				if (_enemy.IsHandReachPhaseFinished())
				{
					if (!_consumed)
					{
						_consumed = true;
						_enemy.ConsumeActiveCoin();
					}
					_enemy.BeginHandReturn();
					_phase = Phase.Return;
				}
			}
			else if (_phase == Phase.Return && _enemy.IsHandReachPhaseFinished())
			{
				_enemy.ClearHandReach();
				_phase = Phase.Done;
				_enemy.TriggerEvent(BartenderEvent.OnTakeCoinFinished);
			}
		}

		public override void OnExit()
		{
			_enemy.ClearHandReach();
		}

		private void FinishImmediate()
		{
			if (!_consumed)
			{
				_consumed = true;
				_enemy.ConsumeActiveCoin();
			}
			_enemy.ClearHandReach();
			_phase = Phase.Done;
			_enemy.TriggerEvent(BartenderEvent.OnTakeCoinFinished);
		}
	}
}
