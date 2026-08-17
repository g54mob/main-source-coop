using System.Collections.Generic;
using UnityEngine;

namespace RadiantGI.Universal
{
	[ExecuteInEditMode]
	public class RadiantVirtualEmitter : MonoBehaviour
	{
		[Header("GI Color")]
		[ColorUsage(false, true)]
		public Color color = new Color(1f, 1f, 1f);

		[Tooltip("Enable this option to add the emission color of the material used by this object to the global illumination.")]
		public bool addMaterialEmission;

		[Tooltip("The renderer from which synchronize the emission color")]
		public Renderer targetRenderer;

		[Tooltip("Optionally specify the material for the emission color")]
		public Material material;

		public string emissionPropertyName = "_EmissionColor";

		[Tooltip("Useful in case the gameobject uses more than one material")]
		public int materialIndex;

		public float intensity = 1f;

		public float range = 10f;

		[Header("Area Of Influence")]
		public Vector3 boxCenter;

		public Vector3 boxSize = new Vector3(25f, 25f, 25f);

		public bool boundsInLocalSpace = true;

		private int emissionNameId;

		private Renderer thisRenderer;

		private static List<Material> sharedMaterials = new List<Material>();

		private void OnValidate()
		{
			intensity = Mathf.Max(0f, intensity);
			range = Mathf.Max(0f, range);
		}

		private void OnEnable()
		{
			emissionNameId = Shader.PropertyToID(emissionPropertyName);
			thisRenderer = GetComponentInChildren<Renderer>();
			RadiantRenderFeature.RegisterVirtualEmitter(this);
		}

		private void OnDisable()
		{
			RadiantRenderFeature.UnregisterVirtualEmitter(this);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.75f);
			Gizmos.DrawWireSphere(base.transform.position, range);
		}

		public Color GetGIColor()
		{
			Color color = this.color;
			if (addMaterialEmission)
			{
				Material material = this.material;
				if (material == null)
				{
					Renderer renderer = ((targetRenderer != null) ? targetRenderer : thisRenderer);
					if (renderer != null)
					{
						if (materialIndex == 0)
						{
							material = renderer.sharedMaterial;
						}
						else
						{
							renderer.GetSharedMaterials(sharedMaterials);
							if (materialIndex < sharedMaterials.Count)
							{
								material = sharedMaterials[materialIndex];
							}
						}
					}
				}
				if (material != null && material.HasProperty(emissionNameId))
				{
					color += material.GetColor(emissionNameId);
				}
			}
			return color * intensity;
		}

		public Vector4 GetGIColorAndRange()
		{
			Color gIColor = GetGIColor();
			return new Vector4(gIColor.r, gIColor.g, gIColor.b, range);
		}

		public Bounds GetBounds()
		{
			Bounds result = new Bounds(boxCenter, boxSize);
			if (boundsInLocalSpace)
			{
				result.center += base.transform.position;
			}
			return result;
		}

		public void SetBounds(Bounds bounds)
		{
			if (boundsInLocalSpace)
			{
				bounds.center -= base.transform.position;
			}
			boxCenter = bounds.center;
			boxSize = bounds.size;
		}
	}
}
