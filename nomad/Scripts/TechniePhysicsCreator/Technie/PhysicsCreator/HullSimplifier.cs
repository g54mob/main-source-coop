using System.Collections.Generic;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	public class HullSimplifier
	{
		public enum PlaneSelection
		{
			Arbitrary = 0,
			LowestDistance = 1,
			DisparateAngle = 2,
			CombinedDistanceAndAngle = 3,
			WeightedAngle = 4
		}

		public enum HoleFillMethod
		{
			ConnectEdges = 0,
			SortVertices = 1
		}

		private List<CutEdge> debugEdges = new List<CutEdge>();

		private List<CutEdge> debugUnconnectedEdges = new List<CutEdge>();

		private Face debugCutFace;

		private List<SortableVector3> debugSortableVertices = new List<SortableVector3>();

		private Plane debugClipPlane;

		private Vector3 debugFillCenter;

		private Vector3 debugClipTangent;

		private Face debugNormalFace;

		private List<Plane> debugAppliedPlanes = new List<Plane>();

		public Mesh Simplify(Mesh inputMesh, int maxPlanes, PlaneSelection planeSelection, HoleFillMethod holeFillMethod)
		{
			Mesh mesh = QHullUtil.FindConvexHull("test", inputMesh, showErrorInLog: true);
			List<Plane> list = new List<Plane>();
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 a = vertices[triangles[i]];
				Vector3 b = vertices[triangles[i + 1]];
				Vector3 c = vertices[triangles[i + 2]];
				Plane plane = new Plane(a, b, c);
				if (plane.normal.magnitude != 0f && !Contains(list, plane))
				{
					list.Add(plane);
				}
			}
			NgonHull ngonHull = NgonHull.FromBounds(mesh.bounds);
			List<Plane> list2 = new List<Plane>();
			list2.Add(new Plane(new Vector3(1f, 0f, 0f), 0f - mesh.bounds.extents.x));
			list2.Add(new Plane(new Vector3(-1f, 0f, 0f), 0f - mesh.bounds.extents.x));
			list2.Add(new Plane(new Vector3(0f, 1f, 0f), 0f - mesh.bounds.extents.y));
			list2.Add(new Plane(new Vector3(0f, -1f, 0f), 0f - mesh.bounds.extents.y));
			list2.Add(new Plane(new Vector3(0f, 0f, 1f), 0f - mesh.bounds.extents.z));
			list2.Add(new Plane(new Vector3(0f, 0f, -1f), 0f - mesh.bounds.extents.z));
			int num = 0;
			foreach (Plane item in list2)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (Approximately(item, list[j]))
					{
						list.RemoveAt(j);
						num++;
						break;
					}
				}
			}
			int num2 = 6;
			int num3 = 0;
			while (num2 < maxPlanes && list.Count > 0)
			{
				Plane clipPlane = PopNextPlane(list, list2, planeSelection);
				ngonHull = Clip(ngonHull, clipPlane, holeFillMethod);
				num2++;
				num3++;
			}
			int num4 = 0;
			for (int num5 = ngonHull.faces.Count - 1; num5 >= 0; num5--)
			{
				if (ngonHull.faces[num5].CalcArea() < 1E-05f)
				{
					ngonHull.faces.RemoveAt(num5);
					num4++;
				}
			}
			Mesh result = ngonHull.ToMesh();
			debugAppliedPlanes = list2;
			return result;
		}

		private static Plane PopNextPlane(List<Plane> possiblePlanes, List<Plane> appliedPlanes, PlaneSelection planeSelection)
		{
			Plane plane = default(Plane);
			switch (planeSelection)
			{
			case PlaneSelection.Arbitrary:
				plane = possiblePlanes[0];
				possiblePlanes.RemoveAt(0);
				break;
			case PlaneSelection.LowestDistance:
			{
				Plane plane8 = default(Plane);
				float num9 = -3.4028235E+38f;
				int index4 = -1;
				for (int l = 0; l < possiblePlanes.Count; l++)
				{
					Plane plane9 = possiblePlanes[l];
					float num10 = CalcDistanceRating(plane9, appliedPlanes);
					if (num10 > num9)
					{
						plane8 = plane9;
						num9 = num10;
						index4 = l;
					}
				}
				possiblePlanes.RemoveAt(index4);
				plane = plane8;
				break;
			}
			case PlaneSelection.DisparateAngle:
			{
				Plane plane4 = default(Plane);
				float num3 = -3.4028235E+38f;
				int index2 = -1;
				for (int j = 0; j < possiblePlanes.Count; j++)
				{
					Plane plane5 = possiblePlanes[j];
					float num4 = CalcAngleRating(plane5, appliedPlanes);
					if (num4 > num3)
					{
						plane4 = plane5;
						num3 = num4;
						index2 = j;
					}
				}
				possiblePlanes.RemoveAt(index2);
				plane = plane4;
				break;
			}
			case PlaneSelection.CombinedDistanceAndAngle:
			{
				Plane plane6 = default(Plane);
				float num5 = -3.4028235E+38f;
				int index3 = -1;
				for (int k = 0; k < possiblePlanes.Count; k++)
				{
					Plane plane7 = possiblePlanes[k];
					float num6 = CalcDistanceRating(plane7, appliedPlanes);
					float num7 = CalcAngleRating(plane7, appliedPlanes);
					float num8 = num6 * num7;
					if (num8 > num5)
					{
						plane6 = plane7;
						num5 = num8;
						index3 = k;
					}
				}
				possiblePlanes.RemoveAt(index3);
				plane = plane6;
				break;
			}
			case PlaneSelection.WeightedAngle:
			{
				Plane plane2 = default(Plane);
				float num = -3.4028235E+38f;
				int index = -1;
				for (int i = 0; i < possiblePlanes.Count; i++)
				{
					Plane plane3 = possiblePlanes[i];
					float num2 = CalcWeightedAngleRating(plane3, appliedPlanes);
					if (num2 > num)
					{
						plane2 = plane3;
						num = num2;
						index = i;
					}
				}
				possiblePlanes.RemoveAt(index);
				plane = plane2;
				break;
			}
			}
			appliedPlanes.Add(plane);
			return plane;
		}

		private static float CalcDistanceRating(Plane currentPlane, List<Plane> otherPlanes)
		{
			float num = -3.4028235E+38f;
			foreach (Plane otherPlane in otherPlanes)
			{
				if (otherPlane.distance > num)
				{
					num = otherPlane.distance;
				}
			}
			return 1f - Mathf.Clamp01(currentPlane.distance / num);
		}

		private static float CalcAngleRating(Plane currentPlane, List<Plane> otherPlanes)
		{
			float num = -3.4028235E+38f;
			foreach (Plane otherPlane in otherPlanes)
			{
				float num2 = Vector3.Dot(otherPlane.normal, currentPlane.normal);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return Mathf.Clamp01(1f - (num * 0.5f + 0.5f));
		}

		private static float CalcWeightedAngleRating(Plane currentPlane, List<Plane> otherPlanes)
		{
			float num = 0f;
			foreach (Plane otherPlane in otherPlanes)
			{
				float num2 = Vector3.Dot(otherPlane.normal, currentPlane.normal);
				float num3 = Mathf.Clamp01(1f - (num2 * 0.5f + 0.5f));
				num += num3;
			}
			return num / (float)otherPlanes.Count;
		}

		private NgonHull Clip(NgonHull inputHull, Plane clipPlane, HoleFillMethod holeFillMethod)
		{
			NgonHull ngonHull = new NgonHull();
			List<CutEdge> list = new List<CutEdge>();
			debugEdges.Clear();
			foreach (Face face3 in inputHull.faces)
			{
				Face face = new Face();
				int num = -1;
				for (int i = 0; i < face3.vertices.Count; i++)
				{
					Vector3 vector = face3.vertices[i];
					Vector3 vector2 = face3.vertices[(i + 1) % face3.vertices.Count];
					bool flag = !clipPlane.GetSide(vector);
					bool flag2 = !clipPlane.GetSide(vector2);
					if (flag && flag2)
					{
						face.vertices.Add(vector);
					}
					else if (flag || flag2)
					{
						if (flag && !flag2)
						{
							float weight;
							Vector3 item = CalcIntersection(vector, vector2, clipPlane, out weight);
							face.vertices.Add(vector);
							face.vertices.Add(item);
							num = face.vertices.Count - 1;
						}
						else if (!flag && flag2)
						{
							float weight2;
							Vector3 item2 = CalcIntersection(vector, vector2, clipPlane, out weight2);
							face.vertices.Add(item2);
						}
					}
				}
				if (face.vertices.Count >= 3)
				{
					ngonHull.faces.Add(face);
				}
				if (num != -1)
				{
					Vector3 v = face.vertices[num];
					Vector3 v2 = face.vertices[(num + 1) % face.vertices.Count];
					list.Add(new CutEdge(v, v2));
				}
			}
			Face face2 = null;
			switch (holeFillMethod)
			{
			case HoleFillMethod.ConnectEdges:
				face2 = FillHoleByConnectingEdges(clipPlane, list);
				break;
			case HoleFillMethod.SortVertices:
				face2 = FillHoleBySortingVertices(clipPlane, list);
				break;
			}
			if (face2 != null)
			{
				ngonHull.faces.Add(face2);
			}
			return ngonHull;
		}

		private static bool Contains(List<Plane> inputPlanes, Plane testPlane)
		{
			foreach (Plane inputPlane in inputPlanes)
			{
				if (Approximately(inputPlane, testPlane))
				{
					return true;
				}
			}
			return false;
		}

		private static bool Approximately(Plane lhs, Plane rhs)
		{
			float num = Vector3.Dot(lhs.normal, rhs.normal);
			float num2 = Mathf.Abs(lhs.distance - rhs.distance);
			if (num > 0.99f)
			{
				return num2 < 0.01f;
			}
			return false;
		}

		private Face FillHoleByConnectingEdges(Plane clipPlane, List<CutEdge> cutEdges)
		{
			if (cutEdges.Count < 3)
			{
				return null;
			}
			List<CutEdge> list = new List<CutEdge>(cutEdges);
			Face face = new Face();
			CutEdge cutEdge = cutEdges[0];
			cutEdges.RemoveAt(0);
			face.vertices.Add(cutEdge.v0);
			face.vertices.Add(cutEdge.v1);
			Vector3 pos = cutEdge.v1;
			int num = 0;
			Vector3 nextVertex;
			while (cutEdges.Count > 0 && PopNextVertex(pos, cutEdges, out nextVertex))
			{
				face.vertices.Add(nextVertex);
				pos = nextVertex;
				num++;
				if (num > 10000)
				{
					Debug.LogError("Unable to create face to cover cut hole!");
					face.vertices.Clear();
					break;
				}
			}
			if (face.vertices.Count >= 3)
			{
				if (Vector3.Distance(face.vertices[0], face.vertices[face.vertices.Count - 1]) < 0.0001f)
				{
					face.vertices.RemoveAt(face.vertices.Count - 1);
				}
				else
				{
					debugEdges = list;
					debugUnconnectedEdges = cutEdges;
					debugCutFace = face;
					Debug.LogError("Giving up trying to create cut face, originally " + list.Count + " cut edges, of which " + cutEdges.Count + " remain unconnected");
					foreach (CutEdge cutEdge2 in cutEdges)
					{
						Debug.LogError(string.Format("Remaining edge length: {0}", cutEdge2.Length.ToString("0.0000000")));
					}
				}
			}
			if (face.vertices.Count >= 3)
			{
				if (Vector3.Dot(Vector3.Cross(face.vertices[1] - face.vertices[0], face.vertices[2] - face.vertices[0]), clipPlane.normal) < 0f)
				{
					face.vertices.Reverse();
				}
				return face;
			}
			return null;
		}

		private Face FillHoleBySortingVertices(Plane clipPlane, List<CutEdge> cutEdges)
		{
			List<Vector3> list = new List<Vector3>();
			foreach (CutEdge cutEdge in cutEdges)
			{
				list.Add(cutEdge.v0);
				list.Add(cutEdge.v1);
			}
			List<Vector3> list2 = new List<Vector3>();
			foreach (Vector3 item in list)
			{
				bool flag = false;
				foreach (Vector3 item2 in list2)
				{
					if (Vector3.Distance(item, item2) < 1E-06f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list2.Add(item);
				}
			}
			if (list2.Count < 3)
			{
				return null;
			}
			Vector3 zero = Vector3.zero;
			foreach (Vector3 item3 in list2)
			{
				zero += item3;
			}
			zero /= (float)list2.Count;
			Matrix4x4 matrix4x = Matrix4x4.TRS(zero, Quaternion.LookRotation(clipPlane.normal), Vector3.one);
			Matrix4x4 inverse = matrix4x.inverse;
			List<SortableVector3> list3 = new List<SortableVector3>();
			foreach (Vector3 item4 in list2)
			{
				Vector3 vector = inverse.MultiplyPoint(item4);
				float s = Vector2.SignedAngle(new Vector2(vector.x, vector.y), Vector2.up);
				list3.Add(new SortableVector3(item4, s));
			}
			list3.Sort();
			Face face = new Face();
			foreach (SortableVector3 item5 in list3)
			{
				face.vertices.Add(item5.value);
			}
			if (Vector3.Dot(face.CalcNormal(), clipPlane.normal) < 0f)
			{
				face.vertices.Reverse();
			}
			debugSortableVertices = list3;
			debugClipPlane = clipPlane;
			debugFillCenter = zero;
			debugClipTangent = matrix4x.MultiplyVector(Vector3.up);
			debugNormalFace = face;
			return face;
		}

		public void RecalcFaceNormal()
		{
			debugNormalFace.CalcNormal();
		}

		private static bool PopNextVertex(Vector3 pos, List<CutEdge> edges, out Vector3 nextVertex)
		{
			for (int i = 0; i < edges.Count; i++)
			{
				CutEdge cutEdge = edges[i];
				if (Vector3.Distance(pos, cutEdge.v0) < 1.5E-05f)
				{
					edges.RemoveAt(i);
					nextVertex = cutEdge.v1;
					return true;
				}
				if (Vector3.Distance(pos, cutEdge.v1) < 1.5E-05f)
				{
					edges.RemoveAt(i);
					nextVertex = cutEdge.v0;
					return true;
				}
			}
			float num = 3.4028235E+38f;
			int num2 = -1;
			int num3 = -1;
			for (int j = 0; j < edges.Count; j++)
			{
				CutEdge cutEdge2 = edges[j];
				float num4 = Vector3.Distance(pos, cutEdge2.v0);
				float num5 = Vector3.Distance(pos, cutEdge2.v1);
				if (num4 < num)
				{
					num = num4;
					num2 = j;
					num3 = 0;
				}
				if (num5 < num)
				{
					num = num5;
					num2 = j;
					num3 = 1;
				}
			}
			Debug.LogWarning($"Couldn't find an unconnected edge to attach to current vertex - closest is edge {num2} {num3} - distance of {num} (threshold is {1.5E-05f})");
			nextVertex = Vector3.zero;
			return false;
		}

		private static Vector3 CalcIntersection(Vector3 v0, Vector3 v1, Plane plane, out float weight)
		{
			Vector3 vector = v1 - v0;
			float magnitude = vector.magnitude;
			Ray ray = new Ray(v0, vector / magnitude);
			plane.Raycast(ray, out var enter);
			Vector3 result = ray.origin + ray.direction * enter;
			weight = enter / magnitude;
			return result;
		}

		public void DrawGizmos(bool showDebugEdges, bool showUnconnectedEdges, bool showCutFace)
		{
			if (showDebugEdges && debugEdges != null)
			{
				foreach (CutEdge debugEdge in debugEdges)
				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(debugEdge.v0, debugEdge.v1);
					Gizmos.DrawSphere(debugEdge.v0, 0.001f);
					Gizmos.DrawSphere(debugEdge.v1, 0.001f);
				}
			}
			if (showUnconnectedEdges && debugUnconnectedEdges != null)
			{
				foreach (CutEdge debugUnconnectedEdge in debugUnconnectedEdges)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(debugUnconnectedEdge.v0, debugUnconnectedEdge.v1);
					Gizmos.DrawSphere(debugUnconnectedEdge.v0, 0.001f);
					Gizmos.DrawSphere(debugUnconnectedEdge.v1, 0.001f);
				}
			}
			if (showCutFace && debugCutFace != null)
			{
				foreach (Vector3 vertex in debugCutFace.vertices)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawSphere(vertex, 0.002f);
				}
			}
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(debugFillCenter, 0.01f);
			Gizmos.DrawLine(debugFillCenter, debugFillCenter + debugClipTangent * 0.1f);
			Gizmos.DrawLine(debugFillCenter, debugFillCenter + debugClipPlane.normal * 0.1f);
			if (debugNormalFace != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(debugNormalFace.vertices[0], debugNormalFace.vertices[0] + debugNormalFace.CalcNormal());
			}
			foreach (Plane debugAppliedPlane in debugAppliedPlanes)
			{
				Gizmos.color = Color.white;
				Vector3 vector = debugAppliedPlane.ClosestPointOnPlane(Vector3.zero);
				Gizmos.DrawSphere(vector, 0.01f);
				Gizmos.DrawLine(vector, vector + debugAppliedPlane.normal * 0.1f);
			}
		}
	}
}
