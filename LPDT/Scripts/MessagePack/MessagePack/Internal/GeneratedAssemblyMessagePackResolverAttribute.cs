using System;

namespace MessagePack.Internal
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public class GeneratedAssemblyMessagePackResolverAttribute : Attribute
	{
		public Type ResolverType { get; }

		public int MajorVersion { get; }

		public int MinorVersion { get; }

		public GeneratedAssemblyMessagePackResolverAttribute(Type resolverType, int majorVersion, int minorVersion)
		{
			ResolverType = resolverType;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
		}
	}
}
