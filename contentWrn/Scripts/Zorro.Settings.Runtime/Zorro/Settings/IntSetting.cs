using UnityEngine;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class IntSetting : Setting
	{
		public int Value { get; protected set; }

		public override void Load(ISettingsSaveLoad loader)
		{
			if (loader.TryLoadInt(GetType(), out var o))
			{
				Value = o;
				return;
			}
			Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
			Value = GetDefaultValue();
		}

		public override void Save(ISettingsSaveLoad saver)
		{
			saver.SaveInt(GetType(), Value);
		}

		public abstract int GetDefaultValue();

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new IntSettingUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.IntSettingCell;
		}

		public void SetValue(int newValue, ISettingHandler settingHandler)
		{
			Value = newValue;
			ApplyValue();
			settingHandler.SaveSetting(this);
		}

		public virtual string Expose(int result)
		{
			return result.ToString();
		}
	}
}
