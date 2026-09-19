using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Sensors
{
	public class SnakeBodyStepSensor : MonoBehaviour
	{
		[SerializeField]
		private SnakeBodyStepAggroSystem _aggroSystem;

		private void OnTriggerEnter(Collider other)
		{
			if (!(_aggroSystem == null) && !(other == null))
			{
				_aggroSystem.OnPlayerLayerEntered(other);
			}
		}
	}
}
