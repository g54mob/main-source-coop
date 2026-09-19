using System;

namespace Google.Apis.Testing
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public class VisibleForTestOnly : Attribute
	{
	}
}
