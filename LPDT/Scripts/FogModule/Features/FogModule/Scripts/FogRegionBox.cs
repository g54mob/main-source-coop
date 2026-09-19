using System;
using UnityEngine;

namespace Features.FogModule.Scripts
{
	[Serializable]
	public class FogRegionBox
	{
		[field: Tooltip("Box collider describing this part of the region. Edit it with the collider's scene handles. It is disabled at runtime — the region only reads its shape, never uses physics.")]
		[field: SerializeField]
		public BoxCollider BoxCollider { get; private set; }

		[field: Tooltip("Blend only: ratio at this box's -Z face. Chain boxes along an uneven aisle by continuing the previous box's range.")]
		[field: Range(0f, 1f)]
		[field: SerializeField]
		public float RatioFrom { get; private set; }

		[field: Tooltip("Blend only: ratio at this box's +Z face.")]
		[field: Range(0f, 1f)]
		[field: SerializeField]
		public float RatioTo { get; private set; } = 1f;

		public void DisablePhysics()
		{
			BoxCollider.enabled = false;
		}

		public bool Contains(Vector3 worldPosition)
		{
			Vector3 localPosition = GetLocalPosition(worldPosition);
			Vector3 vector = BoxCollider.size * 0.5f;
			if (Mathf.Abs(localPosition.x) <= vector.x && Mathf.Abs(localPosition.y) <= vector.y)
			{
				return Mathf.Abs(localPosition.z) <= vector.z;
			}
			return false;
		}

		public float GetBlendRatio(Vector3 worldPosition)
		{
			float z = BoxCollider.size.z;
			if (Mathf.Approximately(z, 0f))
			{
				return RatioFrom;
			}
			float t = Mathf.Clamp01((GetLocalPosition(worldPosition).z + z * 0.5f) / z);
			return Mathf.Lerp(RatioFrom, RatioTo, t);
		}

		private Vector3 GetLocalPosition(Vector3 worldPosition)
		{
			return BoxCollider.transform.InverseTransformPoint(worldPosition) - BoxCollider.center;
		}
	}
}
