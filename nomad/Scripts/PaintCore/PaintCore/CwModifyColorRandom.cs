using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyColorRandom")]
	public class CwModifyColorRandom : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Color";

		public static string Title = "Random";

		[SerializeField]
		private Gradient gradient;

		[SerializeField]
		private BlendType blend;

		public Gradient Gradient
		{
			get
			{
				if (gradient == null)
				{
					gradient = new Gradient();
				}
				return gradient;
			}
		}

		public BlendType Blend
		{
			get
			{
				return blend;
			}
			set
			{
				blend = value;
			}
		}

		protected override void OnModifyColor(ref Color color, float pressure)
		{
			if (gradient != null)
			{
				Color color2 = gradient.Evaluate(UnityEngine.Random.value);
				switch (blend)
				{
				case BlendType.Replace:
					color = color2;
					break;
				case BlendType.Multiply:
					color *= color2;
					break;
				case BlendType.Increment:
					color += color2;
					break;
				}
			}
		}
	}
}
