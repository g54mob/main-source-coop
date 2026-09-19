using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class AutoLanguageDetectionSystem : IInitializable
	{
		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly ISettingsService _settingsService;

		private readonly IRegionLanguageDetectionService _detectionService;

		public AutoLanguageDetectionSystem(CurrentSettingsModel currentSettingsModel, ISettingsService settingsService, IRegionLanguageDetectionService detectionService)
		{
			_currentSettingsModel = currentSettingsModel;
			_settingsService = settingsService;
			_detectionService = detectionService;
		}

		public void Initialize()
		{
			if (!_currentSettingsModel.HasSavedSettings)
			{
				Language language = _detectionService.DetectLanguage();
				_currentSettingsModel.Language = language;
			}
			_settingsService.SetLanguage(_currentSettingsModel.Language);
		}
	}
}
