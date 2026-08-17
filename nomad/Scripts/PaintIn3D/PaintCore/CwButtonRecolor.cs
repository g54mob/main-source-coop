using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwButtonRecolor")]
	[AddComponentMenu("CW/Paint Core/CW Button Recolor")]
	public class CwButtonRecolor : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public enum ApplyType
		{
			CloneMaterial = 0,
			ApplyToExisting = 1,
			PropertyBlock = 2
		}

		[SerializeField]
		private Renderer targetRenderer;

		[SerializeField]
		private int targetMaterial;

		[SerializeField]
		private string targetProperty = "_Color";

		[SerializeField]
		private ApplyType apply = ApplyType.PropertyBlock;

		private static MaterialPropertyBlock properties;

		private static List<Material> tempMaterials = new List<Material>();

		public Renderer TargetRenderer
		{
			get
			{
				return targetRenderer;
			}
			set
			{
				targetRenderer = value;
			}
		}

		public int TargetMaterial
		{
			get
			{
				return targetMaterial;
			}
			set
			{
				targetMaterial = value;
			}
		}

		public string TargetProperty
		{
			get
			{
				return targetProperty;
			}
			set
			{
				targetProperty = value;
			}
		}

		public ApplyType Apply
		{
			get
			{
				return apply;
			}
			set
			{
				apply = value;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Recolor();
		}

		[ContextMenu("Recolor")]
		public void Recolor()
		{
			Color value = Random.ColorHSV();
			if (apply == ApplyType.CloneMaterial && targetRenderer != null)
			{
				targetRenderer.GetSharedMaterials(tempMaterials);
				if (tempMaterials.Count >= targetMaterial && tempMaterials[targetMaterial] != null)
				{
					tempMaterials[targetMaterial] = Object.Instantiate(tempMaterials[targetMaterial]);
					targetRenderer.sharedMaterials = tempMaterials.ToArray();
					apply = ApplyType.ApplyToExisting;
				}
			}
			if (apply == ApplyType.ApplyToExisting && targetRenderer != null)
			{
				targetRenderer.GetSharedMaterials(tempMaterials);
				if (tempMaterials.Count >= targetMaterial && tempMaterials[targetMaterial] != null)
				{
					tempMaterials[targetMaterial].SetColor(targetProperty, value);
				}
			}
			if (apply == ApplyType.PropertyBlock && targetRenderer != null)
			{
				if (properties == null)
				{
					properties = new MaterialPropertyBlock();
				}
				targetRenderer.GetPropertyBlock(properties, targetMaterial);
				properties.SetColor(targetProperty, value);
				targetRenderer.SetPropertyBlock(properties, targetMaterial);
			}
		}
	}
}
