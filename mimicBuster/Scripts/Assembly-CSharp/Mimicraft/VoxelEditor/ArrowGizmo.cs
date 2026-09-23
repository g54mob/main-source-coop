using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class ArrowGizmo : MonoBehaviour
	{
		private const float ShaftRadius = 0.05f;

		private const float HeadRadius = 0.12f;

		private const float HeadLength = 0.25f;

		private const float HeadLengthShare = 0.4f;

		public const float MinLength = 0.4f;

		private const int Segments = 10;

		private static readonly Color InwardColor = new Color(1f, 0.2f, 0.2f, 0.95f);

		private static readonly Color WarningColor = new Color(1f, 0f, 0f, 1f);

		private MeshFilter meshFilter;

		private MeshRenderer meshRenderer;

		private Mesh mesh;

		private static Color OutwardColor => EditorPalette.ExtrudeGizmo;

		public static ArrowGizmo Create(Transform parent)
		{
			GameObject obj = new GameObject("ArrowGizmo");
			obj.transform.SetParent(parent, worldPositionStays: false);
			ArrowGizmo arrowGizmo = obj.AddComponent<ArrowGizmo>();
			arrowGizmo.Init();
			return arrowGizmo;
		}

		private void Init()
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				meshRenderer.sharedMaterial = new Material(shader);
			}
			mesh = new Mesh
			{
				name = "ArrowGizmoMesh"
			};
			meshFilter.sharedMesh = mesh;
			base.gameObject.SetActive(value: false);
		}

		public void Show(Vector3 originLocal, Vector3Int faceNormal, int steps, float scaleCompensation = 1f, float lengthMultiplier = 1f, float thicknessMultiplier = 1f, bool overLimit = false)
		{
			float num = ((steps < 0) ? (-1f) : 1f);
			float num2 = 1f / Mathf.Max(scaleCompensation, 0.0001f);
			float length = Mathf.Max((float)Mathf.Abs(steps) * num2, 0.4f * lengthMultiplier) * num;
			Color color = (overLimit ? WarningColor : ((steps < 0) ? InwardColor : OutwardColor));
			ShowCustom(originLocal, faceNormal, length, color, scaleCompensation, thicknessMultiplier);
		}

		public void ShowCustom(Vector3 originLocal, Vector3 directionLocal, float length, Color color, float scaleCompensation = 1f, float thicknessMultiplier = 1f)
		{
			base.transform.localPosition = originLocal;
			base.transform.localRotation = Quaternion.LookRotation(directionLocal);
			float num = scaleCompensation * thicknessMultiplier;
			base.transform.localScale = new Vector3(num, num, scaleCompensation);
			BuildMesh(length, thicknessMultiplier);
			if (meshRenderer.sharedMaterial != null)
			{
				meshRenderer.sharedMaterial.color = color;
			}
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private void BuildMesh(float length, float thicknessMultiplier)
		{
			float num = Mathf.Min(0.25f * thicknessMultiplier, Mathf.Abs(length) * 0.4f);
			float num2 = Mathf.Sign(length);
			float num3 = length - num2 * num;
			if (num2 * num3 < 0f)
			{
				num3 = 0f;
			}
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			BuildTaperedCylinder(vertices, triangles, 0f, num3, 0.05f, 0.05f);
			BuildTaperedCylinder(vertices, triangles, num3, length, 0.12f, 0f);
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		private static void BuildTaperedCylinder(List<Vector3> vertices, List<int> triangles, float zStart, float zEnd, float radiusStart, float radiusEnd)
		{
			int count = vertices.Count;
			for (int i = 0; i < 10; i++)
			{
				float f = (float)i * MathF.PI * 2f / 10f;
				float num = Mathf.Cos(f);
				float num2 = Mathf.Sin(f);
				vertices.Add(new Vector3(num * radiusStart, num2 * radiusStart, zStart));
				vertices.Add(new Vector3(num * radiusEnd, num2 * radiusEnd, zEnd));
			}
			for (int j = 0; j < 10; j++)
			{
				int num3 = (j + 1) % 10;
				int item = count + j * 2;
				int item2 = count + j * 2 + 1;
				int item3 = count + num3 * 2;
				int item4 = count + num3 * 2 + 1;
				triangles.Add(item);
				triangles.Add(item2);
				triangles.Add(item3);
				triangles.Add(item3);
				triangles.Add(item2);
				triangles.Add(item4);
			}
		}
	}
}
