using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyOpacityPressure")]
	public class CwModifyOpacityPressure : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Opacity";

		public static string Title = "Pressure";

		[SerializeField]
		private float opacity = 1f;

		[SerializeField]
		private BlendType blend;

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

		protected override void OnModifyOpacity(ref float currentOpacity, float pressure)
		{
			float num = 0f;
			switch (blend)
			{
			case BlendType.Replace:
				num = opacity;
				break;
			case BlendType.Multiply:
				num = currentOpacity * opacity;
				break;
			case BlendType.Increment:
				num = currentOpacity + opacity;
				break;
			}
			currentOpacity += (num - currentOpacity) * pressure;
		}
	}
}
