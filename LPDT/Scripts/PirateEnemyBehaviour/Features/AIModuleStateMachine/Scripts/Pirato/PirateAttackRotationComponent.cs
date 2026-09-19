using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	public class PirateAttackRotationComponent : MonoBehaviour
	{
		[SerializeField]
		private Transform _rotationTransform;

		[SerializeField]
		private float _rotationSpeed = 5f;

		public void RotateTowardsTarget(Vector3 targetPosition, float speedOverride = 0f)
		{
			Vector3 normalized = (targetPosition - _rotationTransform.position).normalized;
			normalized.y = 0f;
			Quaternion b = Quaternion.LookRotation(normalized);
			float num = ((speedOverride > 0f) ? speedOverride : _rotationSpeed);
			_rotationTransform.rotation = Quaternion.Slerp(_rotationTransform.rotation, b, Time.deltaTime * num);
		}
	}
}
