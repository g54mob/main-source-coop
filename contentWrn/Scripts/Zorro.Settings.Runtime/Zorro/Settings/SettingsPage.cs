using System.Collections.Generic;
using Zorro.Core.CLI;

namespace Zorro.Settings
{
	public class SettingsPage : DebugPage
	{
		private List<Setting> m_settings;

		public SettingsPage(List<Setting> settings, ISettingHandler settingHandler)
		{
			m_settings = settings;
			foreach (Setting setting in m_settings)
			{
				if (!(setting is KeyCodeSetting))
				{
					DrawSettings(setting, settingHandler);
				}
			}
		}

		private void DrawSettings(Setting setting, ISettingHandler settingHandler)
		{
			Add(setting.GetDebugUI(settingHandler));
		}
	}
}
