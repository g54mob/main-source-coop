using System.Collections.Generic;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	public class ChangeColliderOnJointBreak : MonoBehaviour
	{
		[SerializeField]
		private HingeDespawner _hingeDespawner;

		[SerializeField]
		private List<Collider> _hingeColliders;

		[SerializeField]
		private List<Collider> _commonColliders;

		private void OnEnable()
		{
			_hingeDespawner.OnBreakEvent += SpawnColliderOnBreake;
		}

		private void OnDisable()
		{
			_hingeDespawner.OnBreakEvent -= SpawnColliderOnBreake;
		}

		private void SpawnColliderOnBreake()
		{
			foreach (Collider hingeCollider in _hingeColliders)
			{
				hingeCollider.enabled = false;
			}
			foreach (Collider commonCollider in _commonColliders)
			{
				commonCollider.enabled = true;
			}
		}
	}
}
