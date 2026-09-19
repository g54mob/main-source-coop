using System;
using System.Collections.Generic;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion.Photon.Realtime;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	[PublicAPI]
	public class LobbySettingsItemPresenter : PresenterBehaviour<LobbySettingsItemViewBase>
	{
		private RegionsPingModel _regionsPingModel;

		private IReadOnlyList<RegionInfo> _lastRegionsInfo;

		private ILocalizationService _localizationService;

		private ILanguageService _languageService;

		public LobbySettingsItemPresenter(RegionsPingModel regionsPingModel, IReadOnlyList<RegionInfo> lastRegionsInfo)
		{
			_regionsPingModel = regionsPingModel;
			_lastRegionsInfo = lastRegionsInfo;
		}

		[Inject]
		public void InjectDependencies(ILocalizationService localizationService, ILanguageService languageService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			LobbySettingsItemViewBase view = base.View;
			view.OnRegionChanged = (Action<string>)Delegate.Combine(view.OnRegionChanged, new Action<string>(SelectRoomRegion));
			LobbySettingsItemViewBase view2 = base.View;
			view2.OnDropdownShown = (Action)Delegate.Combine(view2.OnDropdownShown, new Action(PlayShownSound));
			_regionsPingModel.OnRegionsInfoUpdated += UpdateRegionsPingInfo;
			_regionsPingModel.OnRegionsSearchInProgressChanged += OnRegionsSearchInProgressChanged;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Combine(languageService.OnLanguageChanged, new Action(UpdateRegionSearchState));
			UpdateRegionSearchState();
			UpdateRegionsPingInfo();
		}

		private void UpdateRegionsPingInfo()
		{
			if (!_regionsPingModel.IsRegionsSearchInProgress)
			{
				base.View.SetRegionsInfo(_regionsPingModel.Regions);
				base.View.SelectCurrentRegion(_regionsPingModel.CurrentRegion);
			}
		}

		private void OnRegionsSearchInProgressChanged(bool isInProgress)
		{
			UpdateRegionSearchState();
		}

		private void UpdateRegionSearchState()
		{
			if (_regionsPingModel.IsRegionsSearchInProgress)
			{
				base.View.SetRegionSearchInProgress(isInProgress: true, _localizationService.GetLocalizedString(LocalizationKey.Ui_Menu_RegionLoading));
				return;
			}
			base.View.SetRegionSearchInProgress(isInProgress: false, null);
			UpdateRegionsPingInfo();
		}

		protected override void OnDisposed()
		{
			LobbySettingsItemViewBase view = base.View;
			view.OnRegionChanged = (Action<string>)Delegate.Remove(view.OnRegionChanged, new Action<string>(SelectRoomRegion));
			_regionsPingModel.OnRegionsInfoUpdated -= UpdateRegionsPingInfo;
			_regionsPingModel.OnRegionsSearchInProgressChanged -= OnRegionsSearchInProgressChanged;
			ILanguageService languageService = _languageService;
			languageService.OnLanguageChanged = (Action)Delegate.Remove(languageService.OnLanguageChanged, new Action(UpdateRegionSearchState));
			LobbySettingsItemViewBase view2 = base.View;
			view2.OnDropdownShown = (Action)Delegate.Remove(view2.OnDropdownShown, new Action(PlayShownSound));
		}

		private void PlayShownSound()
		{
		}

		private void SelectRoomRegion(string regionCode)
		{
			_regionsPingModel.CurrentRegion = regionCode;
			PhotonAppSettings.Global.AppSettings.FixedRegion = regionCode;
		}
	}
}
