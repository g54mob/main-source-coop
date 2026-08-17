using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyScaleRandom")]
	public class CwModifyScaleRandom : CwModifier
	{
		public enum BlendType
		{
			Replace = 0,
			Multiply = 1,
			Increment = 2
		}

		public static string Group = "Scale";

		public static string Title = "Random";

		[SerializeField]
		private Vector3 min = new Vector3(0.6666f, 0.6666f, 0.6666f);

		[SerializeField]
		private Vector3 max = new Vector3(1.5f, 1.5f, 1.5f);

		[SerializeField]
		private BlendType blend;

		[SerializeField]
		private bool uniform;

		public Vector3 Min
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

		public Vector3 Max
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

		public bool Uniform
		{
			get
			{
				return uniform;
			}
			set
			{
				uniform = value;
			}
		}

		protected override void OnModifyScale(ref Vector3 scale, float pressure)
		{
			Vector3 vector = default(Vector3);
			if (uniform)
			{
				vector = Vector3.LerpUnclamped(min, max, UnityEngine.Random.value);
			}
			else
			{
				vector.x = Mathf.LerpUnclamped(min.x, max.x, UnityEngine.Random.value);
				vector.y = Mathf.LerpUnclamped(min.y, max.y, UnityEngine.Random.value);
				vector.z = Mathf.LerpUnclamped(min.z, max.z, UnityEngine.Random.value);
			}
			switch (blend)
			{
			case BlendType.Replace:
				scale = vector;
				break;
			case BlendType.Multiply:
				scale.x *= vector.x;
				scale.y *= vector.y;
				scale.z *= vector.z;
				break;
			case BlendType.Increment:
				scale += vector;
				break;
			}
		}
	}
}
