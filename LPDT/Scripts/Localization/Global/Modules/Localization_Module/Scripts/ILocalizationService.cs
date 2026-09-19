using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Global.Modules.Localization_Module.Scripts
{
	public interface ILocalizationService
	{
		string GetLocalizedString(LocalizationKey id);
	}
}
