using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class MeshWrapper
	{
		public Vector3[] verts;

		public Vector3[] normals;

		public Vector2[] uv;

		public Vector2[] uv2;

		public Vector2[] uv3;

		public Vector2[] uv4;

		public Color[] colors;

		public Vector4[] tangents;

		public int[] tris;

		[NonSerialized]
		public int vertCounter;

		[NonSerialized]
		public int triCounter;

		public MeshWrapper()
		{
		}

		public MeshWrapper(MeshWrapper src)
		{
			if (src.verts != null)
			{
				verts = new Vector3[src.verts.Length];
				Array.Copy(src.verts, verts, verts.Length);
			}
			if (src.normals != null)
			{
				normals = new Vector3[src.normals.Length];
				Array.Copy(src.normals, normals, normals.Length);
			}
			if (src.uv != null)
			{
				uv = new Vector2[src.uv.Length];
				Array.Copy(src.uv, uv, uv.Length);
			}
			if (src.uv2 != null)
			{
				uv2 = new Vector2[src.uv2.Length];
				Array.Copy(src.uv2, uv2, uv2.Length);
			}
			if (src.uv3 != null)
			{
				uv3 = new Vector2[src.uv3.Length];
				Array.Copy(src.uv3, uv3, uv3.Length);
			}
			if (src.uv4 != null)
			{
				uv4 = new Vector2[src.uv4.Length];
				Array.Copy(src.uv4, uv4, uv4.Length);
			}
			if (src.colors != null)
			{
				colors = new Color[src.colors.Length];
				Array.Copy(src.colors, colors, colors.Length);
			}
			if (src.tangents != null)
			{
				tangents = new Vector4[src.tangents.Length];
				Array.Copy(src.tangents, tangents, tangents.Length);
			}
			if (src.tris != null)
			{
				tris = new int[src.tris.Length];
				Array.Copy(src.tris, tris, tris.Length);
			}
		}

		public void Restart()
		{
			vertCounter = 0;
			triCounter = 0;
		}

		public void SetChannels(byte[] channels)
		{
			int num = channels.Length / 8;
			uv = new Vector2[num];
			uv2 = new Vector2[num];
			uv3 = new Vector2[num];
			uv4 = new Vector2[num];
			for (int i = 0; i < num; i++)
			{
				uv[i] = new Vector2((float)(int)channels[i * 8 + 1] / 32f, (float)(int)channels[i * 8 + 2] / 32f);
				uv2[i] = new Vector2((float)(int)channels[i * 8 + 3] / 32f, (float)(int)channels[i * 8 + 4] / 32f);
				uv3[i] = new Vector2((float)(int)channels[i * 8 + 5] / 32f, (float)(int)channels[i * 8 + 6] / 32f);
				uv4[i] = new Vector2((float)(int)channels[i * 8 + 7] / 32f, (float)(int)channels[i * 8 + 7] / 32f);
			}
		}

		public void ApplyTo(Mesh mesh)
		{
			mesh.Clear();
			mesh.vertices = verts;
			mesh.normals = normals;
			if (uv != null)
			{
				mesh.uv = uv;
			}
			if (uv2 != null)
			{
				mesh.uv2 = uv2;
			}
			if (uv3 != null)
			{
				mesh.uv3 = uv3;
			}
			if (uv4 != null)
			{
				mesh.uv4 = uv4;
			}
			if (colors != null)
			{
				mesh.colors = colors;
			}
			if (tangents != null)
			{
				mesh.tangents = tangents;
			}
			mesh.SetTriangles(tris, 0, calculateBounds: false);
		}

		public void ReadMesh(Mesh mesh)
		{
			verts = mesh.vertices;
			normals = mesh.normals;
			uv = mesh.uv;
			uv2 = mesh.uv2;
			uv3 = mesh.uv3;
			uv4 = mesh.uv4;
			tangents = mesh.tangents;
			colors = mesh.colors;
			tris = mesh.triangles;
			if (normals != null && normals.Length == 0)
			{
				normals = null;
			}
			if (tangents != null && tangents.Length == 0)
			{
				tangents = null;
			}
			if (colors != null && colors.Length == 0)
			{
				colors = null;
			}
			if (uv != null && uv.Length == 0)
			{
				uv = null;
			}
			if (uv2 != null && uv2.Length == 0)
			{
				uv2 = null;
			}
			if (uv3 != null && uv3.Length == 0)
			{
				uv3 = null;
			}
			if (uv4 != null && uv4.Length == 0)
			{
				uv4 = null;
			}
		}

		public void Append(MeshWrapper addMesh, Vector3 offset = default(Vector3), float size = 1f, float height = 1f)
		{
			if (normals != null && uv != null && addMesh.normals != null && normals.Length != 0 && addMesh.normals.Length != 0 && addMesh.uv != null && uv.Length != 0 && addMesh.uv.Length != 0)
			{
				for (int i = 0; i < addMesh.verts.Length; i++)
				{
					int num = vertCounter + i;
					verts[num] = new Vector3(addMesh.verts[i].x * size + offset.x, addMesh.verts[i].y * size * height + offset.y, addMesh.verts[i].z * size + offset.z);
					normals[num] = addMesh.normals[i];
					uv[num] = addMesh.uv[i];
				}
			}
			else
			{
				for (int j = 0; j < addMesh.verts.Length; j++)
				{
					verts[vertCounter + j] = new Vector3(addMesh.verts[j].x * size + offset.x, addMesh.verts[j].y * size * height + offset.y, addMesh.verts[j].z * size + offset.z);
				}
				if (normals != null && addMesh.normals != null && normals.Length != 0 && addMesh.normals.Length != 0)
				{
					for (int k = 0; k < addMesh.normals.Length; k++)
					{
						normals[vertCounter + k] = addMesh.normals[k];
					}
				}
				if (uv != null && addMesh.uv != null && uv.Length != 0 && addMesh.uv.Length != 0)
				{
					for (int l = 0; l < addMesh.verts.Length; l++)
					{
						uv[vertCounter + l] = addMesh.uv[l];
					}
				}
			}
			if (uv2 != null && addMesh.uv2 != null && uv2.Length != 0 && addMesh.uv2.Length != 0)
			{
				for (int m = 0; m < addMesh.verts.Length; m++)
				{
					uv2[vertCounter + m] = addMesh.uv2[m];
				}
			}
			if (uv3 != null && addMesh.uv3 != null && uv3.Length != 0 && addMesh.uv3.Length != 0)
			{
				for (int n = 0; n < addMesh.verts.Length; n++)
				{
					uv3[vertCounter + n] = addMesh.uv3[n];
				}
			}
			if (uv4 != null && addMesh.uv4 != null && uv4.Length != 0 && addMesh.uv4.Length != 0)
			{
				for (int num2 = 0; num2 < addMesh.verts.Length; num2++)
				{
					uv4[vertCounter + num2] = addMesh.uv4[num2];
				}
			}
			if (uv != null && addMesh.uv != null && uv.Length != 0 && addMesh.uv.Length != 0)
			{
				for (int num3 = 0; num3 < addMesh.verts.Length; num3++)
				{
					uv[vertCounter + num3] = addMesh.uv[num3];
				}
			}
			if (colors != null && addMesh.colors != null && colors.Length != 0 && addMesh.colors.Length != 0)
			{
				for (int num4 = 0; num4 < addMesh.verts.Length; num4++)
				{
					colors[vertCounter + num4] = addMesh.colors[num4];
				}
			}
			if (tangents != null && addMesh.tangents != null && tangents.Length != 0 && addMesh.tangents.Length != 0)
			{
				for (int num5 = 0; num5 < addMesh.verts.Length; num5++)
				{
					tangents[vertCounter + num5] = addMesh.tangents[num5];
				}
			}
			for (int num6 = 0; num6 < addMesh.tris.Length; num6++)
			{
				tris[triCounter + num6] = addMesh.tris[num6] + vertCounter;
			}
			vertCounter += addMesh.verts.Length;
			triCounter += addMesh.tris.Length;
		}

		public void RotateMirror(int rotation, bool mirror)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			switch (rotation)
			{
			case 90:
				flag3 = true;
				flag = true;
				break;
			case 180:
				flag = true;
				flag2 = true;
				break;
			case 270:
				flag3 = true;
				flag2 = true;
				break;
			}
			if (mirror)
			{
				flag = !flag;
			}
			for (int i = 0; i < verts.Length; i++)
			{
				Vector3 vector = verts[i];
				if (flag3)
				{
					float x = vector.x;
					vector.x = vector.z;
					vector.z = x;
				}
				if (flag)
				{
					vector.x = 0f - vector.x;
				}
				if (flag2)
				{
					vector.z = 0f - vector.z;
				}
				verts[i] = vector;
				if (normals != null)
				{
					Vector3 vector2 = normals[i];
					if (flag3)
					{
						float x2 = vector2.x;
						vector2.x = vector2.z;
						vector2.z = x2;
					}
					if (flag)
					{
						vector2.x = 0f - vector2.x;
					}
					if (flag2)
					{
						vector2.z = 0f - vector2.z;
					}
					normals[i] = vector2;
				}
				if (tangents != null)
				{
					Vector4 vector3 = tangents[i];
					if (flag3)
					{
						float x3 = vector3.x;
						vector3.x = vector3.z;
						vector3.z = x3;
					}
					if (flag)
					{
						vector3.x = 0f - vector3.x;
					}
					if (flag2)
					{
						vector3.z = 0f - vector3.z;
					}
					tangents[i] = vector3;
				}
			}
			if (!mirror)
			{
				return;
			}
			for (int j = 0; j < tris.Length; j++)
			{
				for (int k = 0; k < tris.Length; k += 3)
				{
					int num = tris[k];
					tris[k] = tris[k + 2];
					tris[k + 2] = num;
				}
			}
		}
	}
}
