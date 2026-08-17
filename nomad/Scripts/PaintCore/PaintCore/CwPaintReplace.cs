using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintReplace")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Replace")]
	public class CwPaintReplace : MonoBehaviour, IHitCoord, IHit
	{
		[SerializeField]
		private CwGroup group;

		[SerializeField]
		private Texture texture;

		[SerializeField]
		private Color color = Color.white;

		[SerializeField]
		private CwModifierList modifiers;

		public CwGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public CwModifierList Modifiers
		{
			get
			{
				if (modifiers == null)
				{
					modifiers = new CwModifierList();
				}
				return modifiers;
			}
		}

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, CwHit hit, Quaternion rotation)
		{
			CwModel componentInParent = hit.Transform.GetComponentInParent<CwModel>();
			if (!(componentInParent != null))
			{
				return;
			}
			List<CwPaintableTexture> list = componentInParent.FindPaintableTextures(group);
			if (list.Count > 0)
			{
				Color color = this.color;
				Texture texture = this.texture;
				if (modifiers != null && modifiers.Count > 0)
				{
					CwHelper.BeginSeed(seed);
					modifiers.ModifyColor(ref color, preview, pressure);
					modifiers.ModifyTexture(ref texture, preview, pressure);
					CwHelper.EndSeed();
				}
				CwCommandReplace.Instance.SetState(preview, priority);
				CwCommandReplace.Instance.SetMaterial(texture, color);
				for (int num = list.Count - 1; num >= 0; num--)
				{
					CwPaintableTexture paintableTexture = list[num];
					CwPaintableManager.Submit(CwCommandReplace.Instance, componentInParent, paintableTexture);
				}
			}
		}
	}
}
