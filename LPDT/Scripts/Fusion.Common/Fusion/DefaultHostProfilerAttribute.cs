using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Assembly)]
	[Obsolete("No longer userd")]
	public class DefaultHostProfilerAttribute : Attribute
	{
		public readonly Type ProfilerType;

		public DefaultHostProfilerAttribute(Type profilerType)
		{
			ProfilerType = profilerType;
		}
	}
}
