using System;

namespace Fusion.Analyzer
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	internal class CanMutateAttribute : Attribute
	{
	}
}
