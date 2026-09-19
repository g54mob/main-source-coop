using System;
using System.Diagnostics;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[Conditional("NEVERDEFINED")]
	public class GeneratedMessagePackResolverAttribute : Attribute
	{
		public bool UseMapMode { get; set; }
	}
}
