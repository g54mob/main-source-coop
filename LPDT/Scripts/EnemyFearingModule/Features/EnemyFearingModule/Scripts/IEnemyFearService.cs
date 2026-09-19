using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;

namespace Features.EnemyFearingModule.Scripts
{
	public interface IEnemyFearService
	{
		UniTask FearEnemy(EnemyType enemy);

		UniTask FearEnemy(int enemyInstance);

		UniTask FearEnemies(List<EnemyType> enemies);

		UniTask<List<EnemyTypeWithTransform>> FearAllEnemy(List<EnemyType> except = null);

		void CancelFear();
	}
}
