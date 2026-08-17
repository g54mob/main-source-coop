using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class PlatformDependentConfigFieldAttribute : Attribute
	{
		public string Label { get; }

		public string ToolTip { get; }

		public int Group { get; }

		public ConfigFieldType FieldType { get; }

		public PlatformDependentConfigFieldAttribute(PlatformManager.Platform supportedPlatforms, string label, ConfigFieldType type, string tooltip = null, int group = -1)
		{
			Label = label;
			ToolTip = tooltip;
			Group = group;
			FieldType = type;
		}
	}
}
