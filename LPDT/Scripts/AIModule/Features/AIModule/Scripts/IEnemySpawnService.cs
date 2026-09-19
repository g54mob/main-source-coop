using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public interface IEnemySpawnService
	{
		UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, LevelType levelType);

		UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType);

		bool TryGetEnemyRandomSpawnPosition(EnemyType enemyType, out Vector3 spawnPosition);

		UniTask<IEnemyBehaviour> SpawnEnemyAtSpawnPosition(EnemyType enemyType, LevelType levelType, EnemySpawnPointData spawnPosition);
	}
}
