using UnityEngine;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class StringSetting : Setting
	{
		public string Value { get; protected set; }

		public override void Load(ISettingsSaveLoad loader)
		{
			if (loader.TryGetString(GetType(), out var value))
			{
				Value = value;
				return;
			}
			Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
			Value = GetDefaultValue();
		}

		public override void Save(ISettingsSaveLoad saver)
		{
			saver.SaveString(GetType(), Value);
		}

		protected abstract string GetDefaultValue();

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new StringSettingUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.StringSettingCell;
		}

		public void SetValue(string newValue, ISettingHandler settingHandler)
		{
			Value = newValue;
			ApplyValue();
			settingHandler.SaveSetting(this);
		}
	}
}
