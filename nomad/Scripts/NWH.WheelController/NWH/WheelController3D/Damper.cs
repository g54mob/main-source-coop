using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.WheelController3D
{
	[Serializable]
	public class Damper
	{
		[FormerlySerializedAs("maxBumpForce")]
		public float bumpRate = 3000f;

		[FormerlySerializedAs("maxReboundForce")]
		public float reboundRate = 3000f;

		[Range(0f, 3f)]
		public float slowBump = 1.4f;

		[Range(0f, 3f)]
		public float fastBump = 0.6f;

		[Range(0f, 0.2f)]
		public float bumpDivisionVelocity = 0.06f;

		[Range(0f, 3f)]
		public float slowRebound = 1.6f;

		[Range(0f, 3f)]
		public float fastRebound = 0.6f;

		[Range(0f, 0.2f)]
		public float reboundDivisionVelocity = 0.05f;

		[Tooltip("    Current damper force.")]
		public float force;

		public float CalculateDamperForce(in float velocity)
		{
			if (velocity > 0f)
			{
				return CalculateBumpForce(in velocity);
			}
			return CalculateReboundForce(in velocity);
		}

		private float CalculateBumpForce(in float velocity)
		{
			if (velocity < 0f)
			{
				return 0f;
			}
			float num = velocity;
			float num2 = ((!(num < bumpDivisionVelocity)) ? (bumpDivisionVelocity * slowBump + (num - bumpDivisionVelocity) * fastBump) : (num * slowBump));
			return num2 * bumpRate;
		}

		private float CalculateReboundForce(in float velocity)
		{
			if (velocity > 0f)
			{
				return 0f;
			}
			float num = 0f - velocity;
			float num2 = ((!(num < reboundDivisionVelocity)) ? (reboundDivisionVelocity * slowRebound + (num - reboundDivisionVelocity) * fastRebound) : (num * slowRebound));
			return (0f - num2) * reboundRate;
		}
	}
}
