using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyRadiusRandom")]
	public class CwModifyRadiusRandom : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Radius";

		public static string Title = "Random";

		[SerializeField]
		private float min = 0.6666f;

		[SerializeField]
		private float max = 1.5f;

		[SerializeField]
		private BlendType blend;

		public float Min
		{
			get
			{
				return min;
			}
			set
			{
				min = value;
			}
		}

		public float Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
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

		protected override void OnModifyRadius(ref float radius, float pressure)
		{
			float num = UnityEngine.Random.Range(min, max);
			switch (blend)
			{
			case BlendType.Replace:
				radius = num;
				break;
			case BlendType.Multiply:
				radius *= num;
				break;
			case BlendType.Increment:
				radius += num;
				break;
			}
		}
	}
}
