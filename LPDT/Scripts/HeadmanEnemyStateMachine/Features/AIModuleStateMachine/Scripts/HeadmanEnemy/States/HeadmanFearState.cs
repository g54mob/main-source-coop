using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanFearState : StateBase<HeadmanStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanFearSettings _settings;

		public HeadmanFearState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanFearSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Fear);
			_context.ClearTarget();
			_context.RestoreDefaultAreaMask();
			_context.FearDestinationTimer = _settings.FearDestinationUpdateInterval;
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.FearDestinationTimer += tickDelta;
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.FearSpeed;
			}
			if (!_context.IsEnemyVisibleByPlayers())
			{
				if (_context.IsDespawnAfterFear)
				{
					_enemy.RequestDespawnAfterFear();
					return;
				}
				_context.IsFearing = false;
				_enemy.FearCompleted = true;
			}
			else if (!(_context.FearDestinationTimer < _settings.FearDestinationUpdateInterval) && (!(_context.Agent != null) || !(Vector3.Distance(_context.Agent.pathEndPosition, _context.transform.position) > _settings.FearDestinationReachedDistance)))
			{
				_context.FearDestinationTimer = 0f;
				_context.MoveToPosition(_context.GetRandomNavmeshPosition(_settings.FearRunDistance));
			}
		}
	}
}
