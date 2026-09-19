using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.AIModuleStateMachine.Scripts.Data
{
	public class EnemyHeadwearModel : ISessionCleanup
	{
		private readonly Dictionary<uint, IEnemyHeadwear> _worn = new Dictionary<uint, IEnemyHeadwear>();

		private readonly Dictionary<uint, int> _pendingRemovers = new Dictionary<uint, int>();

		public event Action<uint, bool> OnEnemyHeadwearChanged;

		public bool IsWearing(uint enemyObjectId)
		{
			return _worn.ContainsKey(enemyObjectId);
		}

		public bool TryGet(uint enemyObjectId, out IEnemyHeadwear headwear)
		{
			return _worn.TryGetValue(enemyObjectId, out headwear);
		}

		public void Set(uint enemyObjectId, bool isWearing, IEnemyHeadwear headwear = null)
		{
			if (isWearing)
			{
				_worn[enemyObjectId] = headwear;
			}
			else
			{
				if (headwear != null && _worn.TryGetValue(enemyObjectId, out var value) && value != headwear)
				{
					return;
				}
				_worn.Remove(enemyObjectId);
			}
			this.OnEnemyHeadwearChanged?.Invoke(enemyObjectId, isWearing);
		}

		public void SetRemovedByPlayer(uint enemyObjectId, int playerId)
		{
			_pendingRemovers[enemyObjectId] = playerId;
		}

		public bool TryConsumeRemover(uint enemyObjectId, out int playerId)
		{
			if (_pendingRemovers.TryGetValue(enemyObjectId, out playerId))
			{
				_pendingRemovers.Remove(enemyObjectId);
				return true;
			}
			playerId = -1;
			return false;
		}

		public void Cleanup()
		{
			_worn.Clear();
			_pendingRemovers.Clear();
		}
	}
}
