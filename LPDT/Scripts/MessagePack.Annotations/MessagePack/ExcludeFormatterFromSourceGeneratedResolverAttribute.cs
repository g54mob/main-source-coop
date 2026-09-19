using System;
using System.Diagnostics;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class)]
	[Conditional("NEVERDEFINED")]
	public class ExcludeFormatterFromSourceGeneratedResolverAttribute : Attribute
	{
	}
}
