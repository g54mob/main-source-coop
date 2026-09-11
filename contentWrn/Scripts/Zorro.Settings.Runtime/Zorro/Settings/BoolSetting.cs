using UnityEngine;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class BoolSetting : Setting
	{
		public bool Value { get; protected set; }

		public override void Load(ISettingsSaveLoad loader)
		{
			if (loader.TryLoadBool(GetType(), out var o))
			{
				Value = o;
				return;
			}
			Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
			Value = GetDefaultValue();
		}

		public override void Save(ISettingsSaveLoad saver)
		{
			saver.SaveBool(GetType(), Value);
		}

		protected abstract bool GetDefaultValue();

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new BoolSettingUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.BoolSettingCell;
		}

		public void SetValue(bool newValue, ISettingHandler settingHandler)
		{
			Value = newValue;
			ApplyValue();
			settingHandler.SaveSetting(this);
		}
	}
}
