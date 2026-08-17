using System.Collections.Generic;
using PaintCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintIn3D
{
	[RequireComponent(typeof(Renderer))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwPaintableMeshAtlas")]
	[AddComponentMenu("CW/Paint in 3D/CW Paintable Mesh Atlas")]
	public class CwPaintableMeshAtlas : CwMeshModel
	{
		[SerializeField]
		[FormerlySerializedAs("paintable")]
		protected CwPaintableMesh parent;

		public virtual CwPaintableMesh Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}

		public override bool IsActivated
		{
			get
			{
				if (parent != null && parent != this)
				{
					return parent.IsActivated;
				}
				return false;
			}
		}

		public override void Activate()
		{
			if (parent != null && parent != this)
			{
				parent.Activate();
			}
		}

		public override List<CwPaintableTexture> FindPaintableTextures(CwGroup group)
		{
			if (parent != null && parent != this)
			{
				return parent.FindPaintableTextures(group);
			}
			return null;
		}
	}
}
