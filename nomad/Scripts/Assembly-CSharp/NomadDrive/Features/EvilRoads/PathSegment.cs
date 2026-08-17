using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public struct PathSegment
	{
		public Vector3 StartPoint;

		public Vector3 EndPoint;

		public float Length;

		public float StartDistance;

		public bool HasPoiTarget;

		public Vector3 TargetPoi;

		public float PoiRadius;
	}
}
