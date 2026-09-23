using System.Collections.Generic;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Tutorial
{
	public class TutorialFaceGlow : MonoBehaviour
	{
		private const float Inset = 0.08f;

		private const float NormalOffset = 0.02f;

		private static readonly Color GlowColor = new Color(0.4f, 0.9f, 1f, 0.45f);

		private MeshFilter meshFilter;

		private MeshRenderer meshRenderer;

		private Material material;

		private Mesh mesh;

		public static TutorialFaceGlow Create(VoxelModel model)
		{
			GameObject gameObject = new GameObject("TutorialFaceGlow");
			gameObject.transform.SetParent(model.transform, worldPositionStays: false);
			TutorialFaceGlow tutorialFaceGlow = gameObject.AddComponent<TutorialFaceGlow>();
			tutorialFaceGlow.meshFilter = gameObject.AddComponent<MeshFilter>();
			tutorialFaceGlow.meshRenderer = gameObject.AddComponent<MeshRenderer>();
			tutorialFaceGlow.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			tutorialFaceGlow.meshRenderer.receiveShadows = false;
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				tutorialFaceGlow.material = new Material(shader);
				tutorialFaceGlow.material.SetColor("_Color", GlowColor);
				tutorialFaceGlow.meshRenderer.sharedMaterial = tutorialFaceGlow.material;
			}
			tutorialFaceGlow.mesh = new Mesh
			{
				name = "TutorialFaceGlow"
			};
			tutorialFaceGlow.meshFilter.sharedMesh = tutorialFaceGlow.mesh;
			gameObject.SetActive(value: false);
			return tutorialFaceGlow;
		}

		public void Show(IReadOnlyList<(Vector3Int Cell, Vector3Int Normal)> faces)
		{
			if (faces == null || faces.Count == 0)
			{
				Hide();
				return;
			}
			List<Vector3> vertices = new List<Vector3>(faces.Count * 4);
			List<Vector3> normals = new List<Vector3>(faces.Count * 4);
			List<int> triangles = new List<int>(faces.Count * 6);
			foreach (var (cell, faceNormal) in faces)
			{
				AppendQuad(cell, faceNormal, vertices, normals, triangles);
			}
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetNormals(normals);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateBounds();
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private static void AppendQuad(Vector3Int cell, Vector3Int faceNormal, List<Vector3> vertices, List<Vector3> normals, List<int> triangles)
		{
			FaceAxes.GetBasis(faceNormal, out var right, out var up);
			Vector3 vector = right;
			Vector3 vector2 = up;
			Vector3 vector3 = faceNormal;
			Vector3 vector4 = cell + new Vector3(0.5f, 0.5f, 0.5f) + vector3 * 0.52f;
			float num = 0.42000002f;
			int count = vertices.Count;
			vertices.Add(vector4 - vector * num - vector2 * num);
			vertices.Add(vector4 + vector * num - vector2 * num);
			vertices.Add(vector4 + vector * num + vector2 * num);
			vertices.Add(vector4 - vector * num + vector2 * num);
			for (int i = 0; i < 4; i++)
			{
				normals.Add(vector3);
			}
			triangles.Add(count);
			triangles.Add(count + 1);
			triangles.Add(count + 2);
			triangles.Add(count);
			triangles.Add(count + 2);
			triangles.Add(count + 3);
		}

		private void LateUpdate()
		{
			if (!(material == null))
			{
				Color glowColor = GlowColor;
				glowColor.a *= 0.6f + 0.4f * Mathf.Sin(Time.unscaledTime * 3f);
				material.SetColor("_Color", glowColor);
			}
		}

		private void OnDestroy()
		{
			if (mesh != null)
			{
				Object.Destroy(mesh);
			}
			if (material != null)
			{
				Object.Destroy(material);
			}
		}
	}
}
