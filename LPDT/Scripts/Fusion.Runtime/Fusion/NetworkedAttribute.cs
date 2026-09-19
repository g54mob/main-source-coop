using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class NetworkedAttribute : Attribute
	{
		public string Default { get; set; }

		public bool PluginAuthority { get; set; }

		public bool AllowPrediction { get; set; } = true;
	}
}
