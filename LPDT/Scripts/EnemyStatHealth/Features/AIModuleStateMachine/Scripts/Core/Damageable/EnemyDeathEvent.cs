using System;
using Features.AIModule.Scripts;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	public class EnemyDeathEvent
	{
		public event Action<int, EnemyType> OnEnemyDeadByPlayer;

		public void InvokeEnemyDeadByPlayer(int playerId, EnemyType enemyType)
		{
			this.OnEnemyDeadByPlayer?.Invoke(playerId, enemyType);
		}
	}
}
