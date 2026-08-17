using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ConfigFieldAttribute : Attribute
	{
		public string Label { get; }

		public string ToolTip { get; set; }

		public int Group { get; }

		public string HelpURL { get; }

		public ConfigFieldType FieldType { get; }

		public PlatformManager.Platform PlatformsEnabledOn { get; }

		public ConfigFieldAttribute(PlatformManager.Platform enabledOn, string label, ConfigFieldType type, string tooltip = null, int group = -1, string helpUrl = null)
			: this(label, type, tooltip, group, helpUrl)
		{
			PlatformsEnabledOn = enabledOn;
		}

		public ConfigFieldAttribute(string label, ConfigFieldType type, string tooltip = null, int group = -1, string helpUrl = null)
		{
			PlatformsEnabledOn = PlatformManager.Platform.Any;
			HelpURL = helpUrl;
			Label = label;
			ToolTip = tooltip;
			Group = group;
			FieldType = type;
		}
	}
}
