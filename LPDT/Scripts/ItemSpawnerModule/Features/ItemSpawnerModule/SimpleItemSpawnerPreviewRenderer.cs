using UnityEngine;

namespace Features.ItemSpawnerModule
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public sealed class SimpleItemSpawnerPreviewRenderer : MonoBehaviour
	{
		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private void Awake()
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponent<MeshRenderer>();
			}
			if (_meshRenderer != null)
			{
				_meshRenderer.enabled = false;
			}
		}
	}
}
