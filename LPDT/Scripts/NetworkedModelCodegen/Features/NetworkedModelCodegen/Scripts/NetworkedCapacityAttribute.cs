using System;

namespace Features.NetworkedModelCodegen.Scripts
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class NetworkedCapacityAttribute : Attribute
	{
		public int Capacity { get; }

		public NetworkedCapacityAttribute(int capacity)
		{
			Capacity = capacity;
		}
	}
}
