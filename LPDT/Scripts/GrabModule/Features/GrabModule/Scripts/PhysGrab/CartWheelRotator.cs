using System;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(1)]
	public class CartWheelRotator : NetworkBehaviour
	{
		[SerializeField]
		private Transform[] _wheels;

		[SerializeField]
		private Rigidbody _cartRigidbody;

		[SerializeField]
		private float _wheelRadius = 0.25f;

		[SerializeField]
		private float _minSpeed = 0.05f;

		[SerializeField]
		private float _rotationDirection = -1f;

		[SerializeField]
		[Range(0f, 89f)]
		private float _maxSideAngle = 35f;

		[SerializeField]
		private Vector3 _wheelAxis = Vector3.right;

		[Tooltip("Two-wheeled carts only: each wheel also rolls by its own distance from the turn centre, so an in-place turn counter-rotates the wheels instead of skidding.")]
		[SerializeField]
		private bool _useDifferentialSpin;

		private bool _spawned;

		private bool _hasLastPosition;

		private Vector3 _lastPosition;

		private float[] _wheelLateralOffsets;

		private bool _hasLastRotation;

		private float _lastYaw;

		private float _turnRateRadians;

		[Networked]
		[UnityNonSerialized]
		[NetworkedWeaved(0, 1)]
		public unsafe float ForwardSpeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartWheelRotator.ForwardSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartWheelRotator.ForwardSpeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		public override void Spawned()
		{
			_spawned = true;
			CacheWheelLateralOffsets();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_spawned = false;
			_hasLastPosition = false;
			_hasLastRotation = false;
		}

		private void CacheWheelLateralOffsets()
		{
			if (!_useDifferentialSpin || _wheels == null)
			{
				return;
			}
			_wheelLateralOffsets = new float[_wheels.Length];
			for (int i = 0; i < _wheels.Length; i++)
			{
				if (!(_wheels[i] == null))
				{
					_wheelLateralOffsets[i] = Vector3.Dot(_wheels[i].position - base.transform.position, base.transform.right);
				}
			}
		}

		private void Update()
		{
			if (!_spawned || _cartRigidbody == null)
			{
				return;
			}
			if (base.Object.HasStateAuthority)
			{
				CalculateForwardSpeed();
			}
			CalculateTurnRate();
			for (int i = 0; i < _wheels.Length; i++)
			{
				Transform transform = _wheels[i];
				if (!(transform == null))
				{
					float angle = (ForwardSpeed - _turnRateRadians * GetWheelLateralOffset(i)) / (MathF.PI * 2f * _wheelRadius) * 360f * Time.deltaTime * _rotationDirection;
					transform.Rotate(_wheelAxis, angle, Space.Self);
				}
			}
		}

		private float GetWheelLateralOffset(int index)
		{
			if (_wheelLateralOffsets == null || index >= _wheelLateralOffsets.Length)
			{
				return 0f;
			}
			return _wheelLateralOffsets[index];
		}

		private void CalculateTurnRate()
		{
			if (_useDifferentialSpin)
			{
				float y = _cartRigidbody.transform.eulerAngles.y;
				if (!_hasLastRotation)
				{
					_hasLastRotation = true;
					_lastYaw = y;
				}
				else
				{
					float num = Mathf.DeltaAngle(_lastYaw, y);
					_lastYaw = y;
					_turnRateRadians = ((Time.deltaTime > 0f) ? (num * (MathF.PI / 180f) / Time.deltaTime) : 0f);
				}
			}
		}

		private void CalculateForwardSpeed()
		{
			ForwardSpeed = 0f;
			Vector3 lhs = Vector3.ProjectOnPlane(GetVelocity(), Vector3.up);
			if (!(lhs.magnitude < _minSpeed))
			{
				Vector3 normalized = lhs.normalized;
				Vector3 normalized2 = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
				float f = Vector3.Dot(normalized2, normalized);
				float num = Mathf.Cos(_maxSideAngle * (MathF.PI / 180f));
				if (!(Mathf.Abs(f) < num))
				{
					ForwardSpeed = Vector3.Dot(lhs, normalized2);
				}
			}
		}

		private Vector3 GetVelocity()
		{
			if (!_cartRigidbody.isKinematic)
			{
				return _cartRigidbody.linearVelocity;
			}
			Vector3 position = _cartRigidbody.transform.position;
			if (!_hasLastPosition)
			{
				_hasLastPosition = true;
				_lastPosition = position;
				return Vector3.zero;
			}
			Vector3 result = ((Time.deltaTime > 0f) ? ((position - _lastPosition) / Time.deltaTime) : Vector3.zero);
			_lastPosition = position;
			return result;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
