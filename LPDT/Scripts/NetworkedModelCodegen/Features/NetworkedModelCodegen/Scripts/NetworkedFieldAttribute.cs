using System;

namespace Features.NetworkedModelCodegen.Scripts
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class NetworkedFieldAttribute : Attribute
	{
	}
}
