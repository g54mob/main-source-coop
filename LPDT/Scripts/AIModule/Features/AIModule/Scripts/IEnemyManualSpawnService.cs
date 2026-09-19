using Cysharp.Threading.Tasks;

namespace Features.AIModule.Scripts
{
	public interface IEnemyManualSpawnService
	{
		UniTask<IEnemyBehaviour> SpawnEnemy(EnemyType enemyType);

		UniTask<IEnemyBehaviour> SpawnEnemyNearPlayer(EnemyType enemyType);
	}
}
