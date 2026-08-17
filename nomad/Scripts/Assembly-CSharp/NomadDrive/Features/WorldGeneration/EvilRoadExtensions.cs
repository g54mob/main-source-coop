using NomadDrive.Features.EvilRoads;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.WorldGeneration
{
	public static class EvilRoadExtensions
	{
		public static Vector3 GetStartPoint(this EvilRoad road)
		{
			Spline spline = road.GetSpline();
			if (spline != null && spline.Count > 0)
			{
				Vector3 position = spline[0].Position;
				Vector3 result = road.transform.TransformPoint(position);
				Vector3 position2 = spline[spline.Count - 1].Position;
				Vector3 result2 = road.transform.TransformPoint(position2);
				if (result.z < result2.z)
				{
					return result;
				}
				return result2;
			}
			return Vector3.zero;
		}

		public static Vector3 GetEndPoint(this EvilRoad road)
		{
			Spline spline = road.GetSpline();
			if (spline != null && spline.Count > 0)
			{
				Vector3 position = spline[spline.Count - 1].Position;
				Vector3 result = road.transform.TransformPoint(position);
				Vector3 position2 = spline[0].Position;
				Vector3 result2 = road.transform.TransformPoint(position2);
				if (result.z > result2.z)
				{
					return result;
				}
				return result2;
			}
			return Vector3.zero;
		}
	}
}
