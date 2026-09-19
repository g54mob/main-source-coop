using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public interface IEnemyFactory
	{
		UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, LevelType levelType, Vector3 spawnPosition, Vector3 areaPosition, Quaternion spawnRotation);

		UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType, Vector3 spawnPosition, Vector3 areaPosition, Quaternion spawnRotation);
	}
}
