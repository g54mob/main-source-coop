using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public class PluginCodeExportSettingsAttribute : Attribute
	{
		public PluginExportOptions Options { get; }

		public bool ShouldExport => (Options & PluginExportOptions.Export) != 0;

		public PluginCodeExportSettingsAttribute(PluginExportOptions options)
		{
			Options = options;
		}
	}
}
