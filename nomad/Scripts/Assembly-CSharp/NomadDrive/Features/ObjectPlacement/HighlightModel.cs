using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public class HighlightModel
	{
		public GameObject Root { get; }

		public List<Renderer> Renderers { get; }

		public HighlightModel(GameObject root, List<Renderer> renderers)
		{
			Root = root;
			Renderers = renderers;
		}

		public void ApplyMaterial(Material ghostMaterial)
		{
			foreach (Renderer renderer in Renderers)
			{
				if (!(renderer == null))
				{
					Material[] array = new Material[renderer.sharedMaterials.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = new Material(ghostMaterial);
					}
					renderer.materials = array;
				}
			}
		}

		public void SetLayer(int layer)
		{
			foreach (Renderer renderer in Renderers)
			{
				if (!(renderer == null))
				{
					renderer.gameObject.layer = layer;
				}
			}
		}

		public void Destroy()
		{
			foreach (Renderer renderer in Renderers)
			{
				if (renderer == null)
				{
					continue;
				}
				Material[] materials = renderer.materials;
				foreach (Material material in materials)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
			}
			if (Root != null)
			{
				Object.Destroy(Root);
			}
			Renderers.Clear();
		}
	}
}
