using System;
using UnityEngine;

namespace Coffee.UIEffects.Timeline
{
	[AttributeUsage(AttributeTargets.Class)]
	public class FloatClipUsageAttribute : PropertyAttribute
	{
		public readonly float min;

		public readonly float max;

		public readonly float defaultValue;

		public FloatClipUsageAttribute(float min, float max, float defaultValue = 0f)
		{
			this.min = min;
			this.max = max;
			this.defaultValue = defaultValue;
		}
	}
}
