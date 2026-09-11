using System;

namespace DunGen
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class FloatRangeLimitAttribute : Attribute
	{
		public float MinLimit { get; private set; }

		public float MaxLimit { get; private set; }

		public FloatRangeLimitAttribute(float minLimit, float maxLimit)
		{
			MinLimit = minLimit;
			MaxLimit = maxLimit;
		}
	}
}
