using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintIn3D
{
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(CwPaintableMesh))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwMaterialCloner")]
	[AddComponentMenu("CW/Paint in 3D/CW Material Cloner")]
	public class CwMaterialCloner : MonoBehaviour
	{
		public struct External
		{
			public Renderer Root;

			public int Index;
		}

		[SerializeField]
		private int index;

		[SerializeField]
		private string shaderKeyword;

		[SerializeField]
		private List<External> externals;

		[SerializeField]
		private bool activated;

		[SerializeField]
		private Material current;

		[SerializeField]
		private Material original;

		[NonSerialized]
		private static List<Material> tempMaterials = new List<Material>();

		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public string ShaderKeyword
		{
			get
			{
				return shaderKeyword;
			}
			set
			{
				shaderKeyword = value;
			}
		}

		public List<External> Externals
		{
			get
			{
				return externals;
			}
			set
			{
				externals = value;
			}
		}

		public Material Original => original;

		public Material Current => current;

		public bool Activated => activated;

		[ContextMenu("Activate")]
		public void Activate()
		{
			if (activated || index < 0)
			{
				return;
			}
			Renderer component = GetComponent<Renderer>();
			component.GetSharedMaterials(tempMaterials);
			if (index < 0 || index >= tempMaterials.Count)
			{
				return;
			}
			original = tempMaterials[index];
			if (original != null)
			{
				activated = true;
				current = UnityEngine.Object.Instantiate(original);
				if (!string.IsNullOrEmpty(shaderKeyword))
				{
					current.EnableKeyword(shaderKeyword);
				}
				Replace(component, index, original, current);
			}
		}

		[ContextMenu("Deactivate")]
		public void Deactivate()
		{
			if (!activated)
			{
				return;
			}
			activated = false;
			Replace(GetComponent<Renderer>(), index, current, original);
			foreach (External external in externals)
			{
				Replace(external.Root, index, current, original);
			}
			current = CwHelper.Destroy(current);
		}

		private void Replace(Renderer renderer, int index, Material oldMaterial, Material newMaterial)
		{
			renderer.GetSharedMaterials(tempMaterials);
			if (index >= 0 && index < tempMaterials.Count && tempMaterials[index] == oldMaterial)
			{
				tempMaterials[index] = newMaterial;
				renderer.sharedMaterials = tempMaterials.ToArray();
			}
		}
	}
}
