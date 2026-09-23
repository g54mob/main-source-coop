using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class ObjectHoverHighlight : MonoBehaviour
	{
		private const float ShownAlpha = 0.18f;

		private const float FadeSpeed = 6f;

		private static readonly Color TintColor = new Color(1f, 1f, 1f, 1f);

		private MeshFilter sourceMeshFilter;

		private MeshFilter meshFilter;

		private Material material;

		private float currentAlpha;

		private bool wantShown;

		public static ObjectHoverHighlight Create(Transform parent, MeshFilter sourceMeshFilter)
		{
			GameObject obj = new GameObject("ObjectHoverHighlight");
			obj.transform.SetParent(parent, worldPositionStays: false);
			ObjectHoverHighlight objectHoverHighlight = obj.AddComponent<ObjectHoverHighlight>();
			objectHoverHighlight.Init(sourceMeshFilter);
			return objectHoverHighlight;
		}

		private void Init(MeshFilter source)
		{
			sourceMeshFilter = source;
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/SurfaceOverlayUnlit");
			if (shader != null)
			{
				material = new Material(shader)
				{
					color = new Color(TintColor.r, TintColor.g, TintColor.b, 0f)
				};
			}
			meshRenderer.sharedMaterial = material;
		}

		public void SetShown(bool shown)
		{
			wantShown = shown;
		}

		private void LateUpdate()
		{
			if (sourceMeshFilter != null && meshFilter.sharedMesh != sourceMeshFilter.sharedMesh)
			{
				meshFilter.sharedMesh = sourceMeshFilter.sharedMesh;
			}
			float b = (wantShown ? 0.18f : 0f);
			currentAlpha = Mathf.Lerp(currentAlpha, b, Time.deltaTime * 6f);
			if (material != null)
			{
				material.color = new Color(TintColor.r, TintColor.g, TintColor.b, currentAlpha);
			}
		}
	}
}
