using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public struct ObjectPlacementData
	{
		public Vector3 PointOnPlane;

		public Vector3 Normal;

		public float PlacementShaderScale;

		public Vector3 BoundsCenterLocalOffset;

		public Vector3 PositionOffset;

		public Vector3 ConfigRotation;
	}
}
