using System;
using System.Linq;
using Features.EnumHelpersModule.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ScreenModeSettingsItemPresenter : PresenterBehaviour<ScreenModeSettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private CurrentSettingsModel _currentSettingsModel;

		private IEnumValuesProvider _enumValuesProvider;

		private ScreenModeLocalizationConfiguration _screenModeLocalizationConfiguration;

		private ILanguageService _languageService;

		private ILocalizationService _localizationService;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel, IEnumValuesProvider enumValuesProvider, ScreenModeLocalizationConfiguration screenModeLocalizationConfiguration, ILocalizationService localizationService, ILanguageService languageService)
		{
			_screenModeLocalizationConfiguration = screenModeLocalizationConfiguration;
			_enumValuesProvider = enumValuesProvider;
			_currentSettingsModel = currentSettingsModel;
			_notAppliedSettingsModel = notAppliedSettingsModel;
			_languageService = languageService;
			_localizationService = localizationService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			InitValue();
			ScreenModeSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Combine(view.OnValueChanged, new Action<int>(OnScreenModeChanged));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(InitValue));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			ScreenModeSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Remove(view.OnValueChanged, new Action<int>(OnScreenModeChanged));
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(InitValue));
		}

		private void InitValue()
		{
			base.View.SetScreenModeDropdownValues((from screenMode in _enumValuesProvider.GetAllValues<ScreenMode>()
				select _localizationService.GetLocalizedString(_screenModeLocalizationConfiguration.GetLocalizationKeyForScreenMode(screenMode))).ToList());
			int screenModeDropdownValue = (_notAppliedSettingsModel.IsScreenModeChanged ? _notAppliedSettingsModel.ScreenMode : ((int)_currentSettingsModel.ScreenMode));
			base.View.SetScreenModeDropdownValue(screenModeDropdownValue);
		}

		private void OnScreenModeChanged(int screenMode)
		{
			_notAppliedSettingsModel.ScreenMode = screenMode;
		}
	}
}
