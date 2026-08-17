using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyRadiusPressure")]
	public class CwModifyRadiusPressure : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Radius";

		public static string Title = "Pressure";

		[SerializeField]
		private float radius = 1f;

		[SerializeField]
		private BlendType blend;

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
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

		protected override void OnModifyRadius(ref float currentRadius, float pressure)
		{
			float num = 0f;
			switch (blend)
			{
			case BlendType.Replace:
				num = radius;
				break;
			case BlendType.Multiply:
				num = currentRadius * radius;
				break;
			case BlendType.Increment:
				num = currentRadius + radius;
				break;
			}
			currentRadius += (num - currentRadius) * pressure;
		}
	}
}
