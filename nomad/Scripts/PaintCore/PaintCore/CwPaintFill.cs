using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintFill")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Fill")]
	public class CwPaintFill : MonoBehaviour, IHitCoord, IHit
	{
		[SerializeField]
		private CwGroup group;

		[SerializeField]
		private CwBlendMode blendMode = CwBlendMode.AlphaBlend(Vector4.one);

		[SerializeField]
		private Texture texture;

		[SerializeField]
		private Color color = Color.white;

		[Range(0f, 1f)]
		[SerializeField]
		private float opacity = 1f;

		[Range(0f, 1f)]
		[SerializeField]
		private float minimum;

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

		public CwBlendMode BlendMode
		{
			get
			{
				return blendMode;
			}
			set
			{
				blendMode = value;
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

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		public float Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				minimum = value;
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

		public void IncrementOpacity(float delta)
		{
			opacity = Mathf.Clamp01(opacity + delta);
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
				float num = opacity;
				Texture texture = this.texture;
				if (modifiers != null && modifiers.Count > 0)
				{
					CwHelper.BeginSeed(seed);
					modifiers.ModifyColor(ref color, preview, pressure);
					modifiers.ModifyOpacity(ref num, preview, pressure);
					modifiers.ModifyTexture(ref texture, preview, pressure);
					CwHelper.EndSeed();
				}
				CwCommandFill.Instance.SetState(preview, priority);
				CwCommandFill.Instance.SetMaterial(blendMode, texture, color, opacity, minimum);
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					CwPaintableTexture paintableTexture = list[num2];
					CwPaintableManager.Submit(CwCommandFill.Instance, componentInParent, paintableTexture);
				}
			}
		}
	}
}
