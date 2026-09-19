using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.All)]
	public class LastSupportedVersionAttribute : Attribute
	{
		public LastSupportedVersionAttribute(string version)
		{
		}
	}
}
