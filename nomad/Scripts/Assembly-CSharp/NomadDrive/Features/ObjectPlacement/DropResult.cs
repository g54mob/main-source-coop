using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public struct DropResult
	{
		public bool Success;

		public RaycastHit HitInfo;

		public SnappingPlane SnappingPlane;

		public Vector3 TargetPosition;

		public Quaternion TargetRotation;

		public static DropResult Failed => new DropResult
		{
			Success = false
		};
	}
}
