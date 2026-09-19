using System;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Global.Modules.Localization_Module.Scripts
{
	public class LanguageService : ILanguageService
	{
		private Language _currentLanguage;

		public Action OnLanguageChanged { get; set; }

		public Action OnPreLanguageChanged { get; set; }

		public Language GetCurrentLanguage()
		{
			return _currentLanguage;
		}

		public void SetCurrentLanguage(Language language)
		{
			_currentLanguage = language;
			OnPreLanguageChanged?.Invoke();
		}
	}
}
