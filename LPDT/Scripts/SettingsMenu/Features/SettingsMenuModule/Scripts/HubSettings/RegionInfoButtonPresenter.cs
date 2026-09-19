using System;
using System.Linq;
using Features.MultiplayerSessionServices.Scripts;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion.Photon.Realtime;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class RegionInfoButtonPresenter : PresenterBehaviour<RegionInfoButtonViewBase>
	{
		private readonly HubSettingsWindow _settingsWindow;

		private readonly RegionsPingModel _regionsPingModel;

		private readonly MultiplayerModel _multiplayerModel;

		private bool _isSearching;

		private bool _isStartGameProgress;

		public RegionInfoButtonPresenter(HubSettingsWindow settingsWindow, RegionsPingModel regionsPingModel, MultiplayerModel multiplayerModel)
		{
			_settingsWindow = settingsWindow;
			_regionsPingModel = regionsPingModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			UpdateButtonInteractable();
			RegionInfoButtonViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Combine(view.OnClickSettings, new Action(OpenSettingsWindow));
			_regionsPingModel.OnRegionsInfoUpdated += UpdateSelectedRegionPing;
			_regionsPingModel.OnCurrentRegionUpdated += UpdateSelectedRegionPing;
			_multiplayerModel.OnSearchingInProgressChanged += OnSearchingChanged;
			_multiplayerModel.OnStartGameInProgressChanged += OnStartGameProgressChanged;
		}

		protected override void OnDisposed()
		{
			RegionInfoButtonViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Remove(view.OnClickSettings, new Action(OpenSettingsWindow));
			_regionsPingModel.OnRegionsInfoUpdated -= UpdateSelectedRegionPing;
			_regionsPingModel.OnCurrentRegionUpdated -= UpdateSelectedRegionPing;
			_multiplayerModel.OnSearchingInProgressChanged -= OnSearchingChanged;
			_multiplayerModel.OnStartGameInProgressChanged -= OnStartGameProgressChanged;
		}

		private void OnSearchingChanged(bool value)
		{
			_isSearching = value;
			UpdateButtonInteractable();
		}

		private void OnStartGameProgressChanged(bool value)
		{
			_isStartGameProgress = value;
			UpdateButtonInteractable();
		}

		private void UpdateButtonInteractable()
		{
			base.View.SetRegionButtonInteractable(!_isSearching && !_isStartGameProgress);
		}

		private void UpdateSelectedRegionPing()
		{
			string currentRegion = _regionsPingModel.CurrentRegion;
			RegionInfo actualRegionPing = _regionsPingModel.Regions.FirstOrDefault((RegionInfo info) => info.RegionCode == currentRegion);
			base.View.SetActualRegionPing(actualRegionPing);
		}

		private void OpenSettingsWindow()
		{
			switch (_settingsWindow.WindowStatus)
			{
			case WindowStatus.Closed:
				_settingsWindow.Open();
				break;
			case WindowStatus.Hidden:
				_settingsWindow.Show();
				break;
			}
		}
	}
}
