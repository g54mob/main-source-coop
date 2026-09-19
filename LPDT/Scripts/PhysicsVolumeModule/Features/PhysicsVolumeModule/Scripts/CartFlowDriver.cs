using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CartFlowDriver : MonoBehaviour
	{
		[Header("References (auto-resolved from this object when empty)")]
		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private CartAngularLock _angularLock;

		[Header("Response")]
		[Tooltip("How fast velocity blends toward the accumulated flow, 1/s.")]
		[SerializeField]
		private float _acceleration = 3f;

		[Tooltip("Max yaw rate toward the flow tangent at full strength, deg/s.")]
		[SerializeField]
		private float _turnRate = 60f;

		[Tooltip("Proportional steering gain: desired yaw rate = error x this, capped by turn rate, 1/s.")]
		[SerializeField]
		private float _steeringResponse = 2f;

		[Tooltip("Sideways pull toward the curve axis, m/s per meter of offset.")]
		[SerializeField]
		private float _centeringGain = 1f;

		[Tooltip("Cap on the centering pull, m/s.")]
		[SerializeField]
		private float _maxCenteringSpeed = 1.2f;

		[Tooltip("How hard the cart's SIDEWAYS velocity component is driven toward the centering pull, m/s per second. This is the direct keep-in-channel push: the velocity is split into along-flow and sideways, and only the sideways part is pushed toward the channel axis - no rotation involved, and the along-flow ride stays whatever gravity and the drive made it. 0 disables.")]
		[SerializeField]
		private float _centeringResponse = 6f;

		[Tooltip("How fast the cart's MOMENTUM is bent toward the flow direction at full strength, deg/s. Yaw steering only turns the box - it does not turn where the mass is going, so on a curved descent gravity-fed velocity runs straight into the outer wall. This rotates the horizontal velocity vector itself (speed preserved - the stairs still power the ride, the channel just bends it around the curve).")]
		[SerializeField]
		private float _velocityRedirectRate = 90f;

		[Tooltip("How many meters AHEAD along the curve the cart aims (pure pursuit): steering, momentum redirection and drive all chase a guide point this far ahead of the closest point (clamped at the curve end), so the cart anticipates the bend instead of reacting to the tangent underfoot.")]
		[SerializeField]
		private float _lookAheadDistance = 3f;

		[Tooltip("Seconds to build from standstill up to the zone's full flow speed while in the channel.")]
		[SerializeField]
		private float _rampUpSeconds = 3f;

		[Tooltip("Seconds to bleed the accumulated flow momentum off after leaving the channel.")]
		[SerializeField]
		private float _rampDownSeconds = 3f;

		[Tooltip("Lift the cart's pitch/roll freeze while flow-driven so it can angle down stairs.")]
		[SerializeField]
		private bool _unlockAngleInFlow = true;

		private Vector3 _flowVelocity;

		private bool _angleUnlockedByFlow;

		private CartGrabObject _cartGrabObject;

		public bool IsFlowDriving { get; private set; }

		private void Awake()
		{
			if (_grabable == null)
			{
				TryGetComponent<SimplePointGrabable>(out _grabable);
			}
			if (_rigidbody == null)
			{
				TryGetComponent<Rigidbody>(out _rigidbody);
			}
			if (_angularLock == null)
			{
				TryGetComponent<CartAngularLock>(out _angularLock);
			}
			if (_cartGrabObject == null)
			{
				TryGetComponent<CartGrabObject>(out _cartGrabObject);
			}
		}

		private void FixedUpdate()
		{
			if (_grabable == null || _rigidbody == null || !_grabable.Initialized)
			{
				return;
			}
			if (!_grabable.HasStateAuthority || _rigidbody.isKinematic || _grabable.GrabbedByPlayersCount != 0 || _grabable.GrabbedBySomethingCount != 0)
			{
				_flowVelocity = Vector3.zero;
				IsFlowDriving = false;
				bool flag = _grabable.HasStateAuthority && !_rigidbody.isKinematic && _grabable.GrabbedByPlayersCount > 0 && IsInsideAnyZone();
				UpdateAngleLock(flag);
				SetTerrainFollow(flag);
				return;
			}
			SetTerrainFollow(value: false);
			CartFlowZone strongestZone = null;
			Vector3 closestPoint = Vector3.zero;
			Vector3 flowDirection = Vector3.forward;
			Vector3 steerDirection = Vector3.forward;
			float steerStrength = 0f;
			float driveStrength = 0f;
			bool flag2 = TrySampleStrongestZone(out strongestZone, out closestPoint, out flowDirection, out steerDirection, out steerStrength, out driveStrength);
			UpdateAngleLock(flag2);
			Vector3 flowTarget = Vector3.zero;
			if (flag2)
			{
				flowTarget = FlowTarget(strongestZone, closestPoint, flowDirection);
				AccumulateFlow(strongestZone, flowTarget, driveStrength);
			}
			else
			{
				DecayFlow();
			}
			IsFlowDriving = _flowVelocity.sqrMagnitude > 0.0025f;
			Vector3 vector = _rigidbody.linearVelocity;
			if (IsFlowDriving)
			{
				vector = Vector3.Lerp(b: new Vector3(_flowVelocity.x, vector.y, _flowVelocity.z), a: vector, t: _acceleration * Time.fixedDeltaTime);
			}
			if (flag2)
			{
				vector = RedirectMomentum(vector, flowTarget, steerStrength);
				vector = ApplyCentering(vector, flowDirection, closestPoint, steerStrength);
				SteerTowardFlow(steerDirection, steerStrength);
			}
			_rigidbody.linearVelocity = vector;
		}

		private Vector3 ApplyCentering(Vector3 velocity, Vector3 flowDirection, Vector3 closestPoint, float strength)
		{
			if (_centeringResponse <= 0f)
			{
				return velocity;
			}
			Vector3 vector = new Vector3(flowDirection.x, 0f, flowDirection.z);
			if (vector.sqrMagnitude < 0.0004f)
			{
				return velocity;
			}
			vector.Normalize();
			Vector3 vector2 = closestPoint - _rigidbody.worldCenterOfMass;
			Vector3 vector3 = vector2 - vector * Vector3.Dot(vector2, vector);
			vector3.y = 0f;
			Vector3 target = Vector3.ClampMagnitude(vector3 * _centeringGain, _maxCenteringSpeed);
			Vector3 vector4 = new Vector3(velocity.x, 0f, velocity.z);
			float num = Vector3.Dot(vector4, vector);
			Vector3 current = vector4 - vector * num;
			current = Vector3.MoveTowards(current, target, _centeringResponse * strength * Time.fixedDeltaTime);
			Vector3 vector5 = vector * num + current;
			velocity.x = vector5.x;
			velocity.z = vector5.z;
			return velocity;
		}

		private void UpdateAngleLock(bool flowDriving)
		{
			if (_angularLock == null)
			{
				return;
			}
			if (flowDriving && _unlockAngleInFlow)
			{
				if (_angularLock.IsAngleLocked)
				{
					_angularLock.UnlockAngle();
					_angleUnlockedByFlow = true;
				}
			}
			else if (_angleUnlockedByFlow)
			{
				_angleUnlockedByFlow = false;
				if (!_angularLock.IsAngleLocked)
				{
					_angularLock.LockAngle();
				}
			}
		}

		private bool IsInsideAnyZone()
		{
			IReadOnlyList<CartFlowZone> zones = CartFlowZoneProvider.Zones;
			Vector3 worldCenterOfMass = _rigidbody.worldCenterOfMass;
			for (int i = 0; i < zones.Count; i++)
			{
				CartFlowZone cartFlowZone = zones[i];
				if (!(cartFlowZone == null) && cartFlowZone.isActiveAndEnabled && cartFlowZone.TrySample(worldCenterOfMass, _lookAheadDistance, out var _, out var _, out var _, out var _))
				{
					return true;
				}
			}
			return false;
		}

		private void SetTerrainFollow(bool value)
		{
			if (_cartGrabObject != null)
			{
				_cartGrabObject.SetTerrainFollow(value);
			}
		}

		private bool TrySampleStrongestZone(out CartFlowZone strongestZone, out Vector3 closestPoint, out Vector3 flowDirection, out Vector3 steerDirection, out float steerStrength, out float driveStrength)
		{
			strongestZone = null;
			closestPoint = Vector3.zero;
			flowDirection = Vector3.forward;
			steerDirection = Vector3.forward;
			steerStrength = 0f;
			driveStrength = 0f;
			IReadOnlyList<CartFlowZone> zones = CartFlowZoneProvider.Zones;
			Vector3 worldCenterOfMass = _rigidbody.worldCenterOfMass;
			Vector3 vector = _rigidbody.rotation * Vector3.forward;
			vector.y = 0f;
			for (int i = 0; i < zones.Count; i++)
			{
				CartFlowZone cartFlowZone = zones[i];
				if (cartFlowZone == null || !cartFlowZone.isActiveAndEnabled || !cartFlowZone.TrySample(worldCenterOfMass, _lookAheadDistance, out var closestPoint2, out var guidePoint, out var guideTangent, out var strength))
				{
					continue;
				}
				Vector3 vector2 = guidePoint - closestPoint2;
				float magnitude = vector2.magnitude;
				Vector3 normalized = Vector3.Slerp((magnitude > 0.001f) ? (vector2 / magnitude) : guideTangent, t: (_lookAheadDistance > 0.001f) ? Mathf.Clamp01(1f - magnitude / _lookAheadDistance) : 1f, b: guideTangent).normalized;
				float num = 1f;
				Vector3 vector3 = normalized;
				Vector3 to = new Vector3(normalized.x, 0f, normalized.z);
				if (to.sqrMagnitude > 0.01f && vector.sqrMagnitude > 0.01f)
				{
					float num2 = Vector3.Angle(vector, to);
					bool flag = num2 > 90f;
					float num3 = (flag ? (180f - num2) : num2);
					float num4 = Mathf.Max(cartFlowZone.AcceptanceAngle, 1f);
					if (num3 >= num4)
					{
						continue;
					}
					num = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(num4, num4 * 0.5f, num3));
					if (flag)
					{
						vector3 = -normalized;
					}
				}
				if (!(strength <= steerStrength))
				{
					strongestZone = cartFlowZone;
					closestPoint = closestPoint2;
					flowDirection = normalized;
					steerDirection = vector3;
					steerStrength = strength;
					driveStrength = strength * num;
				}
			}
			if (strongestZone != null)
			{
				return steerStrength > 0f;
			}
			return false;
		}

		private Vector3 FlowTarget(CartFlowZone zone, Vector3 closestPoint, Vector3 flowDirection)
		{
			Vector3 vector = closestPoint - _rigidbody.worldCenterOfMass;
			Vector3 vector2 = vector - flowDirection * Vector3.Dot(vector, flowDirection);
			vector2.y = 0f;
			vector2 = Vector3.ClampMagnitude(vector2 * _centeringGain, _maxCenteringSpeed);
			return flowDirection * zone.FlowSpeed + vector2;
		}

		private void AccumulateFlow(CartFlowZone zone, Vector3 flowTarget, float driveStrength)
		{
			float num = zone.FlowSpeed / Mathf.Max(_rampUpSeconds, 0.05f);
			_flowVelocity = Vector3.MoveTowards(_flowVelocity, flowTarget, num * driveStrength * Time.fixedDeltaTime);
		}

		private void DecayFlow()
		{
			float num = _flowVelocity.magnitude / Mathf.Max(_rampDownSeconds, 0.05f);
			_flowVelocity = Vector3.MoveTowards(_flowVelocity, Vector3.zero, num * Time.fixedDeltaTime);
		}

		private Vector3 RedirectMomentum(Vector3 velocity, Vector3 flowTarget, float strength)
		{
			Vector3 current = new Vector3(velocity.x, 0f, velocity.z);
			Vector3 vector = new Vector3(flowTarget.x, 0f, flowTarget.z);
			if (current.sqrMagnitude <= 0.01f || vector.sqrMagnitude <= 0.01f)
			{
				return velocity;
			}
			float maxRadiansDelta = _velocityRedirectRate * strength * Time.fixedDeltaTime * (MathF.PI / 180f);
			Vector3 vector2 = Vector3.RotateTowards(current, vector.normalized * current.magnitude, maxRadiansDelta, 0f);
			velocity.x = vector2.x;
			velocity.z = vector2.z;
			return velocity;
		}

		private void SteerTowardFlow(Vector3 steerDirection, float strength)
		{
			Vector3 to = new Vector3(steerDirection.x, 0f, steerDirection.z);
			Vector3 vector = _rigidbody.rotation * Vector3.forward;
			vector.y = 0f;
			if (!(to.sqrMagnitude < 0.02f) && !(vector.sqrMagnitude < 0.0001f))
			{
				float num = Vector3.SignedAngle(vector, to, Vector3.up);
				float num2 = _turnRate * strength;
				float num3 = Mathf.Clamp(num * _steeringResponse, 0f - num2, num2);
				Vector3 angularVelocity = _rigidbody.angularVelocity;
				angularVelocity.y = Mathf.Lerp(angularVelocity.y, num3 * (MathF.PI / 180f), strength);
				_rigidbody.angularVelocity = angularVelocity;
			}
		}
	}
}
