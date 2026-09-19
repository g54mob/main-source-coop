using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy
{
	public class RatsHoleEnemyHostMigrationRecovery
	{
		private const float IdleDespawnGraceSeconds = 2f;

		private float _blockedUntil = -1f;

		private bool _isRecovering;

		public bool IsRecovering => _isRecovering;

		public void Begin(NetworkRunner runner)
		{
			_blockedUntil = GetCurrentTime(runner) + 2f;
			_isRecovering = true;
		}

		public bool IsIdleDespawnBlocked(NetworkRunner runner)
		{
			if (!_isRecovering)
			{
				return false;
			}
			if (GetCurrentTime(runner) < _blockedUntil)
			{
				return true;
			}
			return false;
		}

		public bool ShouldFallbackToHole(NetworkRunner runner)
		{
			if (!_isRecovering)
			{
				return false;
			}
			if (GetCurrentTime(runner) < _blockedUntil)
			{
				return false;
			}
			_isRecovering = false;
			_blockedUntil = -1f;
			return true;
		}

		public void Complete()
		{
			_isRecovering = false;
			_blockedUntil = -1f;
		}

		private static float GetCurrentTime(NetworkRunner runner)
		{
			if (!(runner != null))
			{
				return Time.time;
			}
			return runner.SimulationTime;
		}
	}
}
