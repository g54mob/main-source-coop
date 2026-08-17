using JetBrains.Annotations;

namespace PrimeTween
{
	internal static class Constants
	{
		[NotNull]
		internal static string buildWarningCanBeDisabledMessage(string settingName)
		{
			return "To disable this warning, set 'PrimeTweenConfig." + settingName + " = false;'.";
		}
	}
}
