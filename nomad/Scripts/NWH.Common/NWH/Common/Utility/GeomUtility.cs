using UnityEngine;

namespace NWH.Common.Utility
{
	public static class GeomUtility
	{
		public static bool NearEqual(this Vector3 a, Vector3 b, float threshold = 0.01f)
		{
			return Vector3.SqrMagnitude(a - b) < threshold;
		}

		public static bool Equal(this Quaternion a, Quaternion b)
		{
			return Mathf.Abs(Quaternion.Angle(a, b)) < 0.1f;
		}

		public static Vector3 RoundedMax(this Vector3 v)
		{
			int num = -1;
			float num2 = -1f / 0f;
			for (int i = 0; i < 3; i++)
			{
				float num3 = Mathf.Abs(v[i]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				v[j] = ((j == num) ? (Mathf.Sign(v[j]) * 1f) : 0f);
			}
			return v;
		}

		public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
		{
			lineDir.Normalize();
			float num = Vector3.Dot(pnt - linePnt, lineDir);
			return linePnt + lineDir * num;
		}

		public static float FindDistanceToSegment(Vector3 pt, Vector3 p1, Vector3 p2)
		{
			float num = p2.x - p1.x;
			float num2 = p2.y - p1.y;
			if (num == 0f && num2 == 0f)
			{
				num = pt.x - p1.x;
				num2 = pt.y - p1.y;
				return Mathf.Sqrt(num * num + num2 * num2);
			}
			float num3 = ((pt.x - p1.x) * num + (pt.y - p1.y) * num2) / (num * num + num2 * num2);
			if (num3 < 0f)
			{
				num = pt.x - p1.x;
				num2 = pt.y - p1.y;
			}
			else if (num3 > 1f)
			{
				num = pt.x - p2.x;
				num2 = pt.y - p2.y;
			}
			else
			{
				Vector3 vector = new Vector3(p1.x + num3 * num, p1.y + num3 * num2);
				num = pt.x - vector.x;
				num2 = pt.y - vector.y;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public static float SquareDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.y - b.y;
			float num3 = a.z - b.z;
			return num * num + num2 * num2 + num3 * num3;
		}

		public static Vector3 LinePlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection)
		{
			if (Vector3.Dot(planeNormal, lineDirection.normalized) == 0f)
			{
				return Vector3.zero;
			}
			float num = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / Vector3.Dot(planeNormal, lineDirection.normalized);
			return linePoint + lineDirection.normalized * num;
		}

		public static Vector3 FindChordLine(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float chordPercent)
		{
			return QuadLerp(a, b, c, d, 0.5f, chordPercent);
		}

		public static Vector3 FindSpanLine(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float spanPercent)
		{
			return QuadLerp(a, b, c, d, spanPercent, 0.5f);
		}

		public static float FindArea(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
		{
			return TriArea(A, B, D) + TriArea(B, C, D);
		}

		public static Vector3 FindCenter(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			if (a == d)
			{
				return (a + b + c) / 4f;
			}
			return (a + b + c + d) / 4f;
		}

		public static float DistanceAlongNormal(Vector3 a, Vector3 b, Vector3 normal)
		{
			return Vector3.Project(b - a, normal).magnitude;
		}

		public static bool PointInTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P, float dotThreshold = 0.001f)
		{
			if (SameSide(P, A, B, C) && SameSide(P, B, A, C) && SameSide(P, C, A, B))
			{
				Vector3 normalized = Vector3.Cross(B - A, C - A).normalized;
				if (Mathf.Abs(Vector3.Dot(P - A, normalized)) <= dotThreshold)
				{
					return true;
				}
			}
			return false;
		}

		private static bool SameSide(Vector3 p1, Vector3 p2, Vector3 A, Vector3 B)
		{
			Vector3 normalized = Vector3.Cross(B - A, p1 - A).normalized;
			Vector3 normalized2 = Vector3.Cross(B - A, p2 - A).normalized;
			if (Vector3.Dot(normalized, normalized2) > 0f)
			{
				return true;
			}
			return false;
		}

		public static bool PointIsInsideRect(Vector2 point)
		{
			return new Rect(0f, 0f, Screen.width, Screen.height).Contains(point);
		}

		public static bool NearlyEqual(this float a, float b, double epsilon)
		{
			return (double)Mathf.Abs(a - b) < epsilon;
		}

		public static float AreaFromThreePoints(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			Vector3 lhs = default(Vector3);
			lhs.x = p2.x - p1.x;
			lhs.y = p2.y - p1.y;
			lhs.z = p2.z - p1.z;
			Vector3 rhs = default(Vector3);
			rhs.x = p3.x - p1.x;
			rhs.y = p3.y - p1.y;
			rhs.z = p3.z - p1.z;
			Vector3 vector = Vector3.Cross(lhs, rhs);
			return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z) * 0.5f;
		}

		public static float AreaFromFourPoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
		{
			return AreaFromThreePoints(p1, p2, p4) + AreaFromThreePoints(p2, p3, p4);
		}

		public static float TriArea(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			Vector3 lhs = default(Vector3);
			lhs.x = p2.x - p1.x;
			lhs.y = p2.y - p1.y;
			lhs.z = p2.z - p1.z;
			Vector3 rhs = default(Vector3);
			rhs.x = p3.x - p1.x;
			rhs.y = p3.y - p1.y;
			rhs.z = p3.z - p1.z;
			Vector3 vector = Vector3.Cross(lhs, rhs);
			return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z) * 0.5f;
		}

		public static float MeshArea(Mesh mesh)
		{
			if (mesh.vertices.Length == 0)
			{
				return 0f;
			}
			float num = 0f;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				num += TriArea(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
			}
			return num;
		}

		public static float ProjectedMeshArea(Mesh mesh, Vector3 direction)
		{
			float num = 0f;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			_ = mesh.normals;
			int num2 = 0;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				num += TriArea(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]], direction);
				num2++;
			}
			return num;
		}

		public static float RectArea(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
		{
			return TriArea(p1, p2, p4) + TriArea(p2, p3, p4);
		}

		public static Vector3 FindMeshCenter(Mesh mesh)
		{
			if (mesh.vertices.Length == 0)
			{
				return Vector3.zero;
			}
			Vector3 zero = Vector3.zero;
			int num = 0;
			if (mesh != null)
			{
				Vector3[] vertices = mesh.vertices;
				foreach (Vector3 vector in vertices)
				{
					zero += vector;
					num++;
				}
			}
			return zero / num;
		}

		public static float TriArea(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 view)
		{
			Vector3 lhs = default(Vector3);
			lhs.x = p2.x - p1.x;
			lhs.y = p2.y - p1.y;
			lhs.z = p2.z - p1.z;
			Vector3 rhs = default(Vector3);
			rhs.x = p3.x - p1.x;
			rhs.y = p3.y - p1.y;
			rhs.z = p3.z - p1.z;
			Vector3 vector = Vector3.Cross(lhs, rhs);
			float num = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
			Vector3 vector2 = default(Vector3);
			if (num == 0f)
			{
				vector2.x = (vector2.y = (vector2.z = 0f));
			}
			else
			{
				vector2.x = vector.x / num;
				vector2.y = vector.y / num;
				vector2.z = vector.z / num;
			}
			float num2 = Mathf.Cos(Vector3.Angle(vector2, view));
			if (num2 < 0f)
			{
				return 0f;
			}
			return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z) * 0.5f * num2;
		}

		public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = p3.x * p2.y * p1.z;
			float num2 = p2.x * p3.y * p1.z;
			float num3 = p3.x * p1.y * p2.z;
			float num4 = p1.x * p3.y * p2.z;
			float num5 = p2.x * p1.y * p3.z;
			float num6 = p1.x * p2.y * p3.z;
			return 1f / 6f * (0f - num + num2 + num3 - num4 - num5 + num6);
		}

		public static float VolumeOfMesh(Mesh mesh)
		{
			float num = 0f;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			for (int i = 0; i < mesh.triangles.Length; i += 3)
			{
				Vector3 p = vertices[triangles[i]];
				Vector3 p2 = vertices[triangles[i + 1]];
				Vector3 p3 = vertices[triangles[i + 2]];
				num += SignedVolumeOfTriangle(p, p2, p3);
			}
			return Mathf.Abs(num);
		}

		public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position)
		{
			return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).MultiplyPoint3x4(position);
		}

		public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position)
		{
			return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse.MultiplyPoint3x4(position);
		}

		public static void ChangeLayersRecursively(this Transform trans, string name)
		{
			trans.gameObject.layer = LayerMask.NameToLayer(name);
			foreach (Transform tran in trans)
			{
				tran.ChangeLayersRecursively(name);
			}
		}

		public static void ChangeObjectColor(GameObject gameObject, Color color)
		{
			gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
		}

		public static void ChangeObjectAlpha(GameObject gameObject, float alpha)
		{
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			Color color = component.material.GetColor("_Color");
			color.a = alpha;
			component.material.SetColor("_Color", color);
		}

		public static Vector3 Vector3Abs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		public static Vector3 Vector3RoundToInt(Vector3 v)
		{
			return new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		public static Vector3 Vector3OneOver(Vector3 v)
		{
			return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
		}

		public static float RoundToStep(float value, float step)
		{
			return Mathf.Round(value / step) * step;
		}

		public static float RoundToStep(int value, int step)
		{
			return Mathf.RoundToInt(Mathf.Round(value / step) * (float)step);
		}

		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
		{
			return Quaternion.Euler(angles) * (point - pivot) + pivot;
		}

		public static Vector3 QuadLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u, float v)
		{
			Vector3 v2 = Vector3Lerp(a, b, u);
			Vector3 v3 = Vector3Lerp(d, c, u);
			return Vector3Lerp(v2, v3, v);
		}

		public static Vector3 Vector3Lerp(Vector3 v1, Vector3 v2, float value)
		{
			if (value > 1f)
			{
				return v2;
			}
			if (value < 0f)
			{
				return v1;
			}
			return new Vector3(v1.x + (v2.x - v1.x) * value, v1.y + (v2.y - v1.y) * value, v1.z + (v2.z - v1.z) * value);
		}

		public static float QuaternionMagnitude(Quaternion q)
		{
			return Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
		}
	}
}
