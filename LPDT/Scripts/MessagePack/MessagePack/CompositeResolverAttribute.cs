using System;
using System.Diagnostics;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[Conditional("NEVERDEFINED")]
	public sealed class CompositeResolverAttribute : Attribute
	{
		public bool IncludeLocalFormatters { get; set; }

		public CompositeResolverAttribute(params Type[] formattersAndResolvers)
		{
		}
	}
}
