using System;
using System.Collections.Generic;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	public class SessionAnalyticsModel
	{
		private readonly List<int> _enemyTargetList = new List<int>();

		public IReadOnlyList<int> EnemyTargets => _enemyTargetList;

		public bool IstLocationEntered { get; private set; }

		public event Action OnLocationEntered;

		public event Action<int> OnEnemyTargetRegistered;

		public void SetLocationEnteredStatus(bool isEntered)
		{
			if (IstLocationEntered != isEntered)
			{
				IstLocationEntered = isEntered;
				this.OnLocationEntered?.Invoke();
			}
		}

		public void RegisterEnemyTarget(int playerId)
		{
			if (!_enemyTargetList.Contains(playerId))
			{
				_enemyTargetList.Add(playerId);
				this.OnEnemyTargetRegistered?.Invoke(playerId);
			}
		}
	}
}
