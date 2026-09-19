using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.AudioServiceModule.Scripts;

namespace Features.EnemiesAppearSoundModule.Scripts
{
	public class EnemiesAppearSoundTransformsModel
	{
		private readonly Dictionary<EnemyType, List<ITransformBasedSoundSource>> _enemies = new Dictionary<EnemyType, List<ITransformBasedSoundSource>>();

		public IReadOnlyDictionary<EnemyType, List<ITransformBasedSoundSource>> Enemies => _enemies;

		public void AddEnemy(EnemyType enemyType, ITransformBasedSoundSource lookTarget)
		{
			if (enemyType != EnemyType.None && lookTarget != null)
			{
				if (!_enemies.ContainsKey(enemyType))
				{
					_enemies.Add(enemyType, new List<ITransformBasedSoundSource>());
				}
				if (!_enemies[enemyType].Contains(lookTarget))
				{
					_enemies[enemyType].Add(lookTarget);
				}
			}
		}

		public void RemoveEnemy(EnemyType enemyType, ITransformBasedSoundSource lookTarget)
		{
			if (enemyType != EnemyType.None && lookTarget != null && _enemies.ContainsKey(enemyType))
			{
				_enemies[enemyType].Remove(lookTarget);
				if (_enemies[enemyType].Count == 0)
				{
					_enemies.Remove(enemyType);
				}
			}
		}
	}
}
