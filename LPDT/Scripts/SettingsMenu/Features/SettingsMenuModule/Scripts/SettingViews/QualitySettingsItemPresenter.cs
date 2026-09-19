using System;
using System.Collections.Generic;
using System.Linq;
using Features.DeviceModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class QualitySettingsItemPresenter : PresenterBehaviour<QualitySettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private CurrentSettingsModel _currentSettingsModel;

		private QualitySettingsOptionsConfiguration _qualitySettingsOptionsConfiguration;

		private ILanguageService _languageService;

		private ILocalizationService _localizationService;

		private IDeviceService _deviceService;

		private IReadOnlyList<QualitySetting> _qualitySettings;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel, QualitySettingsOptionsConfiguration qualitySettingsOptionsConfiguration, ILanguageService languageService, ILocalizationService localizationService, IDeviceService deviceService)
		{
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_currentSettingsModel = currentSettingsModel;
			_qualitySettingsOptionsConfiguration = qualitySettingsOptionsConfiguration;
			_languageService = languageService;
			_localizationService = localizationService;
			_deviceService = deviceService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			Initialize();
			QualitySettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Combine(view.OnValueChanged, new Action<int>(OnQualityChanged));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(Initialize));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			QualitySettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Remove(view.OnValueChanged, new Action<int>(OnQualityChanged));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(Initialize));
		}

		private void Initialize()
		{
			_qualitySettings = _qualitySettingsOptionsConfiguration.GetQualitySettings(_deviceService.GetCurrentDevice());
			int currentQualityLevel = (_notAppliedSettingsModel.IsQualityLevelChanged ? _notAppliedSettingsModel.QualityLevel : _currentSettingsModel.QualityLevel);
			int qualityIndex = (from pair in _qualitySettings.Select((QualitySetting setting, int settingIndex) => (setting: setting, settingIndex: settingIndex))
				where pair.setting.QualityOption == currentQualityLevel
				select pair.settingIndex).DefaultIfEmpty(0).First();
			base.View.SetQualityLocalizationKeys(_qualitySettings.Select((QualitySetting setting) => _localizationService.GetLocalizedString(setting.LocalizedKey)).ToList());
			base.View.SetQualityIndex(qualityIndex);
		}

		private void OnQualityChanged(int index)
		{
			_notAppliedSettingsModel.QualityLevel = _qualitySettings[index].QualityOption;
		}
	}
}
