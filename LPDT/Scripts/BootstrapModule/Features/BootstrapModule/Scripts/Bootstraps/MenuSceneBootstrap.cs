using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Core;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SettingsMenuModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public class MenuSceneBootstrap : MonoBehaviour
	{
		private IWindowsService _windowsService;

		private SettingsWindow _settingsWindow;

		private GameAnalyticsEventSendService _gameAnalyticsEventSendService;

		private ILoadingScreenService _loadingScreenService;

		[Inject]
		public void InjectDependencies(IWindowsService windowsService, SettingsWindow settingsWindow, GameAnalyticsEventSendService gameAnalyticsEventSendService, ILoadingScreenService loadingScreenService)
		{
			_windowsService = windowsService;
			_settingsWindow = settingsWindow;
			_gameAnalyticsEventSendService = gameAnalyticsEventSendService;
			_loadingScreenService = loadingScreenService;
		}

		private async void Start()
		{
			await _loadingScreenService.PrepareMenuSceneEntranceAsync();
			_windowsService.OpenWindow<MenuWindow>();
			if (_settingsWindow.WindowStatus == WindowStatus.Showed)
			{
				_settingsWindow.Close();
			}
			_gameAnalyticsEventSendService.TrackMainMenuOpened();
		}
	}
}
