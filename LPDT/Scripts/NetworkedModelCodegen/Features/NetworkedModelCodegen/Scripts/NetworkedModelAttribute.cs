using System;

namespace Features.NetworkedModelCodegen.Scripts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class NetworkedModelAttribute : Attribute
	{
		public ModelScope Scope { get; }

		public ModelOwnership Ownership { get; }

		public NetworkedModelAttribute(ModelScope scope, ModelOwnership ownership)
		{
			Scope = scope;
			Ownership = ownership;
		}
	}
}
