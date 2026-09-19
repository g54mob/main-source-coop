using Global.Modules.LocalizationModule.Scripts.Generated;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[CreateAssetMenu(fileName = "ScreenModeLocalizationConfiguration_Default", menuName = "Configurations/SettingsMenuModule/ScreenModeLocalizationConfiguration")]
	public class ScreenModeLocalizationConfiguration : ScriptableObject
	{
		[SerializeField]
		private LocalizationKey _defaultValue;

		[SerializeField]
		private SerializableDictionary<ScreenMode, LocalizationKey> _screenModeLocalizationKeys;

		public LocalizationKey GetLocalizationKeyForScreenMode(ScreenMode screenMode)
		{
			if (_screenModeLocalizationKeys.TryGetValue(screenMode, out var value))
			{
				return value;
			}
			return _defaultValue;
		}
	}
}
