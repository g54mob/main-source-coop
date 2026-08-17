using System;
using UnityEngine;

namespace NWH.Common.Vehicles
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/Friction Preset", order = 1)]
	public class FrictionPreset : ScriptableObject
	{
		public const int LUT_RESOLUTION = 1000;

		[Tooltip("    B, C, D and E parameters of short version of Pacejka's magic formula.")]
		public Vector4 BCDE;

		[Tooltip("Slip at which the friction preset has highest friction.")]
		public float peakSlip = 0.12f;

		[SerializeField]
		private AnimationCurve _curve;

		public AnimationCurve Curve => _curve;

		public float GetPeakSlip()
		{
			float result = -1f;
			float num = 0f;
			for (float num2 = 0f; num2 < 1f; num2 += 0.01f)
			{
				float num3 = _curve.Evaluate(num2);
				if (num3 > num)
				{
					num = num3;
					result = num2;
				}
			}
			return result;
		}

		public void UpdateFrictionCurve()
		{
			_curve = new AnimationCurve();
			int num = new Keyframe[20].Length;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				float frictionValue = GetFrictionValue(num2, BCDE);
				_curve.AddKey(num2, frictionValue);
				num2 = ((i > 10) ? (num2 + 0.1f) : (num2 + 0.02f));
			}
			for (int j = 0; j < num; j++)
			{
				_curve.SmoothTangents(j, 0f);
			}
			peakSlip = GetPeakSlip();
		}

		private static float GetFrictionValue(float slip, Vector4 p)
		{
			float x = p.x;
			float y = p.y;
			float z = p.z;
			float w = p.w;
			float num = Mathf.Abs(slip);
			return z * Mathf.Sin(y * Mathf.Atan(x * num - w * (x * num - Mathf.Atan(x * num))));
		}
	}
}
