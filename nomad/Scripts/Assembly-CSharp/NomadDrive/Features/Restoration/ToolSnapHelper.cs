using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class ToolSnapHelper
	{
		public float SnapSmoothness;

		public float SnapOffset;

		public Transform AlignTransform;

		public bool UseColliderBoundsForSnap;

		public Vector3 SnapRotationOffset;

		public bool EnableMovementRotation;

		public float MovementRotationSpeed = 8f;

		public float MovementThreshold = 0.005f;

		public float MaxMovementSpeed = 0.5f;

		public float SnapGraceSeconds = 0.12f;

		private Collider _snapReferenceCollider;

		private Vector3 _positionVelocity;

		private Vector3 _lastSnapPoint;

		private Vector3 _smoothedMovementDir;

		private float _movementInfluence;

		private bool _hasLastSnapPoint;

		private float _graceTimer;

		private bool _inGrace;

		public bool IsSnapped { get; private set; }

		public Vector3 SnapPoint { get; private set; }

		public Vector3 SnapNormal { get; private set; }

		public GameObject CurrentTarget { get; private set; }

		public float MovementSpeed { get; private set; }

		public void CacheSnapReferenceCollider()
		{
			if (AlignTransform != null)
			{
				_snapReferenceCollider = AlignTransform.GetComponent<Collider>();
			}
		}

		public bool ProcessHit(Transform toolTransform, Vector3 hitPoint, Vector3 hitNormal, GameObject hitObject)
		{
			CurrentTarget = hitObject;
			SnapPoint = hitPoint;
			SnapNormal = hitNormal;
			_inGrace = false;
			_graceTimer = 0f;
			if (EnableMovementRotation)
			{
				UpdateMovementTracking(hitPoint, hitNormal);
			}
			if (IsSnapped)
			{
				return false;
			}
			_positionVelocity = Vector3.zero;
			IsSnapped = true;
			return true;
		}

		public bool ProcessNoHit()
		{
			if (!IsSnapped)
			{
				CurrentTarget = null;
				ResetMovementState();
				return false;
			}
			if (!_inGrace)
			{
				_inGrace = true;
				_graceTimer = SnapGraceSeconds;
			}
			_graceTimer -= Time.deltaTime;
			if (_graceTimer > 0f)
			{
				return false;
			}
			_inGrace = false;
			CurrentTarget = null;
			ResetMovementState();
			IsSnapped = false;
			return true;
		}

		public bool ForceRelease()
		{
			_inGrace = false;
			_graceTimer = 0f;
			CurrentTarget = null;
			ResetMovementState();
			if (!IsSnapped)
			{
				return false;
			}
			IsSnapped = false;
			return true;
		}

		public void TickLerp(Transform toolTransform)
		{
			if (IsSnapped && !(toolTransform == null))
			{
				Vector3 target = CalculateSnapPosition(SnapPoint, SnapNormal);
				Quaternion b = CalculateSnapRotation(SnapNormal);
				float smoothTime = 1f / Mathf.Max(0.01f, SnapSmoothness);
				toolTransform.position = Vector3.SmoothDamp(toolTransform.position, target, ref _positionVelocity, smoothTime);
				float t = 1f - Mathf.Exp((0f - SnapSmoothness) * Time.deltaTime);
				toolTransform.rotation = Quaternion.Slerp(toolTransform.rotation, b, t);
			}
		}

		public Quaternion CalculateSnapRotation(Vector3 hitNormal)
		{
			Vector3 vector = Vector3.up;
			if (Mathf.Abs(Vector3.Dot(hitNormal, Vector3.up)) > 0.99f)
			{
				vector = Vector3.forward;
			}
			if (EnableMovementRotation && _movementInfluence > 0f)
			{
				vector = Vector3.Slerp(vector, _smoothedMovementDir, _movementInfluence);
			}
			Quaternion quaternion = Quaternion.LookRotation(-hitNormal, vector);
			Quaternion quaternion2 = Quaternion.Euler(SnapRotationOffset);
			return quaternion * quaternion2;
		}

		public Vector3 CalculateSnapPosition(Vector3 hitPoint, Vector3 hitNormal)
		{
			Vector3 vector = hitPoint + hitNormal * SnapOffset;
			if (AlignTransform == null)
			{
				if (UseColliderBoundsForSnap && _snapReferenceCollider != null)
				{
					Quaternion toolRotation = CalculateSnapRotation(hitNormal);
					float colliderExtentAlongDirection = GetColliderExtentAlongDirection(_snapReferenceCollider, toolRotation, -hitNormal);
					vector += hitNormal * colliderExtentAlongDirection;
				}
				return vector;
			}
			Quaternion quaternion = CalculateSnapRotation(hitNormal);
			Vector3 localPosition = AlignTransform.localPosition;
			Vector3 vector2 = quaternion * localPosition;
			Vector3 result = vector - vector2;
			if (UseColliderBoundsForSnap && _snapReferenceCollider != null)
			{
				float colliderExtentAlongDirection2 = GetColliderExtentAlongDirection(_snapReferenceCollider, quaternion, -hitNormal);
				result += hitNormal * colliderExtentAlongDirection2;
			}
			return result;
		}

		private void UpdateMovementTracking(Vector3 hitPoint, Vector3 hitNormal)
		{
			if (!_hasLastSnapPoint)
			{
				_lastSnapPoint = hitPoint;
				_hasLastSnapPoint = true;
				return;
			}
			Vector3 vector = hitPoint - _lastSnapPoint;
			_lastSnapPoint = hitPoint;
			Vector3 vector2 = vector - Vector3.Dot(vector, hitNormal) * hitNormal;
			float num = ((Time.deltaTime > 0f) ? (vector2.magnitude / Time.deltaTime) : 0f);
			float b = ((MaxMovementSpeed > 0f) ? Mathf.Clamp01(num / MaxMovementSpeed) : 0f);
			if (vector2.sqrMagnitude > MovementThreshold * MovementThreshold)
			{
				Vector3 normalized = vector2.normalized;
				float num2 = 1f - Mathf.Exp((0f - MovementRotationSpeed) * Time.deltaTime);
				if (_movementInfluence < 0.01f)
				{
					_smoothedMovementDir = normalized;
				}
				else
				{
					_smoothedMovementDir = Vector3.Slerp(_smoothedMovementDir, normalized, num2);
				}
				_movementInfluence = Mathf.MoveTowards(_movementInfluence, 1f, num2 * 0.5f);
				MovementSpeed = Mathf.Lerp(MovementSpeed, b, 1f - Mathf.Exp(-10f * Time.deltaTime));
			}
			else
			{
				MovementSpeed = Mathf.Lerp(MovementSpeed, 0f, 1f - Mathf.Exp(-5f * Time.deltaTime));
			}
		}

		private void ResetMovementState()
		{
			_hasLastSnapPoint = false;
			_movementInfluence = 0f;
			MovementSpeed = 0f;
			_positionVelocity = Vector3.zero;
		}

		private float GetColliderExtentAlongDirection(Collider collider, Quaternion toolRotation, Vector3 worldDirection)
		{
			Vector3 localColliderExtents = GetLocalColliderExtents(collider);
			Vector3 rhs = Quaternion.Inverse(toolRotation * collider.transform.localRotation) * worldDirection.normalized;
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				float num2 = Vector3.Dot(new Vector3((float)(((i & 1) == 0) ? 1 : (-1)) * localColliderExtents.x, (float)(((i & 2) == 0) ? 1 : (-1)) * localColliderExtents.y, (float)(((i & 4) == 0) ? 1 : (-1)) * localColliderExtents.z), rhs);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		private Vector3 GetLocalColliderExtents(Collider collider)
		{
			Vector3 localScale = collider.transform.localScale;
			if (collider is BoxCollider boxCollider)
			{
				return Vector3.Scale(boxCollider.size * 0.5f, localScale);
			}
			if (collider is SphereCollider sphereCollider)
			{
				float num = Mathf.Max(localScale.x, localScale.y, localScale.z);
				return Vector3.one * (sphereCollider.radius * num);
			}
			if (collider is CapsuleCollider { radius: var radius } capsuleCollider)
			{
				float num2 = capsuleCollider.height * 0.5f;
				return Vector3.Scale(capsuleCollider.direction switch
				{
					0 => new Vector3(num2, radius, radius), 
					1 => new Vector3(radius, num2, radius), 
					_ => new Vector3(radius, radius, num2), 
				}, localScale);
			}
			Bounds bounds = collider.bounds;
			Vector3 lossyScale = collider.transform.lossyScale;
			return new Vector3((lossyScale.x > 0f) ? (bounds.extents.x / lossyScale.x * localScale.x) : bounds.extents.x, (lossyScale.y > 0f) ? (bounds.extents.y / lossyScale.y * localScale.y) : bounds.extents.y, (lossyScale.z > 0f) ? (bounds.extents.z / lossyScale.z * localScale.z) : bounds.extents.z);
		}
	}
}
