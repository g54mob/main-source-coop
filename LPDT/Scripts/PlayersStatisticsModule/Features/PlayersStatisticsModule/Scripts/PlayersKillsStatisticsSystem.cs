using System;
using Features.AIModule.Scripts;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class PlayersKillsStatisticsSystem : IInitializable, IDisposable
	{
		private readonly EnemyDeathEvent _enemyDeathEvent;

		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		public PlayersKillsStatisticsSystem(EnemyDeathEvent enemyDeathEvent, LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel)
		{
			_enemyDeathEvent = enemyDeathEvent;
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
		}

		public void Initialize()
		{
			_enemyDeathEvent.OnEnemyDeadByPlayer += ProcessEnemyDeathByPlayer;
		}

		public void Dispose()
		{
			_enemyDeathEvent.OnEnemyDeadByPlayer -= ProcessEnemyDeathByPlayer;
		}

		private void ProcessEnemyDeathByPlayer(int playerId, EnemyType _)
		{
			_levelPlayersGameStatisticsModel.AddKill(playerId);
		}
	}
}
