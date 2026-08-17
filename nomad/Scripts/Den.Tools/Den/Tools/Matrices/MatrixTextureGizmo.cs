using UnityEngine;

namespace Den.Tools.Matrices
{
	public class MatrixTextureGizmo : DebugGizmos.IGizmo
	{
		private Mesh mesh;

		private Texture2D texture;

		private byte[] bytes;

		private Material material;

		private Matrix4x4 tfm;

		private static readonly Vector3[] verts = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1f, 0f, 1f)
		};

		private static readonly Vector2[] uvs = new Vector2[4]
		{
			new Vector3(0f, 0f),
			new Vector3(0f, 1f),
			new Vector3(1f, 0f),
			new Vector3(1f, 1f)
		};

		private static readonly int[] tris = new int[6] { 1, 2, 0, 2, 1, 3 };

		public Color Color
		{
			get
			{
				return Color.white;
			}
			set
			{
			}
		}

		public void SetMatrix(Matrix matrix, bool centerCell = false, FilterMode filterMode = FilterMode.Point)
		{
			if (texture == null || texture.width != matrix.rect.size.x || texture.height != matrix.rect.size.z)
			{
				texture = new Texture2D(matrix.rect.size.x, matrix.rect.size.z, TextureFormat.RFloat, mipChain: false, linear: true);
			}
			texture.filterMode = filterMode;
			matrix.ExportTextureRaw(texture);
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			mesh.Clear();
			if (centerCell)
			{
				Vector2 vector = new Vector2(1f / (float)matrix.rect.size.x, 1f / (float)matrix.rect.size.z);
				mesh.vertices = verts;
				Vector2[] array = uvs.Copy();
				for (int i = 0; i < array.Length; i++)
				{
					array[i] -= array[i] * vector - vector / 2f;
				}
				mesh.uv = array;
				mesh.triangles = tris;
			}
			else
			{
				mesh.vertices = verts;
				mesh.uv = uvs;
				mesh.triangles = tris;
			}
			if (material == null)
			{
				material = new Material(Shader.Find("Hidden/MapMagic/TexturePreview"));
			}
			material.SetTexture("_MainTex", texture);
			material.SetInt("_Margins", 0);
		}

		public void SetOffsetSize(Vector2D worldOffset, Vector2D worldSize)
		{
			tfm = Matrix4x4.TRS((Vector3)worldOffset, Quaternion.identity, (Vector3)worldSize);
		}

		public void SetMatrixWorld(MatrixWorld matrix, bool centerCell = false, FilterMode filterMode = FilterMode.Point)
		{
			SetMatrix(matrix, centerCell, filterMode);
			SetOffsetSize((Vector2D)matrix.worldPos, (Vector2D)matrix.worldSize);
		}

		public void Draw()
		{
			Draw(false, false, 0f, 1f, null);
		}

		public void Draw(bool colorize = false, bool relief = false, float min = 0f, float max = 1f, Transform parent = null)
		{
			if (!(material == null) && !(mesh == null))
			{
				material.SetFloat("_Colorize", colorize ? 1 : 0);
				material.SetFloat("_Relief", relief ? 1 : 0);
				material.SetFloat("_MinValue", min);
				material.SetFloat("_MaxValue", max);
				material.SetPass(0);
				Graphics.DrawMeshNow(mesh, tfm);
			}
		}

		public static void DrawNow(Matrix matrix, Vector2D worldOffset, Vector2D worldSize)
		{
			MatrixTextureGizmo matrixTextureGizmo = new MatrixTextureGizmo();
			matrixTextureGizmo.SetMatrix(matrix);
			matrixTextureGizmo.SetOffsetSize(worldOffset, worldSize);
			matrixTextureGizmo.Draw();
		}
	}
}
