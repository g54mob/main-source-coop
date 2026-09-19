using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class PorterBlockDetector
	{
		private const float STALL_SPEED = 0.15f;

		private const float WANTS_TO_MOVE_SPEED = 0.1f;

		private readonly MonkeyPorterContext _context;

		private readonly MonkeyPorterSettings _settings;

		private Vector3 _lastPosition;

		private bool _hasLastPosition;

		private float _stalledElapsed;

		private float _recheckElapsed;

		private bool _isGoalUnreachable;

		public PorterBlockDetector(MonkeyPorterContext context, MonkeyPorterSettings settings)
		{
			_context = context;
			_settings = settings;
		}

		public void Reset()
		{
			_hasLastPosition = false;
			_stalledElapsed = 0f;
			_recheckElapsed = _settings.BlockedRecheckInterval;
			_isGoalUnreachable = false;
		}

		public bool IsStalled(float deltaTime)
		{
			return IsStalled(deltaTime, _settings.BlockedDetectDelay);
		}

		public bool IsStalled(float deltaTime, float stallTime)
		{
			Vector3 position = _context.transform.position;
			if (!_hasLastPosition)
			{
				_hasLastPosition = true;
				_lastPosition = position;
				return false;
			}
			float num = Vector3.Distance(position, _lastPosition);
			_lastPosition = position;
			if (deltaTime <= 0f)
			{
				return false;
			}
			bool num2 = _context.AgentDesiredSpeed > 0.1f;
			bool flag = num / deltaTime < 0.15f;
			if (num2 && flag)
			{
				_stalledElapsed += deltaTime;
			}
			else
			{
				_stalledElapsed = 0f;
			}
			return _stalledElapsed >= stallTime;
		}

		public bool IsGoalUnreachable(Vector3 goal, float deltaTime)
		{
			_recheckElapsed -= deltaTime;
			if (_recheckElapsed > 0f)
			{
				return _isGoalUnreachable;
			}
			_recheckElapsed = _settings.BlockedRecheckInterval;
			_isGoalUnreachable = !_context.IsGoalReachable(goal);
			return _isGoalUnreachable;
		}
	}
}
