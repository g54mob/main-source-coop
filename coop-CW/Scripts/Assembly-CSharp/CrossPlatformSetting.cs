using System.Collections.Generic;
using Portningsbolaget.Platforms;
using Zorro.Settings;

public class CrossPlatformSetting : EnumSetting, IExposedSetting
{
	private int m_default = 1;

	public CrossPlatformSetting()
	{
		PlatformUtility.OnLoadedSettings += OnLoadedServerSettings;
		ApplyValue();
	}

	public override void Dispose()
	{
		base.Dispose();
		PlatformUtility.OnLoadedSettings -= OnLoadedServerSettings;
	}

	private void OnLoadedServerSettings(bool crossplay)
	{
		ApplyValue();
	}

	private void UpdateDefault()
	{
		m_default = 1;
		if (m_default == 1)
		{
			m_default = (PlatformUtility.AllowCrossplay ? 1 : 0);
		}
	}

	public override void ApplyValue()
	{
		UpdateDefault();
		MainMenuHandler.CrossPlatform = base.Value != 0 && m_default != 0;
	}

	public override int GetDefaultValue()
	{
		return m_default;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.OnSetting);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.OffSetting);
		return new List<string> { localizedString2, localizedString };
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Matchmaking;
	}

	public string GetDisplayName()
	{
		return "Cross-Platform";
	}
}
