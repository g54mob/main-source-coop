using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StylizedWater2
{
	[Serializable]
	public class WaterMesh
	{
		public enum Shape
		{
			Rectangle = 0,
			Disk = 1
		}

		public Shape shape;

		[FormerlySerializedAs("size")]
		[Range(10f, 1000f)]
		public float scale = 100f;

		[Tooltip("Distance between vertices")]
		[Range(0.15f, 10f)]
		public float vertexDistance = 1f;

		public float UVTiling = 1f;

		[Tooltip("Shifts the vertices in a random direction. Definitely use this when using flat shading")]
		[Range(0f, 1f)]
		public float noise;

		[Min(0f)]
		[Tooltip("The surface is normally flat, yet vertex displacement on the GPU such as waves can give the surface artificial height.\n\nThis can cause a Mesh Renderer to be prematurely culled, despite still actually being visible.\n\nThis value adds an artificial amount of height to the generate mesh's bounds, to avoid this from happening.")]
		public float boundsPadding = 4f;

		public Mesh mesh;

		public Mesh Rebuild()
		{
			switch (shape)
			{
			case Shape.Rectangle:
				mesh = CreatePlane();
				break;
			case Shape.Disk:
				mesh = CreateCircle();
				break;
			}
			return mesh;
		}

		public static Mesh Create(Shape shape, float size, float vertexDistance, float uvTiling = 1f, float noise = 0f)
		{
			return new WaterMesh
			{
				shape = shape,
				scale = size,
				vertexDistance = vertexDistance,
				UVTiling = uvTiling,
				noise = noise
			}.Rebuild();
		}

		private int GetPointIndex(int c, int x)
		{
			if (c < 0)
			{
				return 0;
			}
			x %= (c + 1) * 6;
			return 3 * c * (c + 1) + x + 1;
		}

		private Mesh CreateCircle()
		{
			Mesh mesh = new Mesh();
			mesh.name = "WaterDisk";
			int num = Mathf.FloorToInt(scale / vertexDistance);
			float num2 = 1f / (float)num;
			List<Vector3> list = new List<Vector3>();
			List<Vector2> list2 = new List<Vector2>();
			List<Vector2> list3 = new List<Vector2>();
			list.Add(Vector3.zero);
			List<int> list4 = new List<int>();
			for (int i = 0; i < num; i++)
			{
				float num3 = MathF.PI * 2f / (float)((i + 1) * 6);
				for (int j = 0; j < (i + 1) * 6; j++)
				{
					Vector3 vector = new Vector3(Mathf.Sin(num3 * (float)j), 0f, Mathf.Cos(num3 * (float)j));
					UnityEngine.Random.InitState(i + j);
					vector.x += UnityEngine.Random.Range((0f - noise) * 0.01f, noise * 0.01f);
					vector.z -= UnityEngine.Random.Range(noise * 0.01f, (0f - noise) * 0.01f);
					list.Add(vector * (scale * 0.5f) * num2 * (i + 1));
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				list2.Add(new Vector2(0.5f + list[k].x * UVTiling, 0.5f + list[k].z * UVTiling));
				list3.Add(new Vector2(0.5f + list[k].x / scale, 0.5f + list[k].z / scale));
			}
			for (int l = 0; l < num; l++)
			{
				int m = 0;
				int num4 = 0;
				for (; m < (l + 1) * 6; m++)
				{
					if (m % (l + 1) != 0)
					{
						list4.Add(GetPointIndex(l - 1, num4 + 1));
						list4.Add(GetPointIndex(l - 1, num4));
						list4.Add(GetPointIndex(l, m));
						list4.Add(GetPointIndex(l, m));
						list4.Add(GetPointIndex(l, m + 1));
						list4.Add(GetPointIndex(l - 1, num4 + 1));
						num4++;
					}
					else
					{
						list4.Add(GetPointIndex(l, m));
						list4.Add(GetPointIndex(l, m + 1));
						list4.Add(GetPointIndex(l - 1, num4));
					}
				}
			}
			mesh.SetVertices(list);
			mesh.SetTriangles(list4, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.SetUVs(0, list2);
			mesh.SetUVs(1, list3);
			mesh.colors = new Color[list.Count];
			mesh.bounds = new Bounds(Vector3.zero, new Vector3(scale, boundsPadding, scale));
			return mesh;
		}

		private Mesh CreatePlane()
		{
			Mesh mesh = new Mesh();
			mesh.name = "WaterPlane";
			scale = Mathf.Max(1f, scale);
			int num = Mathf.FloorToInt(scale / vertexDistance);
			int num2 = num + 1;
			int num3 = num + 1;
			int num4 = num * num * 6;
			int num5 = num2 * num3;
			Vector3[] array = new Vector3[num5];
			Vector2[] array2 = new Vector2[num5];
			Vector2[] array3 = new Vector2[num5];
			int[] array4 = new int[num4];
			Vector4[] array5 = new Vector4[num5];
			Vector3[] array6 = new Vector3[num5];
			Vector4 vector = new Vector4(1f, 0f, 0f, -1f);
			int num6 = 0;
			float num7 = scale / (float)num;
			float num8 = scale / (float)num;
			float num9 = vertexDistance * 0.5f;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array[num6] = new Vector3((float)j * num7 - scale * 0.5f, 0f, (float)i * num8 - scale * 0.5f);
					UnityEngine.Random.InitState(num6);
					array[num6].x += UnityEngine.Random.Range((0f - noise) * num9, noise * num9);
					array[num6].z -= UnityEngine.Random.Range(noise * num9, (0f - noise) * num9);
					array5[num6] = vector;
					array2[num6] = new Vector2(0.5f + array[num6].x * UVTiling, 0.5f + array[num6].z * UVTiling);
					array3[num6] = new Vector2(0.5f + array[num6].x / scale, 0.5f + array[num6].z / scale);
					array6[num6] = Vector3.up;
					num6++;
				}
			}
			num6 = 0;
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num; l++)
				{
					array4[num6] = k * num2 + l;
					array4[num6 + 1] = (k + 1) * num2 + l;
					array4[num6 + 2] = k * num2 + l + 1;
					array4[num6 + 3] = (k + 1) * num2 + l;
					array4[num6 + 4] = (k + 1) * num2 + l + 1;
					array4[num6 + 5] = k * num2 + l + 1;
					num6 += 6;
				}
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			mesh.uv = array2;
			mesh.uv2 = array3;
			mesh.tangents = array5;
			mesh.normals = array6;
			mesh.colors = new Color[array.Length];
			mesh.bounds = new Bounds(Vector3.zero, new Vector3(scale, boundsPadding, scale));
			return mesh;
		}
	}
}
