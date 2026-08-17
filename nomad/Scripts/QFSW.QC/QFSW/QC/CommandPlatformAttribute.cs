using System;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class CommandPlatformAttribute : Attribute
	{
		public readonly Platform SupportedPlatforms;

		public CommandPlatformAttribute(Platform supportedPlatforms)
		{
			SupportedPlatforms = supportedPlatforms;
		}
	}
}
