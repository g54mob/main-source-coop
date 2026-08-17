using System.Collections.Generic;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	public class Face
	{
		public List<Vector3> vertices = new List<Vector3>();

		public static GameObject lastDebugObj;

		public Face()
		{
		}

		public Face(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			vertices.Add(p0);
			vertices.Add(p1);
			vertices.Add(p2);
			vertices.Add(p3);
		}

		public Vector3 CalcCenter()
		{
			Vector3 zero = Vector3.zero;
			foreach (Vector3 vertex in vertices)
			{
				zero += vertex;
			}
			return zero / vertices.Count;
		}

		public float CalcArea()
		{
			Vector3 p = CalcCenter();
			float num = 0f;
			for (int i = 0; i < vertices.Count; i++)
			{
				num += CalcTriangleArea(p, vertices[i], vertices[(i + 1) % vertices.Count]);
			}
			return num;
		}

		public Vector3 CalcNormal()
		{
			if (vertices.Count < 3)
			{
				Debug.LogError("Can't calc normal because face doesn't have enough vertices");
				return Vector3.up;
			}
			float a = 3.4028235E+38f;
			Vector3 result = Vector3.up;
			Vector3 vector = CalcCenter();
			Vector3 normalized = (vertices[0] - vector).normalized;
			for (int i = 1; i < vertices.Count; i++)
			{
				Vector3 normalized2 = (vertices[i] - vector).normalized;
				float b = Vector3.Dot(normalized, normalized2);
				if (Vector3.Dot(normalized, normalized2) < 0.9999f)
				{
					result = Vector3.Cross(normalized, normalized2).normalized;
					break;
				}
				a = Mathf.Min(a, b);
			}
			return result;
		}

		private static float CalcTriangleArea(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			Vector3 lhs = p1 - p0;
			Vector3 rhs = p2 - p0;
			return Vector3.Cross(lhs, rhs).magnitude * 0.5f;
		}
	}
}
