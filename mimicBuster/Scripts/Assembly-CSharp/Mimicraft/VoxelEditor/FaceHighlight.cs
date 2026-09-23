using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class FaceHighlight : MonoBehaviour
	{
		private const float Inset = 0.03f;

		private const float NormalOffset = 0.01f;

		private MeshFilter meshFilter;

		private Mesh mesh;

		public static FaceHighlight Create(Transform parent)
		{
			GameObject obj = new GameObject("FaceHighlight");
			obj.transform.SetParent(parent, worldPositionStays: false);
			FaceHighlight faceHighlight = obj.AddComponent<FaceHighlight>();
			faceHighlight.Init();
			return faceHighlight;
		}

		private void Init()
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				meshRenderer.sharedMaterial = new Material(shader);
			}
			mesh = new Mesh
			{
				name = "FaceHighlightMesh"
			};
			meshFilter.sharedMesh = mesh;
			base.gameObject.SetActive(value: false);
		}

		public void Show(Vector3Int voxelPosition, Vector3Int faceNormal)
		{
			BuildQuad(voxelPosition, faceNormal);
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		private void BuildQuad(Vector3Int voxelPosition, Vector3Int faceNormal)
		{
			FaceAxes.GetBasis(faceNormal, out var right, out var up);
			Vector3 vector = right;
			Vector3 vector2 = up;
			Vector3 vector3 = faceNormal;
			Vector3 vector4 = voxelPosition + new Vector3(0.5f, 0.5f, 0.5f) + vector3 * 0.51f;
			float num = 0.47f;
			Vector3 vector5 = vector4 - vector * num - vector2 * num;
			Vector3 vector6 = vector4 + vector * num - vector2 * num;
			Vector3 vector7 = vector4 + vector * num + vector2 * num;
			Vector3 vector8 = vector4 - vector * num + vector2 * num;
			mesh.Clear();
			mesh.vertices = new Vector3[4] { vector5, vector6, vector7, vector8 };
			mesh.normals = new Vector3[4] { vector3, vector3, vector3, vector3 };
			mesh.triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
			mesh.RecalculateBounds();
		}
	}
}
