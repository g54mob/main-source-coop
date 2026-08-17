using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.Splines;

namespace NomadDrive.Features.EvilRoads
{
	public static class RoadSplineMerger
	{
		public static Spline MergeSplines(RoadConnectionData connectionData)
		{
			RoadConnectionPoint connectionPoint = connectionData.ConnectionPoint1;
			RoadConnectionPoint connectionPoint2 = connectionData.ConnectionPoint2;
			EvilRoad ownerRoad = connectionPoint.OwnerRoad;
			EvilRoad ownerRoad2 = connectionPoint2.OwnerRoad;
			if (ownerRoad == null || ownerRoad2 == null)
			{
				EvilLogger.LogError("Cannot merge splines: one or both roads are null", "MergeSplines", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadSplineMerger.cs", 26);
				return null;
			}
			Spline spline = ownerRoad.GetSpline();
			Spline spline2 = ownerRoad2.GetSpline();
			if (spline == null || spline2 == null)
			{
				EvilLogger.LogError("Cannot merge splines: one or both splines are null", "MergeSplines", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadSplineMerger.cs", 35);
				return null;
			}
			return MergeSplines(spline, spline2, connectionPoint, connectionPoint2);
		}

		private static Spline MergeSplines(Spline spline1, Spline spline2, RoadConnectionPoint point1, RoadConnectionPoint point2)
		{
			Spline spline3 = new Spline();
			List<BezierKnot> list = new List<BezierKnot>();
			if (!point1.IsStartPoint && point2.IsStartPoint)
			{
				for (int i = 0; i < spline1.Count; i++)
				{
					list.Add(spline1[i]);
				}
				for (int j = 1; j < spline2.Count; j++)
				{
					list.Add(spline2[j]);
				}
			}
			else if (point1.IsStartPoint && !point2.IsStartPoint)
			{
				for (int k = 0; k < spline2.Count; k++)
				{
					list.Add(spline2[k]);
				}
				for (int l = 1; l < spline1.Count; l++)
				{
					list.Add(spline1[l]);
				}
			}
			else
			{
				for (int m = 0; m < spline1.Count; m++)
				{
					list.Add(spline1[m]);
				}
				for (int n = 1; n < spline2.Count; n++)
				{
					list.Add(spline2[n]);
				}
			}
			foreach (BezierKnot item in list)
			{
				spline3.Add(item);
			}
			spline3.Closed = false;
			if (spline3.Count != spline1.Count + spline2.Count - 1)
			{
				EvilLogger.LogError($"MERGE FAILED! Expected {spline1.Count + spline2.Count - 1} knots but got {spline3.Count}", "MergeSplines", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\RoadSplineMerger.cs", 107);
			}
			return spline3;
		}

		private static void AddKnotsFromSplinePreserveShape(Spline spline, RoadConnectionPoint connectionPoint, List<BezierKnot> orderedKnots, bool isFirstSpline)
		{
			if (spline.Count == 0)
			{
				return;
			}
			List<BezierKnot> list = new List<BezierKnot>();
			for (int i = 0; i < spline.Count; i++)
			{
				list.Add(spline[i]);
			}
			if (connectionPoint.IsStartPoint)
			{
				if (isFirstSpline)
				{
					list.Reverse();
				}
			}
			else if (!isFirstSpline)
			{
				list.Reverse();
			}
			for (int j = 0; j < list.Count; j++)
			{
				if ((!isFirstSpline || j != list.Count - 1) && (isFirstSpline || j != 0))
				{
					orderedKnots.Add(list[j]);
				}
			}
		}

		private static void AddKnotsFromSpline(Spline spline, RoadConnectionPoint connectionPoint, List<BezierKnot> orderedKnots, bool isFirstSpline)
		{
			if (spline.Count == 0)
			{
				return;
			}
			List<BezierKnot> list = new List<BezierKnot>();
			for (int i = 0; i < spline.Count; i++)
			{
				list.Add(spline[i]);
			}
			if (connectionPoint.IsStartPoint)
			{
				if (isFirstSpline)
				{
					list.Reverse();
				}
			}
			else if (!isFirstSpline)
			{
				list.Reverse();
			}
			for (int j = 0; j < list.Count; j++)
			{
				if ((!isFirstSpline || j != list.Count - 1) && (isFirstSpline || j != 0))
				{
					orderedKnots.Add(list[j]);
				}
			}
		}

		private static BezierKnot CreateConnectionKnot(RoadConnectionPoint point1, RoadConnectionPoint point2, Vector3 connectionPos)
		{
			Vector3 vector = -point1.Direction.normalized * 2f;
			Vector3 vector2 = point2.Direction.normalized * 2f;
			return new BezierKnot(connectionPos, vector, vector2);
		}

		public static bool CanMergeSplines(Spline spline1, Spline spline2)
		{
			if (spline1 != null && spline2 != null && spline1.Count > 0)
			{
				return spline2.Count > 0;
			}
			return false;
		}

		public static float GetMergedSplineLength(RoadConnectionData connectionData)
		{
			RoadConnectionPoint connectionPoint = connectionData.ConnectionPoint1;
			RoadConnectionPoint connectionPoint2 = connectionData.ConnectionPoint2;
			EvilRoad ownerRoad = connectionPoint.OwnerRoad;
			EvilRoad ownerRoad2 = connectionPoint2.OwnerRoad;
			if (ownerRoad == null || ownerRoad2 == null)
			{
				return 0f;
			}
			Spline spline = ownerRoad.GetSpline();
			Spline spline2 = ownerRoad2.GetSpline();
			if (spline == null || spline2 == null)
			{
				return 0f;
			}
			return spline.GetLength() + spline2.GetLength();
		}

		private static void ApplyMinimalConnectionSmoothing(Spline spline)
		{
			if (spline != null && spline.Count >= 3)
			{
				int num = spline.Count / 2;
				int num2 = 1;
				int num3 = Mathf.Max(1, num - num2);
				int num4 = Mathf.Min(spline.Count - 2, num + num2);
				for (int i = num3; i <= num4; i++)
				{
					BezierKnot value = spline[i];
					Vector3 vector = spline[i - 1].Position;
					Vector3 vector2 = spline[i + 1].Position;
					Vector3 normalized = (vector2 - vector).normalized;
					float num5 = Vector3.Distance(vector, vector2) * 0.25f;
					value.TangentIn = -normalized * num5;
					value.TangentOut = normalized * num5;
					spline[i] = value;
				}
			}
		}

		public static void OptimizeSpline(Spline spline, float simplificationTolerance = 0.05f)
		{
			if (spline == null || spline.Count < 3)
			{
				return;
			}
			for (int num = spline.Count - 1; num > 0; num--)
			{
				if (Vector3.Distance(spline[num].Position, spline[num - 1].Position) < simplificationTolerance)
				{
					spline.RemoveAt(num);
				}
			}
		}
	}
}
