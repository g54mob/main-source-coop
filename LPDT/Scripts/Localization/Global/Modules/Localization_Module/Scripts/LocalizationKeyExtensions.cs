using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Global.Modules.Localization_Module.Scripts
{
	public static class LocalizationKeyExtensions
	{
		public static string GetLocalized(this LocalizationKey localizationKey, ILocalizationService localizationService)
		{
			return localizationService?.GetLocalizedString(localizationKey) ?? string.Empty;
		}
	}
}
