using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.AIModuleStateMachine.Scripts.AliveEnemyCount
{
	public class AliveEnemyCountModel : ISessionCleanup
	{
		private readonly Dictionary<EnemyType, int> _counts = new Dictionary<EnemyType, int>();

		public void Register(EnemyType enemyType)
		{
			if (!_counts.TryGetValue(enemyType, out var value))
			{
				_counts[enemyType] = 1;
			}
			else
			{
				_counts[enemyType] = value + 1;
			}
		}

		public void Unregister(EnemyType enemyType)
		{
			if (_counts.TryGetValue(enemyType, out var value))
			{
				value--;
				if (value <= 0)
				{
					_counts.Remove(enemyType);
				}
				else
				{
					_counts[enemyType] = value;
				}
			}
		}

		public int GetCount(EnemyType enemyType)
		{
			if (!_counts.TryGetValue(enemyType, out var value))
			{
				return 0;
			}
			return value;
		}

		public void Cleanup()
		{
			_counts.Clear();
		}
	}
}
