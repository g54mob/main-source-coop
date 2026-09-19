using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[Serializable]
	public class AnchorFlight
	{
		[SerializeField]
		private float _settleSpeedThreshold = 1.2f;

		[SerializeField]
		private float _minFlightTimeBeforeSettle = 0.15f;

		private bool _maxDistanceReached;

		private Vector3 _launchOrigin;

		private float _maxHookDistance = 12f;

		public float MinFlightTimeBeforeSettle => _minFlightTimeBeforeSettle;

		public void Assign(float settleSpeedThreshold, float minFlightTimeBeforeSettle)
		{
			_settleSpeedThreshold = settleSpeedThreshold;
			_minFlightTimeBeforeSettle = minFlightTimeBeforeSettle;
		}

		public bool IsSettled(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			float a = 0f;
			if (sphere.Rigidbody != null && !sphere.Rigidbody.isKinematic)
			{
				a = Mathf.Max(a, sphere.Rigidbody.linearVelocity.magnitude);
			}
			a = Mathf.Max(a, pendant.GetSpeed());
			return a <= _settleSpeedThreshold;
		}

		public void ResetRuntime()
		{
			_maxDistanceReached = false;
		}

		public void Prepare(AnchorThrowSphere sphere, AnchorPendant pendant, Action clearPullGrabbers)
		{
			ResetRuntime();
			clearPullGrabbers?.Invoke();
			pendant.SetReturnSpring(0f, 0f, 1f);
			UnlockRotation(sphere, pendant);
			SnapPairReadyForThrow(sphere, pendant, (sphere.HostTransform != null) ? sphere.HostTransform.forward : Vector3.forward);
			AnchorPhysicsUtil.SetRigidbodyThrown(sphere.Rigidbody, thrown: true);
			AnchorPhysicsUtil.SetRigidbodyThrown(pendant.Rigidbody, thrown: true);
		}

		public void LaunchWithImpulse(AnchorThrowSphere sphere, AnchorPendant pendant, Vector3 target, float flightTime, float upBias, float speedMultiplier, float maxHookDistance, Action clearPullGrabbers)
		{
			ResetRuntime();
			clearPullGrabbers?.Invoke();
			pendant.SetReturnSpring(0f, 0f, 1f);
			Vector3 vector = (_launchOrigin = sphere.HostTransform.position);
			_maxHookDistance = Mathf.Max(maxHookDistance, 0.5f);
			Vector3 vector2 = target - vector;
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = sphere.HostTransform.forward;
			}
			Vector3 normalized = vector2.normalized;
			Quaternion quaternion = Quaternion.LookRotation(normalized, Vector3.up);
			sphere.ApplyPose(vector, quaternion);
			pendant.SnapFlightRelativeTo(vector, quaternion);
			LockRotation(sphere, pendant);
			float num = Mathf.Min(vector2.magnitude, _maxHookDistance);
			float num2 = Mathf.Max(flightTime, 0.05f);
			float num3 = num / num2 * Mathf.Max(speedMultiplier, 0.01f);
			Vector3 velocity = normalized * num3 + Vector3.up * (upBias * num3);
			AnchorPhysicsUtil.ApplyYankVelocity(sphere.Rigidbody, velocity);
			AnchorPhysicsUtil.ApplyYankVelocity(pendant.Rigidbody, velocity);
		}

		public void EnforceMaxChainDistance(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			Rigidbody rigidbody = sphere.Rigidbody;
			if (rigidbody == null || rigidbody.isKinematic)
			{
				return;
			}
			Vector3 vector = rigidbody.position - _launchOrigin;
			float magnitude = vector.magnitude;
			if (magnitude <= _maxHookDistance || magnitude < 0.001f)
			{
				return;
			}
			Vector3 vector2 = vector / magnitude;
			Vector3 position = (rigidbody.position = _launchOrigin + vector2 * _maxHookDistance);
			sphere.HostTransform.position = position;
			Rigidbody rigidbody2 = pendant.Rigidbody;
			if (rigidbody2 != null && !rigidbody2.isKinematic)
			{
				Vector3 vector4 = rigidbody2.position - _launchOrigin;
				float magnitude2 = vector4.magnitude;
				if (magnitude2 > _maxHookDistance && magnitude2 > 0.001f)
				{
					Vector3 position2 = (rigidbody2.position = _launchOrigin + vector4 / magnitude2 * _maxHookDistance);
					if (pendant.Transform != null)
					{
						pendant.Transform.position = position2;
					}
				}
			}
			if (!_maxDistanceReached)
			{
				_maxDistanceReached = true;
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				if (rigidbody2 != null)
				{
					rigidbody2.linearVelocity = Vector3.zero;
					rigidbody2.angularVelocity = Vector3.zero;
				}
				return;
			}
			Vector3 linearVelocity = rigidbody.linearVelocity;
			float num = Vector3.Dot(linearVelocity, vector2);
			if (num > 0f)
			{
				rigidbody.linearVelocity = linearVelocity - vector2 * num;
			}
			if (rigidbody2 != null && !rigidbody2.isKinematic)
			{
				Vector3 linearVelocity2 = rigidbody2.linearVelocity;
				float num2 = Vector3.Dot(linearVelocity2, vector2);
				if (num2 > 0f)
				{
					rigidbody2.linearVelocity = linearVelocity2 - vector2 * num2;
				}
			}
		}

		public void SnapPairToBone(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			if (sphere.TryGetRestPose(out var position, out var rotation))
			{
				sphere.ApplyPose(position, rotation);
				pendant.SnapRelativeTo(position, rotation);
			}
		}

		public void SnapPairReadyForThrow(AnchorThrowSphere sphere, AnchorPendant pendant, Vector3 throwDirection)
		{
			if (sphere.TryGetRestPose(out var position, out var _))
			{
				if (throwDirection.sqrMagnitude < 0.0001f)
				{
					throwDirection = ((sphere.HostTransform != null) ? sphere.HostTransform.forward : Vector3.forward);
				}
				Vector3 vector = throwDirection.normalized;
				if (Mathf.Abs(Vector3.Dot(vector, Vector3.up)) > 0.99f)
				{
					vector = Vector3.forward;
				}
				Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
				sphere.ApplyPose(position, quaternion);
				pendant.SnapFlightRelativeTo(position, quaternion);
			}
		}

		public void SetPhysicsThrown(bool thrown, bool isLatched, AnchorThrowSphere sphere, AnchorPendant pendant, Action clearPullGrabbers, ref bool physGrabReeling)
		{
			if (!isLatched)
			{
				if (thrown)
				{
					SnapPairReadyForThrow(sphere, pendant, (sphere.HostTransform != null) ? sphere.HostTransform.forward : Vector3.forward);
					AnchorPhysicsUtil.SetRigidbodyThrown(sphere.Rigidbody, thrown: true);
					AnchorPhysicsUtil.SetRigidbodyThrown(pendant.Rigidbody, thrown: true);
					return;
				}
				clearPullGrabbers?.Invoke();
				pendant.SetReturnSpring(0f, 0f, 1f);
				UnlockRotation(sphere, pendant);
				physGrabReeling = false;
				AnchorPhysicsUtil.SetRigidbodyThrown(sphere.Rigidbody, thrown: false);
				AnchorPhysicsUtil.SetRigidbodyThrown(pendant.Rigidbody, thrown: false);
				SnapPairToBone(sphere, pendant);
			}
		}

		public void UnlockRotation(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			if (sphere.Rigidbody != null)
			{
				sphere.Rigidbody.constraints = RigidbodyConstraints.None;
			}
			pendant.UnlockFlightRotation();
		}

		private static void LockRotation(AnchorThrowSphere sphere, AnchorPendant pendant)
		{
			if (sphere.Rigidbody != null)
			{
				sphere.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
			pendant.LockFlightRotation();
		}
	}
}
