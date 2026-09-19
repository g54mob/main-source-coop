using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsSaveSystem : IInitializable, IDisposable
	{
		private readonly SettingsWindow _settingsWindow;

		private readonly ISavingService _savingService;

		public SettingsSaveSystem(SettingsWindow settingsWindow, ISavingService savingService)
		{
			_settingsWindow = settingsWindow;
			_savingService = savingService;
		}

		public void Initialize()
		{
			_settingsWindow.OnWindowClosed += SaveSettings;
		}

		public void Dispose()
		{
			_settingsWindow.OnWindowClosed -= SaveSettings;
		}

		private void SaveSettings(Type type)
		{
			if (!(type != typeof(SettingsWindow)))
			{
				_savingService.SaveDataForGroup(SavingGroup.Settings);
			}
		}
	}
}
