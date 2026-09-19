using Features.AIModule.Scripts;
using UnityEngine;

namespace Features.EnemyFearingModule.Scripts
{
	public class EnemyTypeWithTransform
	{
		public EnemyType EnemyType;

		public Vector3 Position;

		public EnemyTypeWithTransform(EnemyType enemyType, Vector3 position)
		{
			EnemyType = enemyType;
			Position = position;
		}
	}
}
