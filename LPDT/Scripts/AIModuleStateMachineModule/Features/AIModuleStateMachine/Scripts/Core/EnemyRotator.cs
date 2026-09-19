using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	public class EnemyRotator : MonoBehaviour
	{
		[SerializeField]
		private GameObject _bodyObject;

		[SerializeField]
		private float _bodyRotationSpeed = 2f;

		private float _currentBodyYRotation;

		private bool _isRotationInitialized;

		public void RotateTowardsDirection(Vector3 direction)
		{
			RotateTowardsDirection(direction, _bodyRotationSpeed);
		}

		public void SnapTowardsDirection(Vector3 direction)
		{
			if (TryGetTargetYRotation(direction, out var targetY))
			{
				_currentBodyYRotation = targetY;
				_isRotationInitialized = true;
				_bodyObject.transform.rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
			}
		}

		private void RotateTowardsDirection(Vector3 direction, float bodyRotationSpeed)
		{
			if (TryGetTargetYRotation(direction, out var targetY))
			{
				if (!_isRotationInitialized)
				{
					_currentBodyYRotation = _bodyObject.transform.eulerAngles.y;
					_isRotationInitialized = true;
				}
				_currentBodyYRotation = Mathf.LerpAngle(_currentBodyYRotation, targetY, Time.deltaTime * bodyRotationSpeed);
				_bodyObject.transform.rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
			}
		}

		private bool TryGetTargetYRotation(Vector3 direction, out float targetY)
		{
			targetY = 0f;
			if (direction == Vector3.zero)
			{
				return false;
			}
			direction.Normalize();
			targetY = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
			return true;
		}
	}
}
