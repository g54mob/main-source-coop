using System;
using Global.Modules.Localization_Module.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class LocalizationSettingsItemPresenter : PresenterBehaviour<LocalizationSettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private LanguagesLocalizationConfiguration _languageLocalizationConfiguration;

		private ILanguageService _languageService;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, LanguagesLocalizationConfiguration languageLocalizationConfiguration, ILanguageService languageService)
		{
			_languageService = languageService;
			_languageLocalizationConfiguration = languageLocalizationConfiguration;
			_notAppliedSettingsModel = notAppliedSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			InitValue();
			LocalizationSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Combine(view.OnValueChanged, new Action<int>(OnLocalizationChanged));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			LocalizationSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<int>)Delegate.Remove(view.OnValueChanged, new Action<int>(OnLocalizationChanged));
		}

		private void InitValue()
		{
			int valueIndex = (_notAppliedSettingsModel.IsLanguageChanged ? _notAppliedSettingsModel.Language : _languageLocalizationConfiguration.GetLanguageIndex(_languageService.GetCurrentLanguage()));
			base.View.SetValueIndex(valueIndex);
		}

		private void OnLocalizationChanged(int languageIndex)
		{
			_notAppliedSettingsModel.Language = languageIndex;
		}
	}
}
