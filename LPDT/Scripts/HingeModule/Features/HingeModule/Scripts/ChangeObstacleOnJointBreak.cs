using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Features.HingeModule.Scripts
{
	public class ChangeObstacleOnJointBreak : MonoBehaviour
	{
		[SerializeField]
		private HingeDespawner _hingeDespawner;

		[SerializeField]
		private List<NavMeshObstacle> _obstacles;

		[SerializeField]
		private bool _targetState;

		private void OnEnable()
		{
			_hingeDespawner.OnBreakEvent += ChangeGrabbable;
		}

		private void OnDisable()
		{
			_hingeDespawner.OnBreakEvent -= ChangeGrabbable;
		}

		private void ChangeGrabbable()
		{
			foreach (NavMeshObstacle obstacle in _obstacles)
			{
				obstacle.enabled = _targetState;
			}
		}
	}
}
