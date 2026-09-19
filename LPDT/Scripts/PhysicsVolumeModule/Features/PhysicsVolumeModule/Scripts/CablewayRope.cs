using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[ExecuteAlways]
	public class CablewayRope : MonoBehaviour
	{
		[SerializeField]
		private CablewayLift _cableway;

		[Tooltip("Where the rope meets the cabin — the hanger top. Falls back to the cabin position when unset.")]
		[SerializeField]
		private Transform _cabinAttach;

		[Tooltip("Rope tube for the pylonA→cabin leg.")]
		[SerializeField]
		private RopeTube _segmentA;

		[Tooltip("Rope tube for the cabin→pylonB leg.")]
		[SerializeField]
		private RopeTube _segmentB;

		[Header("Pylon anchors")]
		[Tooltip("Pylon head the cable hangs from at the PointA end. Falls back to the path end at hanger height.")]
		[SerializeField]
		private Transform _pylonAnchorA;

		[Tooltip("Pylon head the cable hangs from at the PointB end. Falls back to the path end at hanger height.")]
		[SerializeField]
		private Transform _pylonAnchorB;

		[Tooltip("Extra slack in metres on both pylon→lift legs. Set here rather than per segment so the two legs cannot drift apart. 0 leaves each leg's own sag alone.")]
		[SerializeField]
		[Min(0f)]
		private float _slack = 0.15f;

		private void LateUpdate()
		{
			if (!(_cableway == null))
			{
				Vector3 vector = ((_cabinAttach != null) ? _cabinAttach.position : _cableway.CabinPosition);
				ResolveEnds(vector, out var endA, out var endB);
				if (_segmentA != null)
				{
					_segmentA.SetSlack(_slack);
				}
				if (_segmentB != null)
				{
					_segmentB.SetSlack(_slack);
				}
				FitSegment(_segmentA, endA, vector);
				FitSegment(_segmentB, vector, endB);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (!(_cableway == null))
			{
				Vector3 vector = ((_cabinAttach != null) ? _cabinAttach.position : _cableway.CabinPosition);
				ResolveEnds(vector, out var endA, out var endB);
				Gizmos.color = new Color(1f, 0.55f, 0.1f);
				Gizmos.DrawLine(endA, vector);
				Gizmos.DrawLine(vector, endB);
				DrawAnchorGizmo(endA, vector);
				DrawAnchorGizmo(endB, vector);
			}
		}

		private void DrawAnchorGizmo(Vector3 anchor, Vector3 attach)
		{
			Gizmos.color = ((anchor.y > attach.y) ? Color.cyan : Color.red);
			Gizmos.DrawWireSphere(anchor, 0.15f);
		}

		private void ResolveEnds(Vector3 attach, out Vector3 endA, out Vector3 endB)
		{
			Vector3 vector = Vector3.up * (attach.y - _cableway.CabinPosition.y);
			endA = ((_pylonAnchorA != null) ? _pylonAnchorA.position : (_cableway.PathStart + vector));
			endB = ((_pylonAnchorB != null) ? _pylonAnchorB.position : (_cableway.PathEnd + vector));
		}

		private void FitSegment(RopeTube segment, Vector3 start, Vector3 end)
		{
			if (!(segment == null))
			{
				segment.SetEndpoints(start, end);
			}
		}
	}
}
