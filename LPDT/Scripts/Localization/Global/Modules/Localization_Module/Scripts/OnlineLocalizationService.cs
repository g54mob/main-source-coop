using System.Linq;
using ArabicSupport;
using Global.Modules.LocalizationModule.Scripts.Generated;

namespace Global.Modules.Localization_Module.Scripts
{
	public class OnlineLocalizationService : ILocalizationService
	{
		private readonly LocalizationBatchModel _localizationBatchModel;

		private readonly LanguageMapper _languageMapper;

		private readonly ILanguageService _languageService;

		private readonly LanguagesLocalizationConfiguration _languagesLocalizationConfiguration;

		public OnlineLocalizationService(LocalizationBatchModel localizationBatchModel, LanguageMapper languageMapper, ILanguageService languageService, LanguagesLocalizationConfiguration languagesLocalizationConfiguration)
		{
			_localizationBatchModel = localizationBatchModel;
			_languageMapper = languageMapper;
			_languageService = languageService;
			_languagesLocalizationConfiguration = languagesLocalizationConfiguration;
		}

		public string GetLocalizedString(LocalizationKey id)
		{
			Language currentLanguage = _languageService.GetCurrentLanguage();
			string text = _languageMapper.GetStringByLanguage(_localizationBatchModel.LocalizationDataHolders.First((ILocalizationDataHolder tableData) => tableData.ID == (int)id), currentLanguage);
			if (_languagesLocalizationConfiguration.ArabicLanguages.Contains(currentLanguage))
			{
				text = ArabicFixer.Fix(text, showTashkeel: false, useHinduNumbers: false);
			}
			return text;
		}
	}
}
