using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PluginAssetExportSettingsAttribute : Attribute
	{
		public PluginExportOptions Options => _003Coptions_003EP;

		public PluginAssetExportSettingsAttribute(PluginExportOptions options)
		{
			_003Coptions_003EP = options;
			base._002Ector();
		}
	}
}
