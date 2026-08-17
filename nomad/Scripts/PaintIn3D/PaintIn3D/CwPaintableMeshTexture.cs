using System;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwPaintableMeshTexture")]
	[AddComponentMenu("CW/Paint in 3D/CW Paintable Mesh Texture")]
	public class CwPaintableMeshTexture : CwPaintableTexture
	{
		[SerializeField]
		private bool autoDilate;

		[NonSerialized]
		private CwPaintableMesh parent;

		public bool AutoDilate
		{
			get
			{
				return autoDilate;
			}
			set
			{
				autoDilate = value;
			}
		}

		protected override void ApplyTexture(Texture texture)
		{
			if (parent == null)
			{
				parent = GetComponentInParent<CwPaintableMesh>();
			}
			if (!(parent != null))
			{
				return;
			}
			if (parent.MaterialApplication == CwPaintableMesh.MaterialApplicationType.PropertyBlock)
			{
				parent.ApplyTexture(base.Slot, texture);
				{
					foreach (Renderer otherRenderer in parent.OtherRenderers)
					{
						if (otherRenderer != null)
						{
							parent.ApplyTexture(otherRenderer, base.Slot, texture);
						}
					}
					return;
				}
			}
			if (parent.MaterialApplication != CwPaintableMesh.MaterialApplicationType.ClonerAndTextures || base.Slot.Index < 0)
			{
				return;
			}
			Material[] materials = parent.Materials;
			if (base.Slot.Index < materials.Length)
			{
				Material material = materials[base.Slot.Index];
				if (material != null)
				{
					material.SetTexture(base.Slot.Name, texture);
				}
			}
		}

		protected override void PostExecuteCommands(RenderTexture main)
		{
			if (!autoDilate)
			{
				return;
			}
			MeshFilter component = GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				CwPaintableMesh cwPaintableMesh = base.Model as CwPaintableMesh;
				if (cwPaintableMesh != null)
				{
					CwDilate.Dilate(main, cwPaintableMesh.GetDilateMeshes(), 0, 0);
				}
			}
		}
	}
}
