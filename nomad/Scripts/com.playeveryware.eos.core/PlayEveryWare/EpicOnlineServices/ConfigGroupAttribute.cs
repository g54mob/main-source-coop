using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ConfigGroupAttribute : Attribute
	{
		public string Label { get; }

		public bool Collapsible { get; }

		public string[] GroupLabels { get; }

		public ConfigGroupAttribute(string label, bool collapsible = true)
		{
			Label = label;
			Collapsible = collapsible;
		}

		public ConfigGroupAttribute(string label, string[] groupLabels, bool collapsible = true)
			: this(label, collapsible)
		{
			GroupLabels = groupLabels;
		}
	}
}
