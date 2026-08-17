using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyAngleRandom")]
	public class CwModifyAngleRandom : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Angle";

		public static string Title = "Random";

		[SerializeField]
		private float min = -180f;

		[SerializeField]
		private float max = 180f;

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

		protected override void OnModifyAngle(ref float angle, float pressure)
		{
			float num = UnityEngine.Random.Range(min, max);
			switch (blend)
			{
			case BlendType.Replace:
				angle = num;
				break;
			case BlendType.Multiply:
				angle *= num;
				break;
			case BlendType.Increment:
				angle += num;
				break;
			}
		}
	}
}
