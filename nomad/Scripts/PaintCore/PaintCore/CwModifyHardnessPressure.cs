using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyHardnessPressure")]
	public class CwModifyHardnessPressure : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Hardness";

		public static string Title = "Pressure";

		[SerializeField]
		private float hardness = 1f;

		[SerializeField]
		private BlendType blend;

		public float Hardness
		{
			get
			{
				return hardness;
			}
			set
			{
				hardness = value;
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

		protected override void OnModifyHardness(ref float currentHardness, float pressure)
		{
			float num = 0f;
			switch (blend)
			{
			case BlendType.Replace:
				num = hardness;
				break;
			case BlendType.Multiply:
				num = currentHardness * hardness;
				break;
			case BlendType.Increment:
				num = currentHardness + hardness;
				break;
			}
			currentHardness += (num - currentHardness) * pressure;
		}
	}
}
