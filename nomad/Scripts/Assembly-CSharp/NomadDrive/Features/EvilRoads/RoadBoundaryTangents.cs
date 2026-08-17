using UnityEngine;

namespace NomadDrive.Features.EvilRoads
{
	public readonly struct RoadBoundaryTangents
	{
		public readonly Vector3 StartDir;

		public readonly Vector3 EndDir;

		public readonly bool HasValue;

		public RoadBoundaryTangents(Vector3 startDir, Vector3 endDir)
		{
			StartDir = startDir;
			EndDir = endDir;
			HasValue = true;
		}
	}
}
