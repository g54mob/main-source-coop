using System.Collections.Generic;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	public class NgonHull
	{
		public List<Face> faces = new List<Face>();

		public static NgonHull FromBounds(Bounds bounds)
		{
			Vector3[] array = new Vector3[8]
			{
				new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
				new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
				new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
				new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
				new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
				new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
				new Vector3(bounds.max.x, bounds.max.y, bounds.max.z),
				new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)
			};
			return new NgonHull
			{
				faces = 
				{
					new Face(array[0], array[1], array[2], array[3]),
					new Face(array[7], array[6], array[5], array[4]),
					new Face(array[5], array[6], array[2], array[1]),
					new Face(array[7], array[4], array[0], array[3]),
					new Face(array[6], array[7], array[3], array[2]),
					new Face(array[4], array[5], array[1], array[0])
				}
			};
		}

		public Mesh ToMesh()
		{
			Mesh mesh = new Mesh();
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<int> list3 = new List<int>();
			int num = 0;
			for (int i = 0; i < faces.Count; i++)
			{
				Face face = faces[i];
				Vector3 item = face.CalcNormal();
				list.Add(face.CalcCenter());
				list2.Add(item);
				for (int j = 0; j < face.vertices.Count; j++)
				{
					list.Add(face.vertices[j]);
					list2.Add(item);
				}
				for (int k = 0; k < face.vertices.Count; k++)
				{
					list3.Add(num);
					list3.Add(num + k + 1);
					list3.Add(num + (k + 1) % face.vertices.Count + 1);
				}
				num += face.vertices.Count + 1;
			}
			mesh.vertices = list.ToArray();
			mesh.normals = list2.ToArray();
			mesh.triangles = list3.ToArray();
			return mesh;
		}
	}
}
