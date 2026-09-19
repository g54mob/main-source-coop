using System;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Global.Modules.Localization_Module.Scripts
{
	public interface ILanguageService
	{
		Action OnLanguageChanged { get; set; }

		Action OnPreLanguageChanged { get; set; }

		Language GetCurrentLanguage();

		void SetCurrentLanguage(Language language);
	}
}
