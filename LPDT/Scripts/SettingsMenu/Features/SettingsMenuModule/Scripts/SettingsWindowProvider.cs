using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsWindowProvider : ISettingsWindowProvider
	{
		private readonly SettingsWindow _settingsWindow;

		private readonly MenuSettingsWindow _menuSettingsWindow;

		private readonly SteamDeckSettingsWindow _steamDeckSettingsWindow;

		private readonly SteamDeckMenuSettingsWindow _steamDeckMenuSettingsWindow;

		private readonly IDeviceService _deviceService;

		public SettingsWindowProvider(SettingsWindow settingsWindow, MenuSettingsWindow menuSettingsWindow, SteamDeckSettingsWindow steamDeckSettingsWindow, SteamDeckMenuSettingsWindow steamDeckMenuSettingsWindow, IDeviceService deviceService)
		{
			_settingsWindow = settingsWindow;
			_menuSettingsWindow = menuSettingsWindow;
			_steamDeckSettingsWindow = steamDeckSettingsWindow;
			_steamDeckMenuSettingsWindow = steamDeckMenuSettingsWindow;
			_deviceService = deviceService;
		}

		public FocusableWindowBehaviour GetSettingsWindow(bool isMenu)
		{
			bool flag = _deviceService.GetCurrentDevice() == DeviceType.SteamDeck;
			if (isMenu)
			{
				if (!flag)
				{
					return _menuSettingsWindow;
				}
				return _steamDeckMenuSettingsWindow;
			}
			if (!flag)
			{
				return _settingsWindow;
			}
			return _steamDeckSettingsWindow;
		}

		public bool IsAnySettingsWindowShown()
		{
			if (_settingsWindow.WindowStatus != WindowStatus.Showed && _menuSettingsWindow.WindowStatus != WindowStatus.Showed && _steamDeckSettingsWindow.WindowStatus != WindowStatus.Showed)
			{
				return _steamDeckMenuSettingsWindow.WindowStatus == WindowStatus.Showed;
			}
			return true;
		}
	}
}
