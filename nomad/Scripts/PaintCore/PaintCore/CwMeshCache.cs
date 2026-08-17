using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public static class CwMeshCache
	{
		private class MeshData
		{
			private Vector3[] positions;

			private int[] indices;

			private int total;

			public void Update(Mesh mesh)
			{
				positions = mesh.vertices;
				indices = mesh.triangles;
				total = indices.Length / 3;
			}

			public bool GetTrianglePositions(CwHit hit, ref Vector3 positionA, ref Vector3 positionB, ref Vector3 positionC)
			{
				int triangleIndex = hit.TriangleIndex;
				if (triangleIndex >= 0 && triangleIndex < total)
				{
					int num = triangleIndex * 3;
					Transform transform = hit.Transform;
					positionA = transform.TransformPoint(positions[indices[num]]);
					positionB = transform.TransformPoint(positions[indices[num + 1]]);
					positionC = transform.TransformPoint(positions[indices[num + 2]]);
					return true;
				}
				return false;
			}
		}

		private static Dictionary<Mesh, MeshData> cachedData = new Dictionary<Mesh, MeshData>();

		public static bool GetTrianglePositions(CwHit hit, ref Vector3 positionA, ref Vector3 positionB, ref Vector3 positionC)
		{
			MeshCollider meshCollider = hit.Collider as MeshCollider;
			if (meshCollider != null && !meshCollider.convex)
			{
				return GetTrianglePositions(meshCollider.sharedMesh, hit, ref positionA, ref positionB, ref positionC);
			}
			if (hit.Transform != null)
			{
				MeshFilter component = hit.Transform.GetComponent<MeshFilter>();
				if (component != null)
				{
					return GetTrianglePositions(component.sharedMesh, hit, ref positionA, ref positionB, ref positionC);
				}
			}
			return false;
		}

		private static bool GetTrianglePositions(Mesh mesh, CwHit hit, ref Vector3 positionA, ref Vector3 positionB, ref Vector3 positionC)
		{
			if (mesh != null)
			{
				MeshData value = null;
				if (!cachedData.TryGetValue(mesh, out value))
				{
					value = new MeshData();
					value.Update(mesh);
					cachedData.Add(mesh, value);
				}
				return value.GetTrianglePositions(hit, ref positionA, ref positionB, ref positionC);
			}
			return false;
		}
	}
}
