using Features.AIModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.EnemyFearingModule.Scripts
{
	public interface IEnemyFearCallbackListener
	{
		EnemyType EnemyType { get; }

		int EnemyInstants { get; }

		NetworkObject NetworkObject { get; }

		bool IsDespawnAfterFear { get; set; }

		Vector3? PositionOnFearEnd { get; set; }

		bool FearCompleted { get; set; }

		void Fear();
	}
}
