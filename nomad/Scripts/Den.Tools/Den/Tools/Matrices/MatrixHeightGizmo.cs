using UnityEngine;

namespace Den.Tools.Matrices
{
	public class MatrixHeightGizmo
	{
		public enum ZMode
		{
			Occluded = 0,
			Overlay = 1,
			Both = 2
		}

		private Mesh mesh;

		private Material material;

		private Matrix4x4 tfm;

		public void SetMatrix(Matrix matrix, bool faceted = false)
		{
			(Vector3[] verts, Vector2[] uvs, int[] tris) tuple = MakePlane(matrix.rect.size.x - 1);
			Vector3[] verts = tuple.verts;
			int[] tris = tuple.tris;
			for (int i = 0; i < matrix.rect.size.x; i++)
			{
				for (int j = 0; j < matrix.rect.size.z; j++)
				{
					int num = j * matrix.rect.size.x + i;
					verts[num].y = matrix.arr[num];
				}
			}
			if (faceted)
			{
				SplitFaceted(ref verts, ref tris);
			}
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.MarkDynamic();
			}
			mesh.vertices = verts;
			mesh.triangles = tris;
			mesh.RecalculateNormals();
			if (material == null)
			{
				material = new Material(Shader.Find("Standard"));
				material.SetColor("_Color", Color.gray);
			}
		}

		public void SetMatrixWorld(MatrixWorld matrix, bool faceted = false)
		{
			SetMatrix(matrix, faceted);
			SetOffsetSize(matrix.worldPos, matrix.worldSize);
		}

		private static (Vector3[] verts, Vector2[] uvs, int[] tris) MakePlane(int resolution)
		{
			float num = 1f / (float)resolution;
			Vector3[] array = new Vector3[(resolution + 1) * (resolution + 1)];
			Vector2[] array2 = new Vector2[array.Length];
			int[] array3 = new int[resolution * resolution * 2 * 3];
			int num2 = 0;
			int num3 = 0;
			for (float num4 = 0f; num4 < 1.00001f; num4 += num)
			{
				for (float num5 = 0f; num5 < 1.00001f; num5 += num)
				{
					array[num2] = new Vector3(num5, 0f, num4);
					array2[num2] = new Vector2(num5, num4);
					if (num4 > 1E-05f && num5 > 1E-05f)
					{
						array3[num3] = num2 - (resolution + 1);
						array3[num3 + 1] = num2 - resolution - 2;
						array3[num3 + 2] = num2 - 1;
						array3[num3 + 3] = num2 - 1;
						array3[num3 + 4] = num2;
						array3[num3 + 5] = num2 - (resolution + 1);
						num3 += 6;
					}
					num2++;
				}
			}
			return (verts: array, uvs: array2, tris: array3);
		}

		private void OffsetScale(Vector3[] verts, Vector2D offset, Vector2D scale)
		{
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i] = verts[i] * scale + offset;
			}
		}

		private void SplitFaceted(ref Vector3[] verts, ref int[] tris)
		{
			Vector3[] array = new Vector3[tris.Length / 6 * 4];
			int num = 0;
			for (int i = 0; i < tris.Length; i += 6)
			{
				array[num] = verts[tris[i]];
				array[num + 1] = verts[tris[i + 1]];
				array[num + 2] = verts[tris[i + 2]];
				array[num + 3] = verts[tris[i + 4]];
				tris[i] = num;
				tris[i + 1] = num + 1;
				tris[i + 2] = num + 2;
				tris[i + 3] = num + 2;
				tris[i + 4] = num + 3;
				tris[i + 5] = num;
				num += 4;
			}
			verts = array;
		}

		public void SetOffsetSize(Vector3 worldOffset, Vector3 worldSize)
		{
			tfm = Matrix4x4.TRS(worldOffset, Quaternion.identity, worldSize);
		}

		public void Draw(Material material = null, Transform parent = null)
		{
			(material ?? this.material).SetPass(0);
			Graphics.DrawMeshNow(mesh, tfm);
		}

		public static void DrawNow(Matrix matrix, Vector3 worldOffset, Vector3 worldSize)
		{
			MatrixHeightGizmo matrixHeightGizmo = new MatrixHeightGizmo();
			matrixHeightGizmo.SetMatrix(matrix);
			matrixHeightGizmo.SetOffsetSize(worldOffset, worldSize);
			matrixHeightGizmo.Draw();
		}
	}
}
