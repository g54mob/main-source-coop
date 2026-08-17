using System;
using UnityEngine;

namespace NWH.WheelController3D
{
	public static class WheelControllerUtility
	{
		public static Mesh CreateCylinderMesh(int subdivisions, float height, float radius)
		{
			Mesh mesh = new Mesh();
			Vector3[] array = new Vector3[(subdivisions + 1) * 2];
			int[] array2 = new int[subdivisions * 6];
			float num = (float)Math.PI * 2f / (float)subdivisions;
			for (int i = 0; i <= subdivisions; i++)
			{
				float f = (float)i * num;
				float y = Mathf.Cos(f) * radius;
				float z = Mathf.Sin(f) * radius;
				array[i] = new Vector3((0f - height) / 2f, y, z);
				array[i + subdivisions + 1] = new Vector3(height / 2f, y, z);
			}
			for (int j = 0; j < subdivisions; j++)
			{
				int num2 = subdivisions + 1;
				array2[j * 6] = j;
				array2[j * 6 + 1] = (j + 1) % num2;
				array2[j * 6 + 2] = 0;
				array2[j * 6 + 3] = j + num2;
				array2[j * 6 + 4] = (j + 1) % num2 + num2;
				array2[j * 6 + 5] = num2;
			}
			mesh.vertices = array;
			mesh.triangles = array2;
			return mesh;
		}
	}
}
