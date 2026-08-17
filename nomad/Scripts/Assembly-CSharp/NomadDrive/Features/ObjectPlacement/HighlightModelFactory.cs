using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class HighlightModelFactory
	{
		public static HighlightModel CreateFrom(Transform source, Transform parent)
		{
			GameObject gameObject = new GameObject("HighlightModel");
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			List<Renderer> renderers = new List<Renderer>();
			CopyVisualsIfPresent(source, gameObject.transform, renderers);
			for (int i = 0; i < source.childCount; i++)
			{
				CloneHierarchy(source.GetChild(i), gameObject.transform, renderers);
			}
			return new HighlightModel(gameObject, renderers);
		}

		private static void CloneHierarchy(Transform source, Transform ghostParent, List<Renderer> renderers)
		{
			Transform transform = new GameObject(source.gameObject.name + "_Ghost").transform;
			transform.SetParent(ghostParent, worldPositionStays: false);
			transform.localPosition = source.localPosition;
			transform.localRotation = source.localRotation;
			transform.localScale = source.localScale;
			CopyVisualsIfPresent(source, transform, renderers);
			for (int i = 0; i < source.childCount; i++)
			{
				CloneHierarchy(source.GetChild(i), transform, renderers);
			}
		}

		private static void CopyVisualsIfPresent(Transform source, Transform ghostTransform, List<Renderer> renderers)
		{
			if (source.gameObject.activeInHierarchy)
			{
				SkinnedMeshRenderer component3;
				if (source.TryGetComponent<MeshRenderer>(out var component) && component.enabled && source.TryGetComponent<MeshFilter>(out var component2) && component2.sharedMesh != null)
				{
					ghostTransform.gameObject.AddComponent<MeshFilter>().sharedMesh = component2.sharedMesh;
					MeshRenderer meshRenderer = ghostTransform.gameObject.AddComponent<MeshRenderer>();
					meshRenderer.sharedMaterials = component.sharedMaterials;
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
					meshRenderer.receiveShadows = false;
					renderers.Add(meshRenderer);
				}
				else if (source.TryGetComponent<SkinnedMeshRenderer>(out component3) && component3.enabled && component3.sharedMesh != null)
				{
					MeshFilter meshFilter = ghostTransform.gameObject.AddComponent<MeshFilter>();
					Mesh mesh = new Mesh();
					component3.BakeMesh(mesh);
					meshFilter.sharedMesh = mesh;
					MeshRenderer meshRenderer2 = ghostTransform.gameObject.AddComponent<MeshRenderer>();
					meshRenderer2.sharedMaterials = component3.sharedMaterials;
					meshRenderer2.shadowCastingMode = ShadowCastingMode.Off;
					meshRenderer2.receiveShadows = false;
					renderers.Add(meshRenderer2);
				}
			}
		}
	}
}
