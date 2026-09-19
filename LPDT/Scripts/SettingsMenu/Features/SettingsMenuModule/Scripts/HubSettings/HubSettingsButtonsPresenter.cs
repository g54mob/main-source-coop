using System;
using Features.InputModule.Scripts.Generated;
using Features.MultiplayerSessionServices.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	[PublicAPI]
	public class HubSettingsButtonsPresenter : PresenterBehaviour<HubSettingsButtonsViewBase>
	{
		private readonly HubSettingsWindow _settingsWindow;

		private readonly IInputService _inputService;

		private readonly MultiplayerModel _multiplayerModel;

		private bool _startGameInProgress;

		private bool _searchingInProgress;

		public HubSettingsButtonsPresenter(HubSettingsWindow settingsWindow, IInputService inputService, MultiplayerModel multiplayerModel)
		{
			_settingsWindow = settingsWindow;
			_inputService = inputService;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			HubSettingsButtonsViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Combine(view.OnClickSettings, new Action(OpenSettingsWindow));
			_multiplayerModel.OnStartGameInProgressChanged += OnStartGameInProgress;
			_multiplayerModel.OnSearchingInProgressChanged += OnSearchingInProgress;
		}

		protected override void OnDisposed()
		{
			HubSettingsButtonsViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Remove(view.OnClickSettings, new Action(OpenSettingsWindow));
			_multiplayerModel.OnStartGameInProgressChanged -= OnStartGameInProgress;
			_multiplayerModel.OnSearchingInProgressChanged -= OnSearchingInProgress;
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

		private void OnStartGameInProgress(bool value)
		{
			_startGameInProgress = value;
			UpdateOpenSettingsInteractable();
		}

		private void OnSearchingInProgress(bool value)
		{
			_searchingInProgress = value;
			UpdateOpenSettingsInteractable();
		}

		private void UpdateOpenSettingsInteractable()
		{
			base.View.SetOpenSettingsButtonInteractable(!_startGameInProgress && !_searchingInProgress);
		}
	}
}
