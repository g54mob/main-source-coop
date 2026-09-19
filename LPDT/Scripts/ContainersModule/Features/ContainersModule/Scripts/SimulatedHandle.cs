using System;
using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	[Serializable]
	public class SimulatedHandle
	{
		[SerializeField]
		private Transform _handle;

		[SerializeField]
		private Vector3 _localHingeAxis = Vector3.forward;

		[SerializeField]
		private Vector3 _localDownReference = Vector3.down;

		[SerializeField]
		private float _minAngle = -90f;

		[SerializeField]
		private float _maxAngle = 90f;

		[SerializeField]
		private float _restAngle;

		[SerializeField]
		private float _spring = 45f;

		[SerializeField]
		private float _damping = 12f;

		[SerializeField]
		private float _accelerationInfluence = 0.08f;

		private Quaternion _restLocalRotation;

		private float _angle;

		private float _angularVelocity;

		private bool _initialized;

		public void Initialize()
		{
			if (!(_handle == null))
			{
				_restLocalRotation = _handle.localRotation;
				_angle = Mathf.Clamp(_restAngle, _minAngle, _maxAngle);
				_angularVelocity = 0f;
				_initialized = true;
				ApplyRotation();
			}
		}

		public void Simulate(Transform root, Vector3 inertialAcceleration, float deltaTime)
		{
			if (_initialized && (bool)_handle && (bool)root && !(deltaTime <= 0f))
			{
				Vector3 safeDirection = GetSafeDirection(_handle.TransformDirection(_localHingeAxis), root.right);
				Vector3 projectedReferenceDirection = GetProjectedReferenceDirection(safeDirection, root);
				Vector3 to = Vector3.ProjectOnPlane(Physics.gravity - inertialAcceleration * _accelerationInfluence, safeDirection);
				if (to.sqrMagnitude < 0.0001f)
				{
					to = projectedReferenceDirection;
				}
				else
				{
					to.Normalize();
				}
				float value = Vector3.SignedAngle(projectedReferenceDirection, to, safeDirection);
				value = Mathf.Clamp(value, _minAngle, _maxAngle);
				float num = Mathf.DeltaAngle(_angle, value);
				_angularVelocity += num * Mathf.Max(0f, _spring) * deltaTime;
				_angularVelocity *= Mathf.Exp((0f - Mathf.Max(0f, _damping)) * deltaTime);
				_angle = Mathf.Clamp(_angle + _angularVelocity * deltaTime, _minAngle, _maxAngle);
				if (Mathf.Approximately(_angle, _minAngle) || Mathf.Approximately(_angle, _maxAngle))
				{
					_angularVelocity = 0f;
				}
				ApplyRotation();
			}
		}

		private Vector3 GetProjectedReferenceDirection(Vector3 hingeAxis, Transform root)
		{
			Vector3 safeDirection = GetSafeDirection((_handle.parent ? (_handle.parent.rotation * _restLocalRotation) : _restLocalRotation) * _localDownReference, -root.up);
			safeDirection = Vector3.ProjectOnPlane(safeDirection, hingeAxis);
			if (safeDirection.sqrMagnitude > 0.0001f)
			{
				return safeDirection.normalized;
			}
			return Vector3.ProjectOnPlane(-root.up, hingeAxis).normalized;
		}

		private void ApplyRotation()
		{
			Vector3 safeDirection = GetSafeDirection(_localHingeAxis, Vector3.forward);
			_handle.localRotation = _restLocalRotation * Quaternion.AngleAxis(_angle, safeDirection);
		}

		private static Vector3 GetSafeDirection(Vector3 direction, Vector3 fallback)
		{
			if (direction.sqrMagnitude > 0.0001f)
			{
				return direction.normalized;
			}
			if (!(fallback.sqrMagnitude > 0.0001f))
			{
				return Vector3.forward;
			}
			return fallback.normalized;
		}
	}
}
