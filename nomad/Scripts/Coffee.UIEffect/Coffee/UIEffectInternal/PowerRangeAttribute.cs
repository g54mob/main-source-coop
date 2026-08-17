using System;
using System.Diagnostics;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	public sealed class PowerRangeAttribute : PropertyAttribute
	{
		public readonly float min;

		public readonly float max;

		public readonly float power;

		public PowerRangeAttribute(float min, float max, float power)
		{
			this.min = min;
			this.max = max;
			this.power = power;
		}
	}
}
