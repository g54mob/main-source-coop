using UnityEngine;

namespace ECM2
{
	public struct FindGroundResult
	{
		public bool hitGround;

		public bool isWalkable;

		public Vector3 position;

		public Vector3 surfaceNormal;

		public Collider collider;

		public float groundDistance;

		public bool isRaycastResult;

		public float raycastDistance;

		public RaycastHit hitResult;

		public bool isWalkableGround
		{
			get
			{
				if (hitGround)
				{
					return isWalkable;
				}
				return false;
			}
		}

		public Vector3 point => hitResult.point;

		public Vector3 normal => hitResult.normal;

		public Rigidbody rigidbody
		{
			get
			{
				if (!collider)
				{
					return null;
				}
				return collider.attachedRigidbody;
			}
		}

		public Transform transform
		{
			get
			{
				if (collider == null)
				{
					return null;
				}
				Rigidbody attachedRigidbody = collider.attachedRigidbody;
				if (!attachedRigidbody)
				{
					return collider.transform;
				}
				return attachedRigidbody.transform;
			}
		}

		public float GetDistanceToGround()
		{
			if (!isRaycastResult)
			{
				return groundDistance;
			}
			return raycastDistance;
		}

		public void SetFromSweepResult(bool hitGround, bool isWalkable, Vector3 position, float sweepDistance, ref RaycastHit inHit, Vector3 surfaceNormal)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			collider = inHit.collider;
			groundDistance = sweepDistance;
			isRaycastResult = false;
			raycastDistance = 0f;
			hitResult = inHit;
			this.surfaceNormal = surfaceNormal;
		}

		public void SetFromSweepResult(bool hitGround, bool isWalkable, Vector3 position, Vector3 point, Vector3 normal, Vector3 surfaceNormal, Collider collider, float sweepDistance)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			this.collider = collider;
			groundDistance = sweepDistance;
			isRaycastResult = false;
			raycastDistance = 0f;
			hitResult = new RaycastHit
			{
				point = point,
				normal = normal,
				distance = sweepDistance
			};
			this.surfaceNormal = surfaceNormal;
		}

		public void SetFromRaycastResult(bool hitGround, bool isWalkable, Vector3 position, float sweepDistance, float castDistance, ref RaycastHit inHit)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			collider = inHit.collider;
			groundDistance = sweepDistance;
			isRaycastResult = true;
			raycastDistance = castDistance;
			float distance = hitResult.distance;
			hitResult = inHit;
			hitResult.distance = distance;
			surfaceNormal = hitResult.normal;
		}
	}
}
