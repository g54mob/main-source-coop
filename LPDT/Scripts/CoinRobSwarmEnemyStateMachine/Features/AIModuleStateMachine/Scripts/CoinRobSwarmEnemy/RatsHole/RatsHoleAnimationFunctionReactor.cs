using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleAnimationFunctionReactor : MonoBehaviour
	{
		public event Action OnEnemySpawnMoment;

		public void InvokeOnEnemySpawnMoment()
		{
			this.OnEnemySpawnMoment?.Invoke();
		}
	}
}
